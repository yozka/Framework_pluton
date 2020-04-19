﻿#region Using framework
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Ionic.Zlib;
#endregion





namespace Pluton.SystemProgram.Devices.WEB
{
    ///--------------------------------------------------------------------------------------
    ///--------------------------------------------------------------------------------------





     ///=====================================================================================
    ///
    /// <summary>
    /// Параметры
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AWebParameters
    {
        ///--------------------------------------------------------------------------------------
        private Dictionary<string, string> mParams = new Dictionary<string, string>();
        ///--------------------------------------------------------------------------------------










         ///=====================================================================================
        ///
        /// <summary>
        /// constructor
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AWebParameters()
        {





        }
        ///--------------------------------------------------------------------------------------










         ///=====================================================================================
        ///
        /// <summary>
        /// constructor 2
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AWebParameters(string data)
        {
            string[] paramList = data.Split(':');
            foreach (var param in paramList)
            {
                string[] kv = param.Split('.');
                if (kv.Length == 2)
                {
                    try
                    {
                        string key = kv[0];
                        string base64 = kv[1];
                        byte[] buffer = Convert.FromBase64String(base64);
                        string value = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                        mParams[key] = value;
                    }
                    catch (Exception e)
                    {
                        gAnalytics.trackException(e);
                    }
                }
            }

        }
        ///--------------------------------------------------------------------------------------




         
         ///=====================================================================================
        ///
        /// <summary>
        /// возвратим количество элементов
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public int count
        {
            get
            {
                return mParams.Count;
            }
        }
        ///--------------------------------------------------------------------------------------
 





         ///=====================================================================================
        ///
        /// <summary>
        /// Переводим параметры в строчку
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public string toString()
        {
            bool prefix = false;
            StringBuilder sb = new StringBuilder();
            foreach (var key in mParams.Keys)
            {
                //преобразуем строчку
                string sData = mParams[key];
                if (sData == null || sData == string.Empty)
                {
                    continue;
                }

                byte[] buffer = Encoding.UTF8.GetBytes(sData);
                string base64 = Convert.ToBase64String(buffer); 
                //
               
                
                if (prefix)
                {
                    sb.Append("&");
                }
                else
                {
                    prefix = true;
                }
                sb.Append(key);
                sb.Append("=");
                sb.Append(base64);
            }


            return sb.ToString();
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// найдем значение ключа тип целочисленное число
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public int keyInteger(string key, int defaultValue)
        {
            if (mParams.ContainsKey(key))
            {
                int numValue;
                bool parsed = Int32.TryParse(mParams[key], out numValue);
                if (parsed)
                {
                    return numValue;
                }
            }
            return defaultValue;
        }
        ///--------------------------------------------------------------------------------------





 
         ///=====================================================================================
        ///
        /// <summary>
        /// найдем значение ключа тип строчка
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public string keyString(string key, string defaultValue)
        {
            if (mParams.ContainsKey(key))
            {
                return mParams[key];
            }
            return defaultValue;
        }
        ///--------------------------------------------------------------------------------------




         ///=====================================================================================
        ///
        /// <summary>
        /// добавить строковой параметр 
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void addString(string key, string value)
        {
            mParams[key] = value;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// добавить целочисленный параметр 
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void addInteger(string key, int value)
        {
            mParams[key] = Convert.ToString(value);
        }
        ///--------------------------------------------------------------------------------------




         ///=====================================================================================
        ///
        /// <summary>
        /// добавить целочисленный параметр 
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void addFloat(string key, float value)
        {
            mParams[key] = Convert.ToString(value);
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// добавить поток
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void addStream(string key, IStream stream)
        {
            Stream sm = new MemoryStream();
            ABinaryWriter bin = new ABinaryWriter();
            bin.write(sm, stream);
            sm.Position = 0;
            int size = (int)sm.Length;
            byte[] buffer = new byte[size];
            sm.Read(buffer, 0, size);
            sm.Flush();
            sm.Dispose();

            byte[] zip = GZipStream.CompressBuffer(buffer);
            mParams[key] = Convert.ToBase64String(zip);
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// очистка всех параметров
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void clear()
        {
            mParams.Clear();
        }
        ///--------------------------------------------------------------------------------------





    }
}