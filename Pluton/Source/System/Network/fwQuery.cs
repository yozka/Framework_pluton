﻿#region Using framework
using System;
//using System.Net.Http;
#endregion





namespace Pluton.SystemProgram.Devices
{
    ///--------------------------------------------------------------------------------------
    ///--------------------------------------------------------------------------------------








     ///=====================================================================================
    ///
    /// <summary>
    /// Команды отправляемые на сервер
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AQuery
    {
        ///--------------------------------------------------------------------------------------
        private EStatus         mStatus     = EStatus.none;  //текущий статус команды
        private int             mCountSend  = 0;             //количество попыток отправки
        protected ANetworkWeb   network     = null;          //сеть
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// описание текущего статуса команды
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public enum EStatus
        {
            none,
            idle,
            execute,
            executeCompleted,
            error
        }
        ///--------------------------------------------------------------------------------------





        ///=====================================================================================
        ///
        /// <summary>
        /// constructor
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AQuery()
        {
        }
        ///--------------------------------------------------------------------------------------









        ///=====================================================================================
        ///
        /// <summary>
        /// проверяет статус, команда запущенна илил нет
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isExecute()
        {
            return mStatus == EStatus.idle || mStatus == EStatus.execute;
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// проверяет статус, команда выполнела свой запрос или нет
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isCompleted()
        {
            return mStatus == EStatus.executeCompleted;
        }
        ///--------------------------------------------------------------------------------------







        ///=====================================================================================
        ///
        /// <summary>
        /// команду отправили в стек выполенения 
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public int sendQueue()
        {
            mStatus = EStatus.idle;
            mCountSend++;
            return mCountSend;
        }
        ///--------------------------------------------------------------------------------------





        ///=====================================================================================
        ///
        /// <summary>
        /// начало выполнения команды
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void send(ANetworkWeb networkWeb)
        {
            network = networkWeb;
            mStatus = EStatus.execute;
            onSend();
        }
        ///--------------------------------------------------------------------------------------




        ///=====================================================================================
        ///
        /// <summary>
        /// начало выполнения команды
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected virtual void onSend()
        {
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// команда выполнилась с ошибкой
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected void executeError(string error)
        {
            mStatus = EStatus.error;
            signal_error?.Invoke(error, this);
            if (network != null)
            {
                network.waitExecute();
                network.addError(error);
            }
        }
        ///--------------------------------------------------------------------------------------






        ///--------------------------------------------------------------------------------------
        public delegate void eventError(string error, AQuery query);
        public event eventError signal_error;
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// команда выполнилась
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected void executeCompleted()
        {
            mStatus = EStatus.executeCompleted;
            mCountSend = 0;
            signal_completed?.Invoke(this);
            if (network != null)
            {
                network.nextExecute();
            }
        }
        ///--------------------------------------------------------------------------------------








        ///--------------------------------------------------------------------------------------
        public delegate void eventQuery(AQuery query);
        public event eventQuery signal_completed;
        ///--------------------------------------------------------------------------------------







        ///=====================================================================================
        ///
        /// <summary>
        /// конец выполнения сообщения
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public EStatus status
        {
            get
            {
                return mStatus;
            }
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// очистить всю команду
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void clear()
        {
            mStatus = EStatus.none;
            mCountSend = 0;
            onClear();
        }
        ///--------------------------------------------------------------------------------------





        ///=====================================================================================
        ///
        /// <summary>
        /// очистить всю команду
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected virtual void onClear()
        {
        }
        ///--------------------------------------------------------------------------------------








    }
}