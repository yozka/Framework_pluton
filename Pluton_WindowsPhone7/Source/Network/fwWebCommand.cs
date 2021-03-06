﻿#region Using framework
using System;
using System.Net;
using System.Text;
#endregion





namespace Pluton.SystemProgram.Devices.WEB
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
    public class AWebCommand
            :
                AQuery
    {
        ///--------------------------------------------------------------------------------------
        private const int cVersionAPI = 1; //версия протокола
        ///--------------------------------------------------------------------------------------
 


        ///--------------------------------------------------------------------------------------
        public readonly AWebParameters parameters = new AWebParameters();
        public AWebParameters result = null; //результат выполнения команды
        ///--------------------------------------------------------------------------------------




        ///--------------------------------------------------------------------------------------
        private string mData = null;
        private object mUserData = null; //пользовательские данные
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// constructor
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AWebCommand()
        {
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// constructor
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AWebCommand(string cmd)
        {
            parameters.addString("cmd", cmd);
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// пользовательские данные
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public object userData
        {
            set
            {
                mUserData = value;
            }

            get
            {
                return mUserData;
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// преобразование команды в текст для отправки на сервер
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public string toString()
        {
            return parameters.toString();
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// начало выполнения команды
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onSend()
        {
            var login = network.authorization as AAuthorization;
            if (login == null)
            {
                executeError("Not authorization");
                return;
            }

            int deviceID = login.deviceID;
            if (deviceID == 0)
            {
                executeError("handle DeviceID invalid");
                return;
            }

            parameters.addInteger("deviceID", deviceID);
            parameters.addInteger("ver", cVersionAPI);
            sendHttp(login.uri);
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// выполнение команды
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected void sendHttp(Uri uri)
        {
            try
            {

                network.http.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                network.http.Encoding = Encoding.UTF8;
                network.http.UploadStringAsync(uri, "POST", parameters.toString(), this);
            }
            catch (Exception e)
            {
                executeError(e.Message);
            }
        }
        ///--------------------------------------------------------------------------------------




         ///=====================================================================================
        ///
        /// <summary>
        /// команда выполнилась на сервере
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onCompleted(Object data)
        {
            mData = data as string;
            result = new AWebParameters(mData);
            executeCompleted();
        }
        ///--------------------------------------------------------------------------------------








        ///=====================================================================================
        ///
        /// <summary>
        /// очистить всю команду
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onClear()
        {
            parameters.clear();
            result = null;
            mData = null;
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// возвратить текст сообщения
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public string resultText
        {
            get
            {
                return mData;
            }
        }
        ///--------------------------------------------------------------------------------------









    }
}
