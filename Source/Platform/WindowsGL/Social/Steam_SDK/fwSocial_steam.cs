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
        public async void openAchievement(string name)
        {
            mSendAchievement.Add(name);
            //await Task.Run(sendAchievement());
            await sendAchievement();
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// асинхронная отправка сообщения
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        static Task sendAchievement()
        {
            //init();
            
            //SteamUserStats.SetAchievement(name);
            
        }
        ///--------------------------------------------------------------------------------------






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
