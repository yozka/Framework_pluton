﻿using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;



namespace Pluton.SystemProgram.Devices
{
    ///--------------------------------------------------------------------------------------
    using Foundation;
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
            Flurry.Analytics.FlurryAgent.StartSession(mSessionID);
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
                var param = new Foundation.NSMutableDictionary();
                foreach (var key in properties.Keys)
                {
                    var nsKey = new NSString(key);
                    var nsValue = new NSString(properties[key]);

                    param[nsKey] = nsValue;
                }
                Flurry.Analytics.FlurryAgent.LogEvent(eventName, param);
            }
            else
            {
                Flurry.Analytics.FlurryAgent.LogEvent(eventName);
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
            var error = new Foundation.NSException("flurryError", ex.Message, new NSDictionary());
            Flurry.Analytics.FlurryAgent.LogError("flurryError", ex.Message, error);
        }
        ///--------------------------------------------------------------------------------------









    }
}
