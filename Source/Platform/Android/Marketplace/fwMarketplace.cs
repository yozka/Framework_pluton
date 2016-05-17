using System;
using System.Collections.Generic;


namespace Pluton.SystemProgram.Devices
{

    ///--------------------------------------------------------------------------------------
    using Xamarin.InAppBilling;
    using Xamarin.InAppBilling.Utilities;

    using Android.App;
    using Android.OS;
    using Android.Content;
    ///--------------------------------------------------------------------------------------














    ///=====================================================================================
    ///
    /// <summary>
    /// Система обслуживание магазина
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AMarketplace
    {
        ///--------------------------------------------------------------------------------------
        private readonly List<string> mProductID = new List<string>();

        private int mPurchase = 0; //количество покупок



        private InAppBillingServiceConnection mServiceConnection = null; //сервис магазина
        private readonly Dictionary<string, Product> mProducts = new Dictionary<string, Product>();
        private readonly Dictionary<string, Purchase> mPurchases = new Dictionary<string, Purchase>();

        private bool mError = false; //сама ошибка
        private readonly Dictionary<string, string> mLastError = new Dictionary<string, string>(); //описание ошибки


        private EState mState = EState.none; //текущее состояние магазина
        ///--------------------------------------------------------------------------------------
        private enum EState
        {
            none,       //неинцеализирован
            loading,    //идет загрузка цен
            completed   //загрузка завершена
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AMarketplace()
        {
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// инциализация магазина
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void create(Activity activity, string publicKey)
        {
             mServiceConnection = new InAppBillingServiceConnection(activity, publicKey);
      
   /* чото этот код не работает
            Intent serviceIntent = new Intent("com.android.vending.billing.InAppBillingService.BIND");
            serviceIntent.SetPackage("com.android.vending");

            //GetApplicationContext().bindService(serviceIntent, connection, Context.MODE_PRIVATE);
            Context bbs = activity.ApplicationContext;

            bool rt =  bbs.BindService(serviceIntent, mServiceConnection, Bind.AutoCreate);
 */



            mServiceConnection.OnConnected += () =>
            {
                mError = false;
                
                // Attach to the various error handlers to report issues
                mServiceConnection.BillingHandler.OnGetProductsError += (int responseCode, Bundle ownedItems) =>
                {
                    Console.WriteLine("Error getting products");
                };

                mServiceConnection.BillingHandler.OnInvalidOwnedItemsBundleReturned += (Bundle ownedItems) =>
                {
                    Console.WriteLine("Invalid owned items bundle returned");
                };

                mServiceConnection.BillingHandler.OnProductPurchasedError += (int responseCode, string sku) =>
                {
                    Console.WriteLine("Error purchasing item {0}", sku);
                    mPurchases[sku] = null;
                    mLastError[sku] = "Error purchasing";
                    mError = true;
                };

                mServiceConnection.BillingHandler.OnPurchaseConsumedError += (int responseCode, string token) =>
                {
                    Console.WriteLine("Error consuming previous purchase");
                };

                mServiceConnection.BillingHandler.InAppBillingProcesingError += (message) =>
                {
                    Console.WriteLine("In app billing processing error {0}", message);
                };



                mServiceConnection.BillingHandler.BuyProductError += (int responseCode, string sku) =>
                {
                    Console.WriteLine("In app billing processing error ");
                    mLastError[sku] = "In app processing error";
                    mError = true;
                };

                mServiceConnection.BillingHandler.OnUserCanceled += () =>
                {
                    Console.WriteLine("OnUserCanceled");
                };


                mServiceConnection.BillingHandler.OnProductPurchased += (int response, Purchase purchase, string purchaseData, string purchaseSignature) =>
                {
                    if (purchase == null)
                    {
                        return;
                    }
                    mPurchases[purchase.ProductId] = purchase;
                    mLastError.Remove(purchase.ProductId);
                    mError = false;
                };

                loadProducts();
            };


            mServiceConnection.OnInAppBillingError += slot_billingError;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// ошибка билинга
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void slot_billingError(InAppBillingErrorType error, string message)
        {
            setError(message);
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// загрузка списка продуктов
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public async void loadProducts()
        {
            mError = false;
            IList<Product> list = await mServiceConnection.BillingHandler.QueryInventoryAsync(mProductID, ItemType.Product);
            if (list == null)
            {
                //ошибка
                setError("Error market");
                return;
            }
    
            //загрузили все нормлаьно
            foreach (var obj in list)
            {
                mProducts[obj.ProductId] = obj;
            }

            mState = EState.completed;
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// инцилизация списка продуктов
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void initProducts(List<string> products)
        {
            mProductID.AddRange(products);
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Доступность магазина
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isEnabled()
        {
            return true;
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// загрузка цены и описание товаров
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void updateStore()
        {
            if (mState != EState.none)
            {
                return;
            }

            mState = EState.loading;
            mServiceConnection.Connect();
        }
        ///--------------------------------------------------------------------------------------





      



        ///=====================================================================================
        ///
        /// <summary>
        /// проверка, были сделаны когданибудь покупки или нет
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isPurchase()
        {
            return mPurchase > 0 ? true : false;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// проверка, продукт куплен или нет
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isProductBuy(string productID)
        {
            if (mServiceConnection.BillingHandler == null)
            {
                return false;
            }

            if (mPurchases.ContainsKey(productID))
            {
                return true;
            }

            return false;
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// процесс завершения покупки
        /// и отослание магазину что все ок, продукт обратоался
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool productCompleted(string productID)
        {
            if (!mPurchases.ContainsKey(productID))
            {
                return false;
            }

            Purchase pur = mPurchases[productID];
            mPurchases.Remove(productID);

            if (pur == null)
            {
                //произошла ошибка при покупке
                return false; 
            }

            if (mServiceConnection.BillingHandler.ConsumePurchase(pur))
            {
                return true;
            }
            return false;
        }
        ///--------------------------------------------------------------------------------------










         ///=====================================================================================
        ///
        /// <summary>
        /// возвратим описание покупки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public string getDescription(string productID)
        {
            updateStore();
            if (mProducts.ContainsKey(productID))
            {
                return mProducts[productID].Title;

            }
            return null;
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// возвратим название покупки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public string getName(string productID)
        {
            updateStore();
            if (mProducts.ContainsKey(productID))
            {
                return mProducts[productID].Description;

            }
            if (mState == EState.completed)
            {
                return "Error";
            }

            return null;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// возвратим цену покупки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public string getFormattedPrice(string productID)
        {
            updateStore();
            if (mProducts.ContainsKey(productID))
            {
                string price = mProducts[productID].Price;
                price = price.Replace(" \u20bd", " руб.");
                return price;
            }

            if (mState == EState.completed)
            {
                return "...";
            }
            return null;
        }
        ///--------------------------------------------------------------------------------------




        


         ///=====================================================================================
        ///
        /// <summary>
        /// совершение опкупки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool productBuy(string productID)
        {
            if (mState != EState.completed)
            {
                return false;
            }

            if (!mProducts.ContainsKey(productID))
            {
                return false;
            }

            mPurchases.Remove(productID);
            mError = false;
            var product = mProducts[productID];
            mServiceConnection.BillingHandler.BuyProduct(product);
            return true;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// установка ошибки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void setError(string message)
        {
            mError = true;
            mLastError["all"] = message;
            mState = EState.none;
        }
        ///--------------------------------------------------------------------------------------




        ///=====================================================================================
        ///
        /// <summary>
        /// проверка, есть ошибка или нет
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isLastError(string productID)
        {
            return mError;
        }
        ///--------------------------------------------------------------------------------------




         ///=====================================================================================
        ///
        /// <summary>
        /// возвратим последнию ошибку
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public string getLastError(string productID)
        {
            if (mLastError.ContainsKey(productID))
            {
                return mLastError[productID];
            }
            return "Error: " + productID;
        }
        ///--------------------------------------------------------------------------------------










         ///=====================================================================================
        ///
        /// <summary>
        /// Загрузка настроек
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void loadSettings(AStorage settings)
        {
            mPurchase = settings.readInteger("purchase", mPurchase);
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Сохранение настроек
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void saveSettings(AStorage settings)
        {
            settings.writeInteger("purchase", mPurchase);
        }
        ///--------------------------------------------------------------------------------------


    }
}




