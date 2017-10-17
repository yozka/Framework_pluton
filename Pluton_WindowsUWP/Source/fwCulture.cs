#region Using framework
using System;
using System.Globalization;
using System.Threading;
using System.Collections.Generic;
using System.Reflection;
#endregion







namespace Pluton.SystemProgram.Devices
{
    ///--------------------------------------------------------------------------------------
    using System.Resources;
    using Windows.ApplicationModel.Resources;
    using Windows.Globalization.Collation;
    using Resources;
    ///--------------------------------------------------------------------------------------



    ///=====================================================================================
    ///
    /// <summary>
    /// Загрузчик ресурсов
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AStringsManager : ResourceManager
    {
        private readonly ResourceLoader mResource;
        private readonly ResourceLoader mResourceDefault;

        private AStringsManager(string baseName, string defaultName, Assembly assembly)
            : base(baseName, assembly)
        {
            mResource = ResourceLoader.GetForViewIndependentUse(baseName);
            mResourceDefault = ResourceLoader.GetForViewIndependentUse(defaultName);
        }


        public static void register(string baseName, string defaultName, Type generate)
        {
            var resourceMan = new AStringsManager(baseName, defaultName, generate.GetTypeInfo().Assembly);

            var fields = generate.GetRuntimeFields();
            foreach (var obj in fields)
            {
                if (obj.Name == "resourceMan")
                {
                    obj.SetValue(null, resourceMan);
                }
            }
        }



        public override string GetString(string name, CultureInfo culture)
        {
            string value = mResource.GetString(name);
            return value != string.Empty ? value : mResourceDefault.GetString(name);
        }
    }



    ///=====================================================================================
    ///
    /// <summary>
    /// Система языковой поддержки
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class ACulture
    {
        ///--------------------------------------------------------------------------------------
        private string mLange;
        private readonly List<CultureInfo> mInfo = new List<CultureInfo>();
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


            CultureInfo ci = new CultureInfo(Windows.System.UserProfile.GlobalizationPreferences.Languages[0]);
            mLange = ci.TwoLetterISOLanguageName;

#if (NO_EXCEPTION)
#else
            try
            {
#endif
                foreach (var lang in AFrameworkSettings.culture)
                {
                    mInfo.Add(new CultureInfo(lang));
                }
        
#if (NO_EXCEPTION)
#else
            }
            catch (Exception)
            {
                
            }
#endif
            register();
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// возвратим системную культуру языка
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public static string cultureSystem()
        {
            CultureInfo ci = new CultureInfo(Windows.System.UserProfile.GlobalizationPreferences.Languages[0]);
            return ci.TwoLetterISOLanguageName;
        }
        ///--------------------------------------------------------------------------------------
        




        ///=====================================================================================
        ///
        /// <summary>
        /// Загрузка настроек
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void loadSettings(AStorage settings)
        {
            mLange = settings.readString("culture", mLange);
            register();
        }
        ///--------------------------------------------------------------------------------------







        ///=====================================================================================
        ///
        /// <summary>
        /// смена языка
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void register()
        {
            string nameFile = "resources." + mLange;
            AStringsManager.register(nameFile, "resources.en", typeof(Resources.strings));
        }
        ///--------------------------------------------------------------------------------------







        ///=====================================================================================
        ///
        /// <summary>
        /// Сохранение настроек
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
        /// количество доступных языков
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
        /// Название языка
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
        /// Название языка
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public string getName(int index)
        {
            if (index >= 0 && index < count)
            {
                return mInfo[index].TwoLetterISOLanguageName;
            }

            return string.Empty;
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// проверка, является ли язык текущим
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
        /// Установка текущего языка
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void setCurrent(string name)
        {
            mLange = name;
            register();
        }
        ///--------------------------------------------------------------------------------------





        ///=====================================================================================
        ///
        /// <summary>
        /// Возвратим текущий язык
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
        /// Поиск языка
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public int find(string langFind)
        {
            for (int i = 0; i < mInfo.Count; i++)
            {
                if (mInfo[i].TwoLetterISOLanguageName == langFind)
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
        /// Возвратим язык по умолчанию
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public string defaultCulture()
        {
            return mInfo[0].TwoLetterISOLanguageName;
        }
        ///--------------------------------------------------------------------------------------



    }
}
