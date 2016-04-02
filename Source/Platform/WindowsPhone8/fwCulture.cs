#region Using framework
using System;
using System.Globalization;
using System.Threading;
using System.Collections.Generic;
#endregion




namespace Pluton.SystemProgram.Devices
{
    ///--------------------------------------------------------------------------------------
    ///--------------------------------------------------------------------------------------





     ///=====================================================================================
    ///
    /// <summary>
    /// ������� �������� ���������
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class ACulture
    {
        ///--------------------------------------------------------------------------------------
        private string mLange;
        private List<CultureInfo> mInfo = new List<CultureInfo>();
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// constructor
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public ACulture()
        {
            mLange = Thread.CurrentThread.CurrentUICulture.Name;

#if (NO_EXCEPTION)
#else
            try
            {
#endif
                mInfo.Add(new CultureInfo("en-US"));
         
                mInfo.Add(new CultureInfo("ru-RU"));
                mInfo.Add(new CultureInfo("es-ES"));
#if (NO_EXCEPTION)
#else
            }
            catch (Exception e)
            {
                gAnalytics.trackException(e);
                
            }
#endif
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// �������� ��������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void loadSettings(AStorage settings)
        {
            /*
            //Resources.strings.Culture = new CultureInfo("ru-RU");

            CultureInfo lange = Thread.CurrentThread.CurrentUICulture;

            //lange = DefaultThreadCurrentUICulture();
            lange = CultureInfo.CurrentCulture;
            //Resources.strings.Culture = lange;
            */


            mLange = settings.readString("culture", mLange);
            Resources.strings.Culture = new CultureInfo(mLange);
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// ���������� ��������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void saveSettings(AStorage settings)
        {
            settings.writeString("culture", mLange);
        }
        ///--------------------------------------------------------------------------------------
       






         ///=====================================================================================
        ///
        /// <summary>
        /// ���������� ��������� ������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public int count
        {
            get
            {
                return mInfo.Count;
            }
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// �������� �����
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public string getDisplayName(int index)
        {
            if (index >= 0 && index < count)
            {
                string sName = mInfo[index].EnglishName;
                int id = sName.LastIndexOf(" (");
                if (id > 0)
                {
                    sName = sName.Remove(id);

                }
                return sName;
            }

            return string.Empty;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// �������� �����
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public string getName(int index)
        {
            if (index >= 0 && index < count)
            {
                return mInfo[index].Name;
            }

            return string.Empty;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// ��������, �������� �� ���� �������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isCurrent(string name)
        {
            return name == mLange ? true : false;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// ��������� �������� �����
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void setCurrent(string name)
        {
            mLange = name;
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// ��������� ������� ����
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public string getCurrent()
        {
            return mLange;
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// ����� �����
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public int find(string langFind)
        {
            for(int i = 0; i < mInfo.Count; i++)
            {
                if (mInfo[i].Name == langFind)
                {
                    return i;
                }
            }
            return -1;
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// ��������� ���� �� ���������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public string defaultCulture()
        {
            return mInfo[0].Name;
        }
        ///--------------------------------------------------------------------------------------



    }
}
