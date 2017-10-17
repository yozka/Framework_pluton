#region Using framework
using System;
using System.IO;
using Windows.Storage;

using Windows.System.Profile;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;

using Windows.Security.ExchangeActiveSyncProvisioning;
#endregion






namespace Pluton.SystemProgram.Devices
{
    ///--------------------------------------------------------------------------------------
    using Ionic.Zlib;
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
        private readonly ApplicationDataContainer mSettings = null;
        private readonly StorageFolder mStorageFolder = null;
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
            mSettings = ApplicationData.Current.LocalSettings;
            mStorageFolder = ApplicationData.Current.LocalFolder;
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
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.System.Profile.HardwareIdentification"))
            {
                var token = HardwareIdentification.GetPackageSpecificToken(null);
                var hardwareId = token.Id;
                var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(hardwareId);

                byte[] bytes = new byte[hardwareId.Length];
                dataReader.ReadBytes(bytes);
                return BitConverter.ToString(bytes).Replace("-", "");
            }

            string guidDef = readString("deviceGuid", null);
            if (guidDef == null)
            {
                var deviceInformation = new EasClientDeviceInformation();
                guidDef = deviceInformation.Id.ToString();
                writeString("deviceGuid", guidDef);
            }
            return guidDef;
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
            try
            {
                if (mSettings.Values.ContainsKey(name))
                {
                    var value = mSettings.Values[name] as string;
                    return value != null ? value : defaultValue;
                }
            }
            catch (Exception)
            {
                return defaultValue;
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
            try
            {
                mSettings.Values[name] = value;
            }
            catch (Exception)
            {

            }
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
            try
            {
                if (mSettings.Values.ContainsKey(name))
                {
                    return (bool)mSettings.Values[name];
                }
            }
            catch (Exception)
            {
                return defaultValue;
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
            try
            {
                mSettings.Values[name] = value;
            }
            catch (Exception)
            {

            }
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
            try
            {
                if (mSettings.Values.ContainsKey(name))
                {
                    return (int)mSettings.Values[name];
                }
            }
            catch (Exception)
            {

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
            try
            {
                mSettings.Values[name] = value;
            }
            catch (Exception)
            {

            }
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
            try
            {
                Stream sm = new MemoryStream();
                ABinaryWriter bin = new ABinaryWriter();
                bin.write(sm, stream);
                sm.Position = 0;
                int size = (int)sm.Length;
                byte[] buf = new byte[size];
                sm.Read(buf, 0, size);
                sm.Dispose();
                var zip = GZipStream.CompressBuffer(buf);
                mSettings.Values["stream" + name] = zip;
            }
            catch (Exception)
            {
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
            string sName = "stream" + name;
            if (!mSettings.Values.ContainsKey(sName))
            {
                return null;
            }

            IStream stream = null;
            try
            {
                byte[] zip = mSettings.Values[sName] as byte[];
                if (zip == null)
                {
                    return null;
                }
                byte[] buf = GZipStream.UncompressBuffer(zip);
                Stream sm = new MemoryStream(buf);
                ABinaryReader<AStreamMemory> bin = new ABinaryReader<AStreamMemory>();
                stream = bin.read(sm);
            }
            catch (Exception)
            {
                stream = null;
            }
            return stream;
            /*
            IStream stream = null;
            try
            {
                var file = mStorageFolder.GetFileAsync(name).GetResults();
                if (file != null)
                {
                    var sm = file.OpenStreamForReadAsync().Result;
                    ABinaryReader<AStreamMemory> bin = new ABinaryReader<AStreamMemory>();
                    stream = bin.read(sm);
                }
            }
            catch (Exception e)
            {
                stream = null;
            }
            return stream;
             */
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
            try
            {
                var fileSync = mStorageFolder.CreateFileAsync(name, CreationCollisionOption.ReplaceExisting);
                while (fileSync.Status == AsyncStatus.Started)
                {
                    System.Threading.Tasks.Task.Delay(20).Wait();
                }
                var file = fileSync.GetResults();
                var writer = file.OpenStreamForWriteAsync();
                writer.Wait(TimeSpan.FromSeconds(10));
                var sm = writer.Result;
                sm.Position = 0;
                stream.Position = 0;
                stream.CopyTo(sm);
                sm.Flush();
                return true;
            }
            catch (Exception e)
            {
                string s = e.Message;
            }
            return false;
  


            /*
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
             * */
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
            try
            {
                var fileSync = mStorageFolder.GetFileAsync(name);
                while (fileSync.Status == AsyncStatus.Started)
                {
                    System.Threading.Tasks.Task.Delay(20).Wait();
                }
                var file = fileSync.GetResults();
                
                //                
                if (file != null)
                {
                    var reader = file.OpenStreamForReadAsync();
                    reader.Wait(TimeSpan.FromSeconds(10));
                    var sm = reader.Result;

                    stream.Position = 0;
                    sm.CopyTo(stream);
                    sm.Dispose();
                    return true;
                }
            }
            catch (Exception)
            {

            }
            return false;


        }
        ///--------------------------------------------------------------------------------------



    }
}
