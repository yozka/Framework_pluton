#region Using framework
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
#endregion




namespace Pluton.SystemProgram.Devices
{
    ///--------------------------------------------------------------------------------------
    using Pluton.SystemProgram.License;
    ///--------------------------------------------------------------------------------------






     ///=====================================================================================
    ///
    /// <summary>
    /// ��������� �������� ��� ����������
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class ADevices
    {


        /// <summary>
        /// ������� ��������
        /// </summary>
        public readonly AVibrationDevice vibration;


        /// <summary>
        /// ������� ������� ������
        /// </summary>
        public readonly AMusicDevice music;


        /// <summary>
        /// ������� �������� ��������
        /// </summary>
        public readonly ASoundDevice sound;



        /// <summary>
        /// ������� ����� ����������
        /// </summary>
        public readonly AInputDevice input;



        /// <summary>
        /// ������� ���������
        /// </summary>
        public readonly AAnalytics analytics;




        /// <summary>
        /// �������� ���������
        /// </summary>
        public readonly ACulture culture;





        /// <summary>
        /// ������ � ����������
        /// </summary>
        public readonly AStorage storage;




        /// <summary>
        /// ������ � ���� �� �������
        /// </summary>
        public readonly ANetworkWeb network;






        /// <summary>
        /// ������ � �������� �������
        /// </summary>
        public readonly ADateTime dateTime;


        /// <summary>
        /// ������� �������������� ����
        /// </summary>
        public readonly ALicense license;



        /// <summary>
        /// �������
        /// </summary>
        public readonly AMarketplace marketplace;



        /*
         * ���������� �� ����������, ��� � ������
         */
        private readonly AScreenManager m_screenManager;
        ///--------------------------------------------------------------------------------------





        ///--------------------------------------------------------------------------------------
#if (STATICS)
        /// FPS
        private int m_frameRate = 0;
        private int m_frameCounter = 0;
        private TimeSpan m_elapsedTime = TimeSpan.Zero;

        /// ������
        private long m_memoryTotoal;
        private long m_memoryUsage;
        private long m_memoryPeak;
        private int m_memUpdate = 0;


        // ����� ���������� ������
        static public int mTimeLogic = 0;
        static public float mTimeUpdate = 0;

        private int m_timeLogic = 0;
        private float m_timeUpdate = 0;
        private string m_info = "";
#endif
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public ADevices(AScreenManager screenManager)
        {
            m_screenManager = screenManager;

            storage         = new AStorage();
            culture         = new ACulture();
            
            vibration       = new AVibrationDevice();
            music           = new AMusicDevice();
            sound           = new ASoundDevice();
            input           = new AInputDevice(screenManager.Game);
            analytics       = new AAnalytics();
            network         = new ANetworkWeb(getDeviceGuid());
 
            dateTime        = new ADateTime();
            license         = new ALicense(this);
            marketplace     = new AMarketplace();
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// ���������� ��������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void update(TimeSpan gameTime)
        {
            input.update();
            vibration.update(gameTime);
            network.update(gameTime);
            sound.update(gameTime);
            dateTime.add(gameTime);

#if (STATICS)
            //FPS
            m_elapsedTime += gameTime;
            if (m_elapsedTime.TotalSeconds > 1)
            {
                m_elapsedTime -= TimeSpan.FromSeconds(1);
                m_frameRate = m_frameCounter;
                m_frameCounter = 0;

                //������
                if (++m_memUpdate > 1)
                {
                    /*
                    m_memoryTotoal = (long)DeviceExtendedProperties.GetValue("DeviceTotalMemory");
                    m_memoryUsage = (long)DeviceExtendedProperties.GetValue("ApplicationCurrentMemoryUsage");
                    m_memoryPeak = (long)DeviceExtendedProperties.GetValue("ApplicationPeakMemoryUsage");

                    m_memoryTotoal = m_memoryTotoal / 1024;
                    m_memoryUsage = m_memoryUsage / 1024;
                    m_memoryPeak = m_memoryPeak / 1024;
                    */

                    // Microsoft.Phone.Info.DeviceStatus.DeviceTotalMemory;



                    m_memUpdate = 0;
                    m_timeLogic = mTimeLogic;
                    m_timeUpdate = mTimeUpdate;
                }

                m_info = string.Format("fps: {0}\ntotal: {1}\nusage: {2}\npeak: {3}\nlogic: {4}\nupdate: {5}", m_frameRate, m_memoryTotoal, m_memoryUsage, m_memoryPeak, m_timeLogic, m_timeUpdate);
            }
#endif


        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// ��������� ���������� �� �������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void draw(ASpriteBatch spriteBatch)
        {
#if (STATICS)
            //FPS
            m_frameCounter++;

            Vector2 pos = pointExt.toVector2(ASpriteBatch.viewPort) - AFonts.fontinSmall.MeasureString(m_info);

            spriteBatch.begin();
            spriteBatch.DrawString(AFonts.fontinSmall, m_info, pos, Color.White * 0.8f);
            spriteBatch.end();
#endif
        }






         ///=====================================================================================
        ///
        /// <summary>
        /// �������� ��������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void loadSettings()
        {
            culture.loadSettings(storage);
            sound.loadSettings(storage);
            music.loadSettings(storage);
            marketplace.loadSettings(storage);

        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// ���������� ��������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void saveSettings()
        {
            marketplace.saveSettings(storage);
            culture.saveSettings(storage);
            sound.saveSettings(storage);
            music.saveSettings(storage);
            storage.flush();
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// �������� �������� 
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void loadContent(ContentManager content)
        {
            sound.loadContent(content);
            music.loadContent(content);
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// ���������� ���������� ����������� �������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public string getDeviceGuid()
        {
            return storage.getDeviceGuid();
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// �������� ��� ��� ������ � ��������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isLike()
        {
            return false;
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// ������� ������� � ��������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void like()
        {
            
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// ���� ������������ ��������� FPS
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isHighSpeed()
        {
            return false;
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// ������� ������� � ��������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool highSpeed
        {
            get
            {
                return true;
            }
            set
            {

            }
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// ������ ��������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool runBrowser(string url)
        {
            //var prc = Process.Start(url);
            return false;
        }
        ///--------------------------------------------------------------------------------------


















    }
}
