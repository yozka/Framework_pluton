#region Using framework
using System;
using System.IO;
using System.IO.IsolatedStorage;
#endregion






namespace Pluton.SystemProgram.Devices
{
    ///--------------------------------------------------------------------------------------
    using Ionic.Zlib;
    ///--------------------------------------------------------------------------------------





     ///=====================================================================================
    ///
    /// <summary>
    /// —истема доступа к настройкам
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AStorage
    {
        ///--------------------------------------------------------------------------------------
 




         ///=====================================================================================
        ///
        /// <summary>
        /// Constrictor
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AStorage()
        {
            

        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// возвратить уникальный индификатор клиента
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public string getDeviceGuid()
        {
            byte[] d = new byte[16];
            Array.Copy((byte[])Microsoft.Phone.Info.DeviceExtendedProperties.GetValue("DeviceUniqueId"), d, d.Length);
            return new Guid(d).ToString();
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// —брос настроек на диск
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void flush()
        {
            try
            {
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
            catch (Exception)
            {

            }
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// чтение строковых настроек
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public string readString(string name, string defaultValue)
        {
            string value = defaultValue;
            if (IsolatedStorageSettings.ApplicationSettings.Contains(name))
            {
                value = IsolatedStorageSettings.ApplicationSettings[name] as string; 
            }
            return value != null ? value : defaultValue;
        }
        ///--------------------------------------------------------------------------------------





 
         ///=====================================================================================
        ///
        /// <summary>
        /// запись строковых настроек
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void writeString(string name, string value)
        {
            IsolatedStorageSettings.ApplicationSettings[name] = value; 
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// чтение булевых настроек
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool readBoolean(string name, bool defaultValue)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(name))
            {
                bool value = (bool)IsolatedStorageSettings.ApplicationSettings[name];
                return value;
            }
            return defaultValue;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// запись булеввых настроек
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void writeBoolean(string name, bool value)
        {
            IsolatedStorageSettings.ApplicationSettings[name] = value;
        }
        ///--------------------------------------------------------------------------------------




         ///=====================================================================================
        ///
        /// <summary>
        /// чтение целочисленных настроек
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public int readInteger(string name, int defaultValue)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(name))
            {
                int value = (int)IsolatedStorageSettings.ApplicationSettings[name];
                return value;
            }
            return defaultValue;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// запись целочисленных настроек
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void writeInteger(string name, int value)
        {
            IsolatedStorageSettings.ApplicationSettings[name] = value;
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// —охранение потока в файл
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void writeStream(string name, IStream stream)
        {
            Stream sm = null;
            try
            {
                sm = new MemoryStream();          
                ABinaryWriter bin = new ABinaryWriter();
                bin.write(sm, stream);

                sm.Position = 0;
                int size = (int)sm.Length;
                byte[] buffer = new byte[size];
                sm.Read(buffer, 0, size);
                sm.Close();
                sm.Dispose();

                byte[] zip = GZipStream.CompressBuffer(buffer);

                IsolatedStorageSettings.ApplicationSettings[name] = zip;
            }
            catch (Exception)
            {
                if (sm != null)
                {
                    sm.Close();
                    sm.Dispose();
                }
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// «загрузка потока из файла
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public IStream readStream(string name)
        {
            if (!IsolatedStorageSettings.ApplicationSettings.Contains(name))
            {
                return null;
            }
      

            Stream sm = null;
            IStream stream = null;
            try
            {

                byte[] zip = IsolatedStorageSettings.ApplicationSettings[name] as byte[];
                byte[] buffer = GZipStream.UncompressBuffer(zip);
                sm = new MemoryStream(buffer);

                ABinaryReader<AStreamMemory> bin = new ABinaryReader<AStreamMemory>();
                stream = bin.read(sm);
                sm.Close();
                sm.Dispose();
            }
            catch (Exception)
            {
                stream = null;
                if (sm != null)
                {
                    sm.Close();
                    sm.Dispose();
                }
            }
            return stream;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// —охранение потока в файл
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool writeStreamNativ(string name, Stream stream)
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream sm = null;
            try
            {
                if (storage.FileExists(name))
                {
                    storage.DeleteFile(name);
                }

                sm = storage.CreateFile(name);
                stream.Position = 0;
                stream.CopyTo(sm);
                sm.Close();
                sm.Dispose();
            }
            catch (Exception)
            {
                if (sm != null)
                {
                    sm.Close();
                    sm.Dispose();
                }
                return false;
            }
            return true;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// «загрузка потока из файла
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool readStreamNativ(string name, Stream stream)
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream sm = null;
            try
            {
                if (storage.FileExists(name))
                {
                    sm = storage.OpenFile(name, FileMode.Open);
                    stream.Position = 0;
                    sm.CopyTo(stream);
                    sm.Close();
                    sm.Dispose();
                    return true;
                }
            }
            catch (Exception)
            {
                if (sm != null)
                {
                    sm.Close();
                    sm.Dispose();
                }
            }
            return false;
        }
        ///--------------------------------------------------------------------------------------



    }
}
