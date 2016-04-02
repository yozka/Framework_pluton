#region Using framework
using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
#endregion




namespace Pluton.SystemProgram.Devices
{
    ///--------------------------------------------------------------------------------------
    ///--------------------------------------------------------------------------------------





    ///=====================================================================================
    ///
    /// <summary>
    /// Система доступа к настройкам
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AStorage
    {
        ///--------------------------------------------------------------------------------------
        //private IsolatedStorageSettings mSettings = IsolatedStorageSettings.ApplicationSettings;
        private IsolatedStorageFile mStorageFile = null; 
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
            mStorageFile = IsolatedStorageFile.GetUserStoreForAssembly();
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
            string sGuid = readString("_deviceGuid", string.Empty);
            if (sGuid == string.Empty)
            {
                sGuid = Guid.NewGuid().ToString();
                writeString("_deviceGuid", sGuid);
            }
            return sGuid;
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// Сброс настроек на диск
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void flush()
        {
            //mSettings.Save();
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
            IStream stream = readStream("main_options");
            if (stream != null)
            {
                return stream.readString(name, defaultValue);
            }
            return defaultValue;
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
            IStream stream = readStream("main_options");
            if (stream == null)
            {
                stream = new AStreamMemory();
            }
            stream.writeString(name, value);
            writeStream("main_options", stream);
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
            IStream stream = readStream("main_options");
            if (stream != null)
            {
                return stream.readBoolean(name, defaultValue);
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
            IStream stream = readStream("main_options");
            if (stream == null)
            {
                stream = new AStreamMemory();
            }
            stream.writeBoolean(name, value);
            writeStream("main_options", stream);
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
            IStream stream = readStream("main_options");
            if (stream != null)
            {
                return stream.readInteger(name, defaultValue);
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
            IStream stream = readStream("main_options");
            if (stream == null)
            {
                stream = new AStreamMemory();
            }
            stream.writeInteger(name, value);
            writeStream("main_options", stream);
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Сохранение потока в файл
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void writeStream(string name, IStream stream)
        {
            IsolatedStorageFileStream sm = null;
            try
            {
                if (mStorageFile.FileExists(name))
                {
                    mStorageFile.DeleteFile(name);
                }
                
                sm = mStorageFile.CreateFile(name);
                ABinaryWriter bin = new ABinaryWriter();
                bin.write(sm, stream);
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
            }
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// Ззагрузка потока из файла
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public IStream readStream(string name)
        {
            IsolatedStorageFileStream sm = null;
            IStream stream = null;
            try
            {
                if (mStorageFile.FileExists(name))
                {
                    sm = mStorageFile.OpenFile(name, FileMode.Open);
                    ABinaryReader<AStreamMemory> bin = new ABinaryReader<AStreamMemory>();
                    stream = bin.read(sm);
                    sm.Close();
                    sm.Dispose();
                }
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
        /// Сохранение потока в файл
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool writeStreamNativ(string name, Stream stream)
        {
            IsolatedStorageFileStream sm = null;
            try
            {
                if (mStorageFile.FileExists(name))
                {
                    mStorageFile.DeleteFile(name);
                }
                
                sm = mStorageFile.CreateFile(name);
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
        /// Ззагрузка потока из файла
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool readStreamNativ(string name, Stream stream)
        {
            IsolatedStorageFileStream sm = null;
            try
            {
                if (mStorageFile.FileExists(name))
                {
                    sm = mStorageFile.OpenFile(name, FileMode.Open);
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
