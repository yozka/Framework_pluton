﻿using System;
using System.Collections.Generic;


namespace Pluton.SystemProgram.Devices
{
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
            return false;
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
            return false;
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
            return false;
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










         ///=====================================================================================
        ///
        /// <summary>
        /// Загрузка настроек
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void loadSettings(AStorage settings)
        {

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
