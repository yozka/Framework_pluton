#if PLUTON_JSON
#region Using framework
using System;
using Newtonsoft.Json;
#endregion





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
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
    ///--------------------------------------------------------------------------------------





}
#endif