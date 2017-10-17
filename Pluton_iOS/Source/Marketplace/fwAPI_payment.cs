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
    public class AAPI_payments
            :
                SKPaymentTransactionObserver
    {
        ///--------------------------------------------------------------------------------------
        private readonly Dictionary<string, string> mErrors = new Dictionary<string, string>(); //ошибки текущие

        private readonly Dictionary<string, SKPaymentTransaction> mTransaction = new Dictionary<string, SKPaymentTransaction>(); //выполненые покупки
        private readonly Dictionary<string, bool> mProcessBuy = new Dictionary<string, bool>();//список продуктов которые совершают покупки
        ///--------------------------------------------------------------------------------------










        ///=====================================================================================
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AAPI_payments()
        {
            SKPaymentQueue.DefaultQueue.AddTransactionObserver(this);
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
            if (mProcessBuy.ContainsKey(productID))
            {
                if (mProcessBuy[productID] == false)
                {
                    //продукт некорректно завершилось покупка.
                    return true;
                }



                //покупка данного продукта незавершена
                mErrors[productID] = "Error store";
                return false;
            }

            mErrors.Remove(productID);
            mTransaction.Remove(productID);

            mProcessBuy[productID] = true; //идет покупка товара в магазине

            var pay = SKPayment.PaymentWithProduct(productID);
            SKPaymentQueue.DefaultQueue.AddPayment(pay);
            return true;
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// проверка, есть ошибка или нет
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void UpdatedTransactions(SKPaymentQueue queue, SKPaymentTransaction[] transactions)
        {
            foreach (SKPaymentTransaction transaction in transactions)
            {
                switch (transaction.TransactionState)
                {
                    case SKPaymentTransactionState.Purchased:
                        completeTransaction(transaction);
                        break;
                    case SKPaymentTransactionState.Failed:
                        failedTransaction(transaction);
                        break;
                    default:
                        break;
                }
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// покупка совершена успешна
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void completeTransaction(SKPaymentTransaction transaction)
        {
            var productID = transaction.Payment.ProductIdentifier;
            mTransaction[productID] = transaction;
            mProcessBuy[productID] = false; //все. покупка завершена
            mErrors.Remove(productID);
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// ошибка покупки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void failedTransaction(SKPaymentTransaction transaction)
        {
            var productID = transaction.Payment.ProductIdentifier;
            if (productID == null || productID == string.Empty)
            {
                return;
            }
            SKPaymentQueue.DefaultQueue.FinishTransaction(transaction);
            mErrors[productID] = transaction.Error.LocalizedDescription;

            mTransaction.Remove(productID);
            mProcessBuy.Remove(productID);

            /*
            mTransaction[productID] = transaction;
            mProcessBuy[productID] = false; //все. покупка завершена
            */
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
            if (!mProcessBuy.ContainsKey(productID))
            {
                return false;
            }

            return mProcessBuy[productID] ? false : true;
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
            if (mTransaction.ContainsKey(productID))
            {
                var trans = mTransaction[productID];
                if (trans != null)
                {
                    SKPaymentQueue.DefaultQueue.FinishTransaction(trans);
                }
                mTransaction.Remove(productID);

            }

            mProcessBuy.Remove(productID);
            return mErrors.ContainsKey(productID) ? false : true;
        }
        ///--------------------------------------------------------------------------------------





        ///=====================================================================================
        ///
        /// <summary>
        /// подтверждение совершения покупки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void finishTransaction(SKPaymentTransaction transaction, bool wasSuccessful)
        {
            Console.WriteLine("FinishTransaction " + wasSuccessful);
            // remove the transaction from the payment queue.
            SKPaymentQueue.DefaultQueue.FinishTransaction(transaction);     // THIS IS IMPORTANT - LET'S APPLE KNOW WE'RE DONE !!!!


            /*
            using (var pool = new NSAutoreleasePool())
            {
                NSDictionary userInfo = NSDictionary.FromObjectsAndKeys(new NSObject[] { transaction }, new NSObject[] { new NSString("transaction") });
                if (wasSuccessful)
                {
                    // send out a notification that we’ve finished the transaction
                    NSNotificationCenter.DefaultCenter.PostNotificationName(InAppPurchaseManagerTransactionSucceededNotification, this, userInfo);
                }
                else
                {
                    // send out a notification for the failed transaction
                    NSNotificationCenter.DefaultCenter.PostNotificationName(InAppPurchaseManagerTransactionFailedNotification, this, userInfo);
                }
            }*/
        }











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













    }
}




