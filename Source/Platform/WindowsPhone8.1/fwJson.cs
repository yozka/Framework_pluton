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
         /*   JsonParser parser = new JsonParser();
            var data = parser.Parse(value);
            */
            
            //return data;
            //return default(T) ;
            //int k = 0;
            //var s = value.ToString();

            var data =  Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);


            return data;
            //return  JsonConvert.DeserializeObject<T>(value);
        }

    }
    ///--------------------------------------------------------------------------------------





}
