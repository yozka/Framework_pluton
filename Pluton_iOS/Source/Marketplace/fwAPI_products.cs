using System;
using System.Collections.Generic;


namespace Pluton.SystemProgram.Devices
{
    ///--------------------------------------------------------------------------------------

    using StoreKit;
    using Foundation;

    ///--------------------------------------------------------------------------------------


        




    ///=====================================================================================
    ///
    /// <summary>
    /// Система обслуживание магазина
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AAPI_products
            :
                SKProductsRequestDelegate
    {
        ///--------------------------------------------------------------------------------------
        private readonly Dictionary<string, string> mDescriptions   = new Dictionary<string, string>();
        private readonly Dictionary<string, string> mTitles         = new Dictionary<string, string>();
        private readonly Dictionary<string, string> mPrices         = new Dictionary<string, string>(); 
        private readonly List<string>               mProductID      = new List<string>();

        private SKProductsRequest mProducts = null; //указатеь на внутренний магазин платформы
        ///--------------------------------------------------------------------------------------










         ///=====================================================================================
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AAPI_products()
        {
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// загрузка цены и описание товаров
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void loadStore(List<string> products)
        {
            mDescriptions.Clear();
            mTitles.Clear();
            mPrices.Clear();
            mProductID.Clear();
            mProductID.AddRange(products);

           
            var array = new NSString[mProductID.Count];
            for (var i = 0; i < mProductID.Count; i++)
            {
                array[i] = new NSString(mProductID[i]);
            }
            NSSet productIdentifiers = NSSet.MakeNSObjectSet<NSString>(array);

            //set up product request for in-app purchase
            mProducts = new SKProductsRequest(productIdentifiers);
            mProducts.Delegate = this;
            mProducts.Start();
            
        }
        ///--------------------------------------------------------------------------------------




         
         ///=====================================================================================
        ///
        /// <summary>
        /// проверка, были сделаны покупки или нет
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void ReceivedResponse(SKProductsRequest request, SKProductsResponse response)
        {
            bool ok = false;
            foreach ( var prd in response.Products)
            {
                string sProductID   = prd.ProductIdentifier;
                string sDescription = prd.LocalizedDescription;
                string sTitle       = prd.LocalizedTitle;
                string sPrice       = localizedPrice(prd);

                if (sProductID != null && sDescription != null && sTitle != null && sPrice != null)
                {
                    mDescriptions[sProductID]   = sDescription;
                    mTitles[sProductID]         = sTitle;
                    mPrices[sProductID]         = sPrice;
                }
                ok = true;
            }

            if (ok)
            {
                if (signal_completed != null)
                {
                    signal_completed();
                }
            }
            else
            {
                if (signal_failed != null)
                {
                    signal_failed();
                }
            }


        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Перевод цены с учетом локали
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private static string localizedPrice(SKProduct product)
        {
            var formatter = new NSNumberFormatter();
            formatter.FormatterBehavior = NSNumberFormatterBehavior.Version_10_4;
            formatter.NumberStyle = NSNumberFormatterStyle.Currency;
            formatter.Locale = product.PriceLocale;
            var formattedString = formatter.StringFromNumber(product.Price);

            formattedString = formattedString.Replace(" \u20bd", " руб.");

            return formattedString;
        }
        ///--------------------------------------------------------------------------------------





        ///=====================================================================================
        ///
        /// <summary>
        /// Сигнал завершение загрузка цены
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public delegate void eventCompleted();
        public event eventCompleted signal_completed;
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Сигнал ошибка во время загрузки цены
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public delegate void eventFailed();
        public event eventFailed signal_failed;
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
            return mDescriptions.ContainsKey(productID) ? mDescriptions[productID] : null;
        }
        ///--------------------------------------------------------------------------------------







        ///=====================================================================================
        ///
        /// <summary>
        /// возвратим название покупки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public string getTitle(string productID)
        {
            return mTitles.ContainsKey(productID) ? mTitles[productID] : null;
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// возвратим цену покупки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public string getPrice(string productID)
        {
            return mPrices.ContainsKey(productID) ? mPrices[productID] : null;
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
            return false;
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
            return null;
        }
        ///--------------------------------------------------------------------------------------













    }
}




