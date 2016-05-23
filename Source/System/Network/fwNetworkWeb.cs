#region Using framework
using System;
using System.Collections.Generic;
using System.Net.Http;
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
        private static readonly int cTimeOut = 10000; //время которое ждем для следующей попытки выполнениня запроса
        ///--------------------------------------------------------------------------------------





        ///--------------------------------------------------------------------------------------
        private bool mLoginAth = false;    //процесс авторизации на сервере
        private int mDeviceID = 0;        //индификатор девайса в базе
        private readonly string mDeviceGuid;            //уникальный номер девайса


        private readonly HttpClient mHttpClient = null;     //управляющий поток сервера
        private readonly List<AWebQuery> mPool = new List<AWebQuery>(); //пулл выполняемых команд
        private bool mBussy = false; //флаг занятости

        private TimeSpan mTimeWait = TimeSpan.Zero;  //время которое ждем, после ошибки
        private bool mWait = false;              //флаг того что будем ждать 


        private TimeSpan mTimeoutWait = TimeSpan.Zero; //таймайт выполнения команды
        private bool mTimeout = false;
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
            mHttpClient = new HttpClient();
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
        private async void deviceAuthorization()
        {
            if (mLoginAth)
            {
                return;
            }
            mLoginAth = true;
            AWebCommand cmd = new AWebCommand("ath");
            cmd.parameters.addString("deviceGuid", mDeviceGuid);
            string dataCmd = cmd.toString();

            var content = new StringContent(dataCmd, Encoding.UTF8, "application/x-www-form-urlencoded");
            try
            {
                using (var data = await mHttpClient.PostAsync(AWebCommand.cURI, content))
                {
                    string result = data.Content.ReadAsStringAsync().Result;
                    AWebParameters param = new AWebParameters(result);
                    mDeviceID = param.keyInteger("deviceID", mDeviceID);
                    mLoginAth = false;
                    if (mDeviceID != 0)
                    {
                        nextExecute();
                    }
                    else
                    {
                        waitExecute();
                    }
                }
            }
            catch (Exception)
            {
                mLoginAth = false;
                waitExecute();
            }


            /*
        mWebClient.Headers["Content-Type"] = "application/x-www-form-urlencoded";
        mWebClient.Encoding = Encoding.UTF8;
        mWebClient.UploadStringAsync(AWebCommand.cURI, "POST", data, this);
        */
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


            if (mPool.Count == 0 || mBussy)
            {
                return;
            }

            mBussy = true;
            AWebQuery cmd = mPool[0];
            mPool.Remove(cmd);
            cmd.send(this);
            startTimeout();
        }
        ///--------------------------------------------------------------------------------------





        ///=====================================================================================
        ///
        /// <summary>
        /// завершение загрузки на сервер
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        /*
        private void evCommandCompleted(object sender, UploadStringCompletedEventArgs e)
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
        */




        ///=====================================================================================
        ///
        /// <summary>
        /// ждем, чтобы снова выполнить попытку отправки данных
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected void waitExecute()
        {
            mTimeout = false;
            mBussy = false;
            mWait = true;
            mTimeWait = TimeSpan.Zero;
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// следующее выполнение данных
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void nextExecute()
        {
            mTimeout = false;
            mBussy = false;
            mWait = true;
            mTimeWait = TimeSpan.FromMilliseconds(cTimeOut - 500);
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// режим ожидания выполнения команды
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected void startTimeout()
        {
            mTimeout = true;
            mTimeoutWait = TimeSpan.Zero;
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

            if (mTimeout)
            {
                mTimeoutWait += gameTime;
                if (mTimeoutWait.TotalMilliseconds > cTimeOut)
                {
                    nextExecute();
                }
            }
        }
        ///--------------------------------------------------------------------------------------





        ///=====================================================================================
        ///
        /// <summary>
        /// возвратим индификатор девайса
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public int deviceID
        {
            get
            {
                return mDeviceID;
            }
        }
        ///--------------------------------------------------------------------------------------





        ///=====================================================================================
        ///
        /// <summary>
        /// возвратим ьранспорт передачи данных
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public HttpClient http
        {
            get
            {
                return mHttpClient;
            }
        }



    }
}
