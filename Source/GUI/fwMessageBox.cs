#region Using framework
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion



namespace Pluton.GUI
{
    ///------------------------------------------------------------------------------------------
    using Pluton;
    using Pluton.SystemProgram;
    using Pluton.SystemProgram.Devices;
    ///------------------------------------------------------------------------------------------








     ///=========================================================================================
    ///
    /// <summary>
    /// Модальное окно MessageBox
    /// </summary>
    /// 
    ///------------------------------------------------------------------------------------------
    public class AMessageBox
                :
                    AScreen
    {

        ///--------------------------------------------------------------------------------------
        private const int cHeight = 288; //ширина бокса
        ///--------------------------------------------------------------------------------------



        private readonly AFrame         mGUI        = null;
        private readonly ALabel         mText       = null;
        private readonly AControlButton mButYES     = null;
        private readonly AControlButton mButNO      = null;

        private EType   mResult  = EType.NONE;

        ///--------------------------------------------------------------------------------------






         ///--------------------------------------------------------------------------------------
        /// <summary>
        /// Тип кнопок
        /// </summary>
        [Flags]
        public enum EType
        {
            NONE = 0,
            YES = 0x01,
            NO = 0x02
        }
        ///--------------------------------------------------------------------------------------





        ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор 3
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AMessageBox(EType buttons)
            :
            this(string.Empty, buttons, AFonts.normal)
        {
        }
        ///--------------------------------------------------------------------------------------





        
         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор 2
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AMessageBox(string caption, EType buttons)
            :
            this(caption, buttons, AFonts.normal)
        {
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор 1
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AMessageBox(string caption, EType buttons, SpriteFont font)
        {
            setPopup(true);
            
            timeShowing = TimeSpan.FromMilliseconds(600);
            timeHiding  = TimeSpan.FromMilliseconds(500);

            mGUI = new AFrame();

   

            //создание загаловка
            mText = new ALabel(mGUI, caption, font, ATheme.messageBox_color, ALabel.enAlign.center, ALabel.enAlign.center);
            mText.scaleTextAuto = true;
            mGUI.addWidget(mText);
            //


            //
            // создание кнопок
            // выесняем разметку кнопок
            //
            bool bYES = (buttons & EType.YES) == EType.YES;
            bool bNO = (buttons & EType.NO) == EType.NO;
            
            
            
      
            //создание кнопок
            if (bYES)
            {
                mButYES = new AButtonIcon(mGUI, sprite.gui_icon_ok, 0, 0);
                mButYES.signal_click += slot_buttonYES;
                mGUI.addWidget(mButYES);
            }

            if (bNO)
            {
                mButNO = new AButtonIcon(mGUI, sprite.gui_icon_cancel, 0, 0);
                mButNO.signal_click += slot_buttonNO;
                mGUI.addWidget(mButNO);
            }

            setCentralWidget(mGUI);
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// изменение размеров
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void resize()
        {
            mGUI.rect = new Rectangle(0, (ASpriteBatch.viewPort.Y - cHeight) / 2, ASpriteBatch.viewPort.X, cHeight);

            //размеры
            const int margin = 20;
            int y = cHeight - AButtonIcon.cHeight + 40;


            mText.width = mGUI.width - margin * 2;
            mText.height = y;
            mText.left = margin;
            mText.top = margin;

            y += 20;


            int x = (mGUI.contentWidth - AButtonIcon.cWidth) / 2;
            int next = mGUI.contentWidth / 2;

            if (mButYES != null && mButNO != null)
            {
                x -= next / 2;
            }

            if (mButYES != null)
            {
                mButYES.pos = new Point(x, y);
                x += next;
            }

            if (mButNO != null)
            {
                mButNO.pos = new Point(x, y);
                x += next;
            }
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Показ диалога
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void show(AScreenManager parent)
        {
            parent.addScreen(this);
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Переинциализация графики
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onResetGraphics()
        {
            resize();
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Перехват нажатия кнопок
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onHandleInput(AInputDevice input)
        {
            base.onHandleInput(input);
            if (!mGUI.onHandleInput(input))
            {
                if (input.isMenuCancel())
                {
                    exitScreen();
                }
            }
        }
        ///--------------------------------------------------------------------------------------


        




         ///=====================================================================================
        ///
        /// <summary>
        /// Логика игрового экрана
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onUpdate(TimeSpan gameTime)
        {
            mGUI.onUpdate(gameTime);
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// начало перехода экрана
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onTransitionShow()
        {
            resize();
            mGUI.left = -mGUI.width;
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Полностью показался экран
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onShow()
        {
            resize();
            buttonsEnabled(true);
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка заставки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onDraw(ASpriteBatch spriteBatch)
        {
            float anim = transition;

            if (isTransition())
            {
                float an = (1.0f - anim);
                an = tweener.exponential.easeIn(an, 0, 1, 1);

                mGUI.left = -(int)(an * (float)mGUI.width);
            }

            float colorAlpha = 0.5f + 0.5f * anim;

            spriteBatch.begin();
            
            //sprite.background_screen
            spriteBatch.drawSprite(ATheme.messageBox_backgroundID, new Rectangle(-10, -10, ASpriteBatch.viewPort.X + 20, ASpriteBatch.viewPort.Y + 20), Color.White * (0.9f * anim), 0, Vector2.Zero, 1.0f);
            spriteBatch.Draw(spriteBatch.getSprite(ATheme.messageBox_spriteID), mGUI.rect, null, Color.White * colorAlpha, 0, Vector2.Zero, SpriteEffects.None, 0.0f);
            spriteBatch.end();
            mGUI.draw(spriteBatch);
        }
        ///--------------------------------------------------------------------------------------









         ///=====================================================================================
        ///
        /// <summary>
        /// Виртуальный метод уберание экрана из менеджера
        /// запустим сигналы, с учетом того, какие нажали на кнопки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onRemove()
        {
            switch (mResult)
            {
                //нажатие кнопки YES
                case EType.YES:
                    {
                        if (signal_yes != null)
                        {
                            signal_yes();
                        }
                        break;
                    }


                //нажатие кнопки NO
                case EType.NO:
                    {
                        if (signal_no != null)
                        {
                            signal_no();
                        }
                        break;
                    }

            }

        }
        ///--------------------------------------------------------------------------------------





        
         ///=====================================================================================
        ///
        /// <summary>
        /// Выключение всех кнопок
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void buttonsEnabled(bool enabled)
        {
            if (mButYES != null)
            {
                mButYES.enabled = enabled;
            }

            if (mButNO != null)
            {
                mButNO.enabled = enabled;
            }
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Обработка нажатия на кнопки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public delegate void eventClick();
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Нажали на кнопку YES
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public event eventClick signal_yes;
        public void slot_buttonYES()
        {
            buttonsEnabled(false);
            mResult = EType.YES;
            exitScreen();
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Нажали на кнопку NO
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public event eventClick signal_no;
        public void slot_buttonNO()
        {
            buttonsEnabled(false);
            mResult = EType.NO;
            exitScreen();
        }
        ///--------------------------------------------------------------------------------------




         ///=====================================================================================
        ///
        /// <summary>
        /// текст сообщения
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public string text
        {
            get
            {
                return mText.text;
            }
            set
            {
                mText.text = value;
            }
        }
        ///--------------------------------------------------------------------------------------




    }//AMessageBox
}
