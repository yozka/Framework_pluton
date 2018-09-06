#region Using framework
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
#endregion





namespace Pluton.SystemProgram.Devices
{
    ///--------------------------------------------------------------------------------------






    ///=====================================================================================
    ///
    /// <summary>
    /// Фоновая музыка
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AMusicDevice
    {
        ///--------------------------------------------------------------------------------------
        private bool mEnabled = false; //включена выключена музыка
        private string mCurrent = null;

        private Dictionary<string, SoundEffect> mSounds = new Dictionary<string, SoundEffect>();
        private Dictionary<string, SoundEffectInstance> mSoundsOne = new Dictionary<string, SoundEffectInstance>();
        ///--------------------------------------------------------------------------------------





        ///=====================================================================================
        ///
        /// <summary>
        /// constructor
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AMusicDevice()
        {
            setDefault();
        }
        ///--------------------------------------------------------------------------------------















         ///=====================================================================================
        ///
        /// <summary>
        /// Загрузка основной музкыки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void loadContent(ContentManager content)
        {
            foreach (var name in AFrameworkSettings.musics)
            {
                loadMusic(name, content);
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Загрузка контента музыки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void loadMusic(string name, ContentManager content)
        {
            var sound = content.Load<SoundEffect>("Music\\" + name);
            var soundOne = sound.CreateInstance();
            mSounds[name] = sound;
            mSoundsOne[name] = soundOne;
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
            mEnabled = settings.readBoolean("music", mEnabled);
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
            settings.writeBoolean("music", mEnabled);
        }
        ///--------------------------------------------------------------------------------------
       







         ///=====================================================================================
        ///
        /// <summary>
        /// Установка музыки по умолчанию
        /// если на фоне ничего не играет, то галка поумолчанию будет вклчена
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void setDefault()
        {
            /*
            try
            {
                mEnabled = MediaPlayer.State != MediaState.Playing;
            }
            catch 
            {
                mEnabled = true;
            }*/
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// Включенна выключена фоновая музыка
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool enabled
        {
            get
            {
                return mEnabled;
            }
            set
            {
                mEnabled = value;
                if (mEnabled)
                {
                    playRepeat(mCurrent);
                }
                else
                {
                    playStop();
                }
            }
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Проигрывание музыки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void playRepeat(string name)
        {
            if (!mEnabled)
            {
                playStop();
                if (mSoundsOne.ContainsKey(name))
                {
                    mCurrent = name;
                }
                return;
            }

            if (name != null && mSoundsOne.ContainsKey(name))
            {
                if (name != mCurrent)
                {
                    playStop();
                }

                mCurrent = name;
                

                var sound = mSoundsOne[mCurrent];
                sound.IsLooped = true;
                if (sound.State == SoundState.Playing)
                {
                    return;
                }
                sound.Play();
            }
        }
        ///--------------------------------------------------------------------------------------








        ///=====================================================================================
        ///
        /// <summary>
        /// Проигрывание музыки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        /*
        public void playRepeat(Song song)
        {
            if (mEnabled && mCurrentSong != song)
            {
                mCurrentSong = song;
                play();
            }
            else
            {
                mCurrentSong = song;
            }

        }*/
        ///--------------------------------------------------------------------------------------








        ///=====================================================================================
        ///
        /// <summary>
        /// Проигрывание музыки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void play()
        {
            if (!mEnabled)
            {
                playStop();
                return;
            }

            if (mCurrent != null && mSoundsOne.ContainsKey(mCurrent))
            {
                var sound = mSoundsOne[mCurrent];
                //sound.IsLooped = false;
                if (sound.State == SoundState.Playing)
                {
                    return;
                }
                sound.Play();
            }
        }
        ///--------------------------------------------------------------------------------------




         ///=====================================================================================
        ///
        /// <summary>
        /// Остановка проигрывания музыки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void playStop()
        {
            if (mCurrent != null && mSoundsOne.ContainsKey(mCurrent))
            {
                var sound = mSoundsOne[mCurrent];
                sound.Stop();
            }
        }
        ///--------------------------------------------------------------------------------------






    }
}
