#region Using framework
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
#endregion





namespace Pluton.SystemProgram.Devices
{
    ///--------------------------------------------------------------------------------------
    ///--------------------------------------------------------------------------------------





     ///=====================================================================================
    ///
    /// <summary>
    /// Система работы с интернет сервером
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class ANetworkWeb
    {
        ///--------------------------------------------------------------------------------------
        private static readonly int     cTimeOut    = 10000; //время которое ждем для следующей попытки выполнениня запроса
        ///--------------------------------------------------------------------------------------





        ///--------------------------------------------------------------------------------------
        private bool        mLoginAth   = false;    //процесс авторизации на сервере
        private int         mDeviceID   = 0;        //индификатор девайса в базе
        private string      mDeviceGuid;            //уникальный номер девайса
        

        private WebClient       mWebClient  = null;     //управляющий поток сервера
        private List<AWebQuery> mPool = new List<AWebQuery>(); //пулл выполняемых команд

        private TimeSpan    mTimeWait   = TimeSpan.Zero;  //время которое ждем, после ошибки
        private bool        mWait       = false;              //флаг того что будем ждать 
        ///--------------------------------------------------------------------------------------





     


         ///=====================================================================================
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public ANetworkWeb(string deviceGuid)
        {
            mDeviceGuid = deviceGuid;
            mWebClient = new WebClient();
            mWebClient.UploadStringCompleted += evCommandCompleted;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// передача данных серверу
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void send(AWebQuery command)
        {
            command.sendQueue();
            mPool.Add(command);
            executeCommand();
        }
        ///--------------------------------------------------------------------------------------
  






         ///=====================================================================================
        ///
        /// <summary>
        /// Вход на сервер
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void deviceAuthorization()
        {
            if (mLoginAth)
            {
                return;
            }
            mLoginAth = true;
            AWebCommand cmd = new AWebCommand("ath");
            cmd.parameters.addString("deviceGuid", mDeviceGuid);
            string data = cmd.toString();

            mWebClient.Headers["Content-Type"] = "application/x-www-form-urlencoded";
            mWebClient.Encoding = Encoding.UTF8;
            mWebClient.UploadStringAsync(AWebCommand.cURI, "POST", data, this);
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// начало выполнения сообщения
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void executeCommand()
        {
            mWait = false; //ждать для отправки ненужно
            if (mDeviceID == 0)
            {
                //нет авторизации, авторизиуемся на сервере
                deviceAuthorization();
                return;
            }

            if (mPool.Count > 0 && !mWebClient.IsBusy)
            {
                AWebQuery cmd = mPool[0];
                mPool.Remove(cmd);
                cmd.send(mWebClient, mDeviceID);
            }
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// завершение загрузки на сервер
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void evCommandCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            AWebQuery cmd = e.UserState as AWebQuery;
            if (cmd == null && e.UserState is ANetworkWeb)
            {
                mLoginAth = false;
                if (e.Error != null)
                {
                    waitExecute();
                    return;
                }
                AWebParameters param = new AWebParameters(e.Result);
                mDeviceID = param.keyInteger("deviceID", mDeviceID);
                if (mDeviceID == 0)
                {
                    waitExecute();
                    return;
                }
                executeCommand();
                return;
            }

            if (e.Error != null)
            {
                if (cmd.sendQueue() < 10)
                {
                    mPool.Insert(0, cmd);
                }
                waitExecute();
                return;
            }
            cmd.execute(e.Result);
            executeCommand();
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// ждем, чтобы снова выполнить попытку отправки данных
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected void waitExecute()
        {
            mWait = true;
            mTimeWait = TimeSpan.Zero;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// ждать для повторной отпавки данных
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void update(TimeSpan gameTime)
        {
            if (mWait)
            {
                mTimeWait += gameTime;
                if (mTimeWait.TotalMilliseconds > cTimeOut)
                {
                    executeCommand();
                }
            }
        }
        ///--------------------------------------------------------------------------------------








    }
}
