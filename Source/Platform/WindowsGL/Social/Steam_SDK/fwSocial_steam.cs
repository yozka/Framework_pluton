using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Pluton.Social
{
    ///--------------------------------------------------------------------------------------
    using Pluton;
    using Pluton.SystemProgram.Devices;

    using Steamworks;
    ///--------------------------------------------------------------------------------------







     ///=====================================================================================
    ///
    /// <summary>
    /// Steam SDK
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class ASocial_steam
        :
            ISocial
    {
        ///--------------------------------------------------------------------------------------
        private readonly List<string> mAchievement = new List<string>(); //все открытые ачивки
        private bool mInit = false;
        private bool mProcess = false; //процесс запущен
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public ASocial_steam()
        {

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
            string all = settings.readString("steamAchiv", string.Empty);
            if (all != string.Empty)
            {
                foreach (string name in all.Split('$'))
                {
                    if (name != string.Empty && !mAchievement.Contains(name))
                    {
                        mAchievement.Add(name);
                    }
                }
            }
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
            string all = mAchievement.join("$");
            settings.writeString("steamAchiv", all);
        }
        ///--------------------------------------------------------------------------------------
       






         ///=====================================================================================
        ///
        /// <summary>
        /// открытие ачивки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void openAchievement(string name)
        {
            /*
            init();
            SteamUserStats.SetAchievement(name);
            SteamAPI.RunCallbacks();
     //       SteamUserStats.ResetAllStats(true);
            SteamUserStats.StoreStats();
            */
            
            mAchievement.Add(name);
            reopenAchievement();
        }
        ///---------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// переоткроем ачивки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void reopenAchievement()
        {
            if (mProcess)
            {
                return;
            }

            mProcess = true;
            Task.Run(() =>
            {
                if (!init())
                {
                    mProcess = false;
                    return;
                }
                
                int i = 0;
                while (i < mAchievement.Count)
                {
                    string sName = mAchievement[i];
                    i++;
                    if (sName == string.Empty)
                    {
                        continue;
                    }

                    bool pbAchieved = false;
                    bool ok = SteamUserStats.GetAchievement(sName, out pbAchieved);
                    if (ok && !pbAchieved)
                    {
                        SteamUserStats.SetAchievement(sName);
                        SteamAPI.RunCallbacks();
                        SteamUserStats.StoreStats();
                    }
                }

                mProcess = false;
            });
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// инциализация сервиса
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private bool init()
        {
            if (mInit)
            {
                return mInit;
            }

            mInit = SteamAPI.Init();
            if (mInit)
            {
                SteamUserStats.RequestCurrentStats();
            }

            return mInit;
        }
        ///--------------------------------------------------------------------------------------







        /*
            //

            private void test()
            {
                int i = 0;

                bool ok = SteamAPI.Init();
                var steamId = SteamUser.GetSteamID();

                var v1 = SteamUtils.GetAppID();


                var b1 = SteamUserStats.RequestCurrentStats();
            
                var v2 = SteamUserStats.GetNumAchievements();
            
                string name = SteamUserStats.GetAchievementName(0);
                //SteamUserStats.SetAchievement(

                var v3 = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagAll);
                i--;
            }

            */







        ///--------------------------------------------------------------------------------------
    }
}
