#region Using framework
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
//using Microsoft.Phone.Controls;
#endregion


using Pluton.SystemProgram.Devices;



namespace Pluton.SystemProgram
{
    ///------------------------------------------------------------------------------------














     ///=====================================================================================
    ///
    /// <summary>
    /// Это менеджер кранов, управляет стеком экранов, отрисовкой, переходы между экранами
    /// Передает фокус экранов
    /// </summary>
    /// 
    /// -------------------------------------------------------------------------------------
    public class AScreenManager : DrawableGameComponent
    {

        
        /// <summary>
        /// Контейнер устройств.
        /// </summary>
        public readonly ADevices devices;





        private ADisplayInfo                mDisplayInfo    = null; //текущие значение графики
        private ASpriteBatch                mSpriteBatch    = null; //Контекст для отрисовки спрайтов
        private readonly ASpriteContent     mSpriteContent  = null; //содержимое спрайтов



        /*
         * внутренние ресурсы менеджера
         */
        private bool                        mInitialized    = false;
        private int                         mHContent       = 1; //индификатор контента

        private bool mUpdateReboot = false; //флаг того что были изменены экраны
        private readonly List<AScreen>           mScreens = new List<AScreen>();
   
   
        private AScreen                          mFocusScreen = null;//текущий экран который находится в фокусе
        private readonly List<IScreenFrame>      mFrames = new List<IScreenFrame>(); //экраны для быстрой отрисовки
        ///--------------------------------------------------------------------------------------




        ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор менеджера экранов.
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AScreenManager(Game deviceGame)
            : base(deviceGame)
        {
            devices = new ADevices(this);
            mSpriteContent = new ASpriteContent(Game.Content.ServiceProvider);
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Выгрузка контента
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void Dispose(bool disposing)
        {
            mSpriteBatch = null;
            mSpriteContent.unload();
            base.Dispose(disposing);
        }
        ///--------------------------------------------------------------------------------------
     





         ///=====================================================================================
        ///
        /// <summary>
        /// Возвращаем текущий экран
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AScreen focusScreen
        {
            get
            {
                return mFocusScreen;
            }
        }
        ///--------------------------------------------------------------------------------------








        ///=====================================================================================
        ///
        /// <summary>
        /// Инциализация менеджера экранов
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void Initialize()
        {
#if (NO_EXCEPTION)
#else
            try
            {
#endif

            base.Initialize(); //в этотм методе выывается LoadContent!!!

            mInitialized = true;
            foreach (AScreen screen in mScreens)
            {
                screen.initialized(this);
            }




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
        /// Загрузка графического контента
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void LoadContent()
        {
#if (NO_EXCEPTION)
#else
            try
            {
#endif
            //загрузка девайсов
            var content = Game.Content;
            AFonts.loadContent(content);
            
            devices.loadContent(content);
            mSpriteContent.loadContent(content);



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
        /// Выгрузка графического контента игры
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void UnloadContent()
        {
            mSpriteContent.unloadContent();
            mSpriteBatch = null;
        }
        ///--------------------------------------------------------------------------------------








        ///=====================================================================================
        ///
        /// <summary>
        /// Обработка логики обновления экранов
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void Update(GameTime snapshotGameTime)
        {
#if (NO_EXCEPTION)
#else
            try
            {
                //-----
#endif

            TimeSpan gameTime = snapshotGameTime.ElapsedGameTime;
                


            if (!Game.IsActive)
            {
                //если игра не активная, то нах ничего не делаем, чтобы незлоупортреблять ресурсами
                Game.SuppressDraw();
                return;
            }

            mUpdateReboot = false;

            // опрашиваем тачпад и кнопки
            // вибрацию и все девайсы
            devices.update(gameTime);

            //загружаем спрайты
            mSpriteContent.update(gameTime);


            foreach (var frame in mFrames)
            {
                frame.update(gameTime);
                if (mUpdateReboot)
                {
                    break;
                }
            }


            updateScreens(gameTime);

#if (NO_EXCEPTION)
#else
                ///
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
        /// обновление всех экранов
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void updateScreens(TimeSpan gameTime)
        {
            mUpdateReboot = false;
            AScreen newFocus = null;
            
            /*
            foreach (AScreen screen in mScreens)
            {
                screen.update(gameTime);
                newFocus = screen;
                if (mUpdateReboot)
                {
                    newFocus = null;
                    break;
                }
            }
            */

            int count = mScreens.Count;
            for (int i = 0; i < count; i++)
            {
                var screen = mScreens[i];

                int next = i + 1;
                var screenNext = (next < count) ? mScreens[next] : null;
                if (!screen.isShow() || screenNext == null || screenNext.isPopup() || screenNext.isTransition())
                {
                    screen.update(gameTime);
                    newFocus = screen;
                    if (mUpdateReboot)
                    {
                        newFocus = null;
                        break;
                    }
                }
            }


            if (newFocus != null)
            {
                if (mFocusScreen != newFocus)
                {
                    //передача фокуса
                    if (mFocusScreen != null)
                    {
                        mFocusScreen.focusDeactivation(newFocus);
                    }
                    newFocus.focusActivation(mFocusScreen);
                    mFocusScreen = newFocus;
                }
            }


            if (mFocusScreen != null)
            {
                mFocusScreen.onHandleInput(devices.input);
            }
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка экранов.
        /// В буфере рисуем экраны, которым необходимы спецэффкты перехода.
        /// Далее отрисовываем верхний экран
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void Draw(GameTime snapshotGameTime)
        {
#if (NO_EXCEPTION)
#else
            try
            {
#endif
            if (mSpriteBatch == null)
            {
                return;
            }
            mUpdateReboot = false;
            mSpriteBatch.renderBegin(snapshotGameTime.ElapsedGameTime);
            GraphicsDevice.Clear(AFrameworkSettings.background);
            
            //отрисовываем 
            int count = mScreens.Count;
            for (int i = 0; i < count; i++)
            {
                var screen = mScreens[i];
                if (!screen.isVisible())
                {
                    continue;
                }
                
                int next = i + 1;
                var screenNext = (next < count) ? mScreens[next] : null;
                if (screenNext == null || screenNext.isPopup() || screenNext.isTransition() || !screenNext.isVisible())
                {
                    screen.onDraw(mSpriteBatch);
                    if (mUpdateReboot)
                    {
                        break;
                    }
                }
            }


            foreach (var frame in mFrames)
            {
                frame.draw(mSpriteBatch);
                if (mUpdateReboot)
                {
                    break;
                }
            }



            devices.draw(mSpriteBatch);
            mSpriteBatch.renderEnd();
#if (NO_EXCEPTION)
#else
                ///
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
        /// Добавление нового экрана в менеджер экранов
        /// </summary>
        /// <param name="screen">Добовляемый экран.</param>
        /// 
        /// -------------------------------------------------------------------------------------
        /// 
        public bool addScreen(AScreen screen)
        {
            if (screen == null)
            {
                return false;
            }

            /*
             * проверяем, есть ли данный экран или нет.
             * нужно исключить ситуацию, когда находятся два одинаковых экрана в менеджере
             */
            foreach (AScreen scr in mScreens)
            {
                if (scr == screen)
                {
                    screen.onReinclusion();
                    return false; //такой экран ужо есть
                }
            }




  
            // если мы уже инцеализировались, то подгружаем контент
            if (mInitialized)
            {
                screen.initialized(this);
            }

            mUpdateReboot = true;

            mScreens.Add(screen);

            screen.add();
            return true;
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Немедленное удаление экрана, из пула менеджера. 
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void removeScreen(AScreen screen)
        {
            if (mFocusScreen == screen)
            {
                screen.focusDeactivation(null);
                mFocusScreen = null;
            }

            if (mScreens.Contains(screen))
            {
                mUpdateReboot = true;
                mScreens.Remove(screen);
                screen.remove();
            }
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// закрытие всех окон
        /// </summary>
        /// 
        /// -------------------------------------------------------------------------------------
        /// 
        public void removeScreenAll()
        {
            while (mScreens.Count > 0)
            {
                removeScreen(mScreens[0]);
            }
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// Возвратим массив экранов для обработки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        /*
        public AScreen[] getScreens()
        {
            return m_screens.ToArray();
        }
         */
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Оставить только одно окно, остальные закрыть
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void oneScreen(AScreen oneScreen)
        {
            //закроем все окна кроме своего
            AScreen[] lst = mScreens.ToArray();
            foreach (AScreen scr in lst)
            {
                if (scr != oneScreen)
                {
                    scr.exitScreen();
                }
            }
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Добавим фрейм для обработки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void addScreenFrame(IScreenFrame frame)
        {
            if (!mFrames.Contains(frame))
            {
                mUpdateReboot = true;
                mFrames.Add(frame);
                frame.connectManager(this);
            }
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Удалим фрем для обработки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void removeScreenFrame(IScreenFrame frame)
        {
            mUpdateReboot = true;
            frame.disconnectManager(this);
            mFrames.Remove(frame);
        }
        ///--------------------------------------------------------------------------------------


        



         ///=====================================================================================
        ///
        /// <summary>
        /// Изменим текущий графический режим
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void initGraphics(ADisplayInfo info)
        {
            mDisplayInfo = info;
            if (mDisplayInfo.displayHardScreen.X < 200)
            {
                mDisplayInfo.displayHardScreen.X = 200;
            }

            if (mDisplayInfo.displayHardScreen.Y < 200)
            {
                mDisplayInfo.displayHardScreen.Y = 200;
            }

            resetGraphics();
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Изменим текущий графический режим
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void resetGraphics()
        {
            if (GraphicsDevice == null || mDisplayInfo == null || mSpriteContent == null)
            {
                return;
            }

            try
            {
                mSpriteBatch = new ASpriteBatch(GraphicsDevice, mDisplayInfo, mSpriteContent);
                foreach (var screen in mScreens)
                {
                    screen.onResetGraphics();
                }
            }
            catch (Exception e)
            {
                gAnalytics.trackException(e);
                mSpriteBatch = null;
            }
        }
        ///--------------------------------------------------------------------------------------





     




         ///=====================================================================================
        ///
        /// <summary>
        /// Полная выгрузка спрайтов
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public int spriteUnload(int hContent)
        {
            if (mHContent != hContent)
            {
                mHContent++;
                mSpriteContent.unload();


                foreach (var frame in mFrames)
                {
                    frame.spriteUnload(mHContent);
                }
            }
            return mHContent;
        }
        ///--------------------------------------------------------------------------------------




         ///=====================================================================================
        ///
        /// <summary>
        /// Полная выгрузка спрайтов
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void spriteUnload()
        {
            spriteUnload(0);
        }
        ///--------------------------------------------------------------------------------------




         ///=====================================================================================
        ///
        /// <summary>
        /// проверка, есть спрайты для загрузки или нет
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isSpriteLoading()
        {
            return mSpriteContent.isLoading();
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Принудительная загрузка спрайта
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void spriteLoadForced(uint spriteID)
        {
            mSpriteContent.loadForced(spriteID);
        }
        ///--------------------------------------------------------------------------------------



    }//AScreenManager
}