#region Using framework
using System;
using System.Net.Http;
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
    public class AAuthorization
            :
                AQuery
    {
        ///--------------------------------------------------------------------------------------
        private readonly string mURL;                   //адрес апи сервера
        private readonly string mDeviceGuid;            //уникальный номер девайса
        private readonly Uri    mURI;                   //нативный адрес сервера
        
        private int             mDeviceID = 0;          //индификатор девайса в базе
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// constructor
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AAuthorization(string url, string deviceGuid)
        {
            mURL = url;
            mURI = new Uri(url);
            mDeviceGuid = deviceGuid;
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
            if (mDeviceID == 0)
            {
                sendHttp();
                return;
            }
            executeCompleted();
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// выполнение команды
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected async void sendHttp()
        {
            try
            {

                AWebCommand cmd = new AWebCommand("ath");
                cmd.parameters.addString("deviceGuid", mDeviceGuid);
                cmd.parameters.addString("culture", ACulture.cultureSystem());
                cmd.parameters.addString("platform", AInputDevice.platform);
                string dataCmd = cmd.toString();



                var content = new StringContent(dataCmd, Encoding.UTF8, "application/x-www-form-urlencoded");
                using (var data = await network.http.PostAsync(mURI, content))
                {
                    string sdata = data.Content.ReadAsStringAsync().Result;
                    var result = new AWebParameters(sdata);
                    mDeviceID = result.keyInteger("deviceID", mDeviceID);
                    if (mDeviceID != 0)
                    {
                        executeCompleted();
                    }
                    else
                    {
                        executeError("Authorization fail");
                    }
                }
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
        /// очистить всю команду
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onClear()
        {
  
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
        /// возвратим адрес апи сервера
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public Uri uri
        {
            get
            {
                return mURI;
            }
        }
        ///--------------------------------------------------------------------------------------




    }
}
