using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.UI.Core;


namespace Pluton.SystemProgram.Devices
{
    ///--------------------------------------------------------------------------------------



 
    
     ///=====================================================================================
    ///
    /// <summary>
    /// вызов функции в потоке интерфейса
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public static class UIThread
    {
        private static CoreDispatcher mDispatcher = null;
        
        static UIThread()
        {
        }

        public static void init(CoreDispatcher dispatcher)
        {
            mDispatcher = dispatcher;
        }

        public static async void invoke(DispatchedHandler action)
        {
            //action.Invoke();
            if (mDispatcher != null)
            {
                await mDispatcher.RunAsync(CoreDispatcherPriority.Normal, action);
            }
        }
    }
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
        //private IAsyncOperation<ListingInformation> mMarket = null;
        private ListingInformation mInfo = null;
        private bool mUpdateInfo = false;

        private IReadOnlyDictionary<string, ProductListing> mProducts = null;
        private Dictionary<string, Guid> mTransactions = null; //незавершенные покупки

        private readonly Dictionary<string, string> mErrors = new Dictionary<string, string>(); //ошибки текущие
        private int mPurchase = 0; //количество покупок
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
        /// инцилизация списка продуктов
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void initProducts(List<string> products)
        {

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
        /// проверка, были сделаны покупки или нет
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
            try
            {
                if (CurrentApp.LicenseInformation.ProductLicenses[productID].IsActive)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                gAnalytics.trackException(e);
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
            try
            {
                /*
                if (mTransactions != null && mTransactions.ContainsKey(productID))
                {
                     
                    CurrentApp.ReportConsumableFulfillmentAsync(productID, );
                    

                }
                */
                /*if (CurrentApp.LicenseInformation.ProductLicenses[productID].IsActive)
                {
                    return true;
                }*/
                
            }
            catch (Exception e)
            {
                gAnalytics.trackException(e);
            }
            return false;
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// обновление информации о покупках
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private bool refreshInformation()
        {
            if (mProducts != null)
            {
                return true;
            }

            if (mUpdateInfo)
            {
                return false;
            }

            asyncRefreshInfo();
            return mProducts != null ? true : false;

        }
        ///--------------------------------------------------------------------------------------




        ///=====================================================================================
        ///
        /// <summary>
        /// обновление информации о покупках
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private async void asyncRefreshInfo()
        {
            mUpdateInfo = true;
            mInfo = await CurrentApp.LoadListingInformationAsync();
            if (mInfo != null)
            {
                mProducts = mInfo.ProductListings;
                mUpdateInfo = false;
            }



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
            if (!refreshInformation())
            {
                return null;
            }

            if (mProducts.ContainsKey(productID))
            {
                return mProducts[productID].Description;
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
            if (!refreshInformation())
            {
                return null;
            }

            if (mProducts.ContainsKey(productID))
            {
                return mProducts[productID].Name;
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
            if (!refreshInformation())
            {
                return null;
            }

            if (mProducts.ContainsKey(productID))
            {
                return mProducts[productID].FormattedPrice;
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
            /*
            if (mCurrentPoductID != null)
            {
                return false;
            }
            mErrors[productID] = null;
            mCurrentPoductID = productID;            
            UIThread.invoke(inapp);
            
             */
            return true;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// выполнения запроса на покупку
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private static async void inapp()
        {
            /*
            string error = null;
            string id = mCurrentPoductID;
            mCurrentPoductID = null;

            

            try
            {
                PurchaseResults result = await CurrentApp.RequestProductPurchaseAsync(id);
                if (result.Status == ProductPurchaseStatus.Succeeded)
                {
                    mPurchase++;
                    //аналитика
                    var prop = new Dictionary<string, string> 
                            { 
                                { "id",     id }, 
                                { "count",  mPurchase.ToString() },
                            };
                    gAnalytics.trackEvent("purchase", prop);
                }
                else
                {
                    var prop = new Dictionary<string, string> 
                            { 
                                { "id",     id }, 
                                { "count",  mPurchase.ToString() },
                            };
                    gAnalytics.trackEvent("purchase_error", prop);
                }
                

                
                
 
            }
            catch (Exception ex)
            {
                gAnalytics.trackException(ex);

                error = ex.Message;

                if (error.IndexOf("HRESULT") >= 0)
                {
                    error = "Marketplace error";
                }
            }

            if (error != null)
            {
                mErrors[id] = error;
            }
            */
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
            if (!mErrors.ContainsKey(productID))
            {
                return false;
            }

            return mErrors[productID] == null ? false : true;
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
            if (!mErrors.ContainsKey(productID))
            {
                return null;
            }

            return mErrors[productID];
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









///=====================================================================================
///
/// <summary>
/// Начало запуска программы
/// </summary>
/// 
///--------------------------------------------------------------------------------------
/*
public void test()
{
    UIThread.invoke(test2);
            
            
       
      var listing = await CurrentApp.LoadListingInformationAsync();
      foreach (var product in listing.ProductListings)
      {
          string ss = string.Format("{0}, {1}, {2},{3}, {4}",
                             product.Key,
                             product.Value.Name,
                             product.Value.FormattedPrice,
                             product.Value.ProductType,
                             product.Value.Description);

          int i = 0;
      }


  
    int i = 0;
    //UIThread.Invoke(() => start());

   
    var dispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;
    dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, inapp);
    
    //var dispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread();
   // dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, inapp);
    
    var mainpage = HexoTree_WP8.MainPage.instance();
    mainpage.LaunchStoreForProductPurchase("coins_pack_2", false, exitStore);
         
}

*/

/*
private void exitStore(string arg1, string arg2, bool arg3)
{
    int i = 0;

    if (CurrentApp.LicenseInformation.ProductLicenses[arg1].IsActive)
    {
        CurrentApp.ReportProductFulfillment(arg1);
    }
}
///--------------------------------------------------------------------------------------


    

private void inapp()
{
    string id = "coins_pack_1";


    try
    {
        var purchaseAsync = CurrentApp.RequestProductPurchaseAsync(id, false);
        purchaseAsync.Completed = (async, status) =>
            {
                if (CurrentApp.LicenseInformation.ProductLicenses[id].IsActive)
                {
                    CurrentApp.ReportProductFulfillment(id);
                }

            };
                    
                
              

    }
    catch (Exception ex)
    {
        string ss = ex.ToString();
    }         
            

}
*/

/*

private async void test2()
{
    string id = "coins_pack_1";
            
    bool productPurchaseError = false;
    string receipt = "";

    try
    {
        receipt = await CurrentApp.RequestProductPurchaseAsync(id, false);
    }
    catch (Exception ex)
    {
        productPurchaseError = true;
    }



    if (CurrentApp.LicenseInformation.ProductLicenses[id].IsActive)
    {
        CurrentApp.ReportProductFulfillment(id);
    }
            
}
 * */
