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
        private readonly List<string> mSendAchievement = new List<string>();
        private bool mInit = false;

        private Task mSendTask = null;
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
            
  
            
            mSendAchievement.Add(name);

            if (mSendTask != null && !mSendTask.IsCompleted)
            {
                return;
            }

            Task.Run(() =>
            {
                init();
                while (mSendAchievement.Count > 0)
                {
                    string sName = mSendAchievement[0];
                    mSendAchievement.Remove(sName);
                    SteamUserStats.SetAchievement(sName);

                    SteamAPI.RunCallbacks();
                    SteamUserStats.StoreStats();
                }
            });
        }
        ///---------------------------------------------------------------------------------------





        




         ///=====================================================================================
        ///
        /// <summary>
        /// инциализация сервиса
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void init()
        {
            if (mInit)
            {
                return;
            }

            mInit = SteamAPI.Init();
            var b1 = SteamUserStats.RequestCurrentStats();
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
