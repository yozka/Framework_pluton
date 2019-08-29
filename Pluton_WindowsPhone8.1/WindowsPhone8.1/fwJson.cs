#region Using framework
using System;
#endregion


//using MicroJson;

using Newtonsoft;


namespace Pluton.SystemProgram.Devices
{
    ///--------------------------------------------------------------------------------------





    ///=====================================================================================
    ///
    /// <summary>
    /// Десериализация из хеша
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    class AJson
    {
        public static T DeserializeObject<T>(string value) 
        {
            var data =  Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
            return data;
        }

    }
    ///--------------------------------------------------------------------------------------





}
