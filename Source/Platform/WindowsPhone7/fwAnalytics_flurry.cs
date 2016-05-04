using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Phone.Tasks;


namespace Pluton.SystemProgram.Devices
{
    ///--------------------------------------------------------------------------------------





     ///=====================================================================================
    ///
    /// <summary>
    /// Система аналитики приложения
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AAnalytics_flurry
        :
            IAnalytics
    {
        ///--------------------------------------------------------------------------------------
        private readonly string mSessionID = null;
        ///--------------------------------------------------------------------------------------
        









         ///=====================================================================================
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AAnalytics_flurry(string sessionID)
        {
            mSessionID = sessionID;
        }
        ///--------------------------------------------------------------------------------------



      


         ///=====================================================================================
        ///
        /// <summary>
        /// Начало запуска программы
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void startSession()
        {
            FlurryWP8SDK.Api.StartSession(mSessionID);
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// выключение сесси
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void endSession()
        {
            FlurryWP8SDK.Api.EndSession();
        }
        ///--------------------------------------------------------------------------------------






        
         ///=====================================================================================
        ///
        /// <summary>
        /// событие c параметрами
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void trackEvent(string eventName, IDictionary<string, string> properties)
        {
            if (properties != null)
            {
                var param = new List<FlurryWP8SDK.Models.Parameter>();
                foreach (var key in properties.Keys)
                {
                    param.Add(new FlurryWP8SDK.Models.Parameter(key, properties[key]));
                }
                FlurryWP8SDK.Api.LogEvent(eventName, param);
            }
            else
            {
                FlurryWP8SDK.Api.LogEvent(eventName);
            }
        }
        ///--------------------------------------------------------------------------------------




        
         ///=====================================================================================
        ///
        /// <summary>
        /// ошибка
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void trackException(Exception ex)
        {
            FlurryWP8SDK.Api.LogError(ex.Message, ex);
        }
        ///--------------------------------------------------------------------------------------










         ///=====================================================================================
        ///
        /// <summary>
        /// обработка ошибок
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void logError(string message, Exception e)
        {
#if ANALYTICS  
            string sError = message + " -> M:" + e.Message + "; S:" + e.StackTrace;
            FlurryWP8SDK.Api.LogError(sError, e);
            MessageBox.Show("Dear user, There was error in the programm. Fix it in the next version. Thanks!", "Error", MessageBoxButton.OK);

            try
            {
                EmailComposeTask emailComposeTask = new EmailComposeTask();

                emailComposeTask.Subject = "Error";
                emailComposeTask.Body = sError;
                emailComposeTask.To = "error@robotrek.info";
                emailComposeTask.Show();
            }
            catch (Exception)
            {
            }
            //mDeviceGame.Exit();
            
#endif
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// начало выполнения сообщения
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void beginEvent(string eventName)
        {
#if ANALYTICS  
            FlurryWP8SDK.Api.LogEvent(eventName, true);
#endif
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// конец выполнения сообщения
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void endEvent(string eventName)
        {
#if ANALYTICS
            FlurryWP8SDK.Api.EndTimedEvent(eventName);
#endif
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// событие с параметром
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void eventParam(string eventName, string paramName)
        {
#if ANALYTICS
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter(paramName, String.Empty));
            FlurryWP8SDK.Api.LogEvent(eventName, param);
#endif
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// событие простое, без параметров
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void eventLog(string eventName)
        {
#if ANALYTICS
            FlurryWP8SDK.Api.LogEvent(eventName);
#endif
        }
        ///--------------------------------------------------------------------------------------



    }
}
