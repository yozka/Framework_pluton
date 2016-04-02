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
    /// ��� �������� ������, ��������� ������ �������, ����������, �������� ����� ��������
    /// �������� ����� �������
    /// </summary>
    /// 
    /// -------------------------------------------------------------------------------------
    public class AScreenManager : DrawableGameComponent
    {

        
        /// <summary>
        /// ��������� ���������.
        /// </summary>
        public readonly ADevices devices;





        private ADisplayInfo                mDisplayInfo = null;   //������� �������� �������
        private ASpriteBatch                mSpriteBatch = null;   //�������� ��� ��������� ��������
        private readonly ASpriteContent     mSpriteContent = null; //���������� ��������



        /*
         * ���������� ������� ���������
         */
        private bool                        mInitialized = false;
        private int                         mHContent = 1; //����������� ��������

        private bool mUpdateReboot = false; //���� ���� ��� ���� �������� ������
        private readonly List<AScreen>           mScreens = new List<AScreen>();
   
   
        private AScreen                          mFocusScreen = null;//������� ����� ������� ��������� � ������
        private readonly List<IScreenFrame>      mFrames = new List<IScreenFrame>(); //������ ��� ������� ���������
        ///--------------------------------------------------------------------------------------




        ///=====================================================================================
        ///
        /// <summary>
        /// ����������� ��������� �������.
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
        /// �������� ��������
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
        /// ���������� ������� �����
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
        /// ������������ ��������� �������
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

            base.Initialize(); //� ����� ������ ��������� LoadContent!!!

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
        /// �������� ������������ ��������
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
            //�������� ��������
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
        /// �������� ������������ �������� ����
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
        /// ��������� ������ ���������� �������
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
                //���� ���� �� ��������, �� ��� ������ �� ������, ����� ����������������� ���������
                Game.SuppressDraw();
                return;
            }

            mUpdateReboot = false;

            // ���������� ������ � ������
            // �������� � ��� �������
            devices.update(gameTime);

            //��������� �������
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
        /// ���������� ���� �������
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
                    //�������� ������
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
        /// ��������� �������.
        /// � ������ ������ ������, ������� ���������� ���������� ��������.
        /// ����� ������������ ������� �����
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
            
            //������������ 
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
        /// ���������� ������ ������ � �������� �������
        /// </summary>
        /// <param name="screen">����������� �����.</param>
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
             * ���������, ���� �� ������ ����� ��� ���.
             * ����� ��������� ��������, ����� ��������� ��� ���������� ������ � ���������
             */
            foreach (AScreen scr in mScreens)
            {
                if (scr == screen)
                {
                    screen.onReinclusion();
                    return false; //����� ����� ��� ����
                }
            }




  
            // ���� �� ��� �����������������, �� ���������� �������
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
        /// ����������� �������� ������, �� ���� ���������. 
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
        /// �������� ���� ����
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
        /// ��������� ������ ������� ��� ���������
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
        /// �������� ������ ���� ����, ��������� �������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void oneScreen(AScreen oneScreen)
        {
            //������� ��� ���� ����� ������
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
        /// ������� ����� ��� ���������
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
        /// ������ ���� ��� ���������
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
        /// ������� ������� ����������� �����
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
        /// ������� ������� ����������� �����
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
        /// ������ �������� ��������
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
        /// ������ �������� ��������
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
        /// ��������, ���� ������� ��� �������� ��� ���
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
        /// �������������� �������� �������
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