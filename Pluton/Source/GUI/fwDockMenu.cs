#region Using framework
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
#endregion





namespace Pluton.GUI
{
    ///------------------------------------------------------------------------------------------
    using Pluton.SystemProgram;
    using Pluton.SystemProgram.Devices;
    using Pluton.GraphicsElement;
    ///------------------------------------------------------------------------------------------









     ///=========================================================================================
    ///
    /// <summary>
    /// GUI контрол показывающие выезжающее меню
    ///
    /// </summary>
    /// 
    ///------------------------------------------------------------------------------------------
    public class ADockMenu
            :
                AFrame
    {
        ///--------------------------------------------------------------------------------------
        //private const int cIconSize = 128;
        private const int cAnimation = 150; //скорость анимации пунктов

        private readonly AControlButton mButCaption = null; //кнопка заголовок


        ///--------------------------------------------------------------------------------------
        private ETypeState  mState = ETypeState.hide; //скрыть или показать меню
        private TimeSpan    mAnimation = TimeSpan.Zero;
        private int         mItemAnimation = 0; //текущий анимируемый элемент
        private bool        mResizeItemShow = false; //изменить размеры перед показом
        ///--------------------------------------------------------------------------------------




         ///--------------------------------------------------------------------------------------
        /// <summary>
        /// Текущее деййствия состояние машины
        /// </summary>
        protected enum ETypeState
        {
            /// <summary>
            /// скрыто
            /// </summary>
            hide,

            /// <summary>
            /// скрывает и анимирует 
            /// </summary>
            hideAnimat,


            /// <summary>
            /// активно показывает
            /// </summary>
            show,

            /// <summary>
            /// анимация при показе
            /// </summary>
            showAnimat

        };
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public ADockMenu(AWidget parent)
            :
            base(parent, 0, 0, ATheme.dockmenu_buttonWidth /*100*/, ATheme.dockmenu_buttonHeight /*80*/)
        {

            mButCaption = new AButtonIcon(this, ATheme.dockmenu_spriteID, 0, ATheme.dockmenu_buttonID, 0, 0); //dockmenu_buttonID = 0
            mButCaption.size = new Point(ATheme.dockmenu_buttonWidth, ATheme.dockmenu_buttonHeight); //dockmenu_buttonHeight = 80
            mButCaption.signal_click += onClickMenu;
            addWidget(mButCaption);

        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// включить выключить видемость заголовка
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool visibleCaption
        {
            get
            {
                return mButCaption.visible;
            }
            set
            {
                bool bSave = mButCaption.visible;
                mButCaption.visible = value;
                if (mButCaption.visible != bSave)
                {
                    resizeItems();
                }
            }
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// показывать или не показывать виджет
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override bool onVisible(bool value)
        {
            visibleCaption = value;
            if (!value && value != m_visible)
            {
                hideItems();
            }

            return value;
        }
        ///--------------------------------------------------------------------------------------

        




         ///=====================================================================================
        ///
        /// <summary>
        /// немедленно все закрыть
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void hideItems()
        {
            for (int i = 0; i < countChilds; i++)
            {
                var action = child(i) as AActionMenu;
                if (action != null)
                {
                    action.hideTo(-action.width, action.top);
                }
            }
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// пересчитать перед показом
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void resizeItemShow()
        {
            mResizeItemShow = true;
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// изменение размеров и позиция виджета
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onResize(bool changeLeft, bool changeTop, bool changeWidth, bool changeHeight)
        {
            base.onResize(changeLeft, changeTop, changeWidth, changeHeight);
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// обычный нажиматор
        /// обратнаый метод
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public delegate void eventClick(AActionMenu action);
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Добавим новый элемент меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AActionMenu addAction(string text, eventClick onClick)
        {
            return addAction(text, 0, onClick);
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Добавим новый элемент меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AActionMenu addAction(string text, uint spriteID, eventClick onClick)
        {
            var action = addAction(spriteID, 0, onClick);
            action.text = text;
            action.left = -action.width;
            return action;
        }
        ///--------------------------------------------------------------------------------------




        
         ///=====================================================================================
        ///
        /// <summary>
        /// Добавим новый элемент меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AActionMenu addAction(uint spriteID, eventClick onClick)
        {
            return addAction(spriteID, 0, onClick);
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Добавим новый элемент меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AActionMenu addAction(uint spriteID, uint spriteCheckID, eventClick onClick)
        {
            AActionMenu action = new AActionMenu(this, spriteID, spriteCheckID);
            action.signal_actionClick += onClick;
            return addAction(action);
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Добавим новый элемент меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AActionMenu addAction(AActionMenu action)
        {
           // action.size = new Point(128, 128);
            action.visible = false;
            action.left = -action.width;
            addWidget(action);


            //расположем все элементы
            resizeItems();


            hide();
            return action;
        }
        ///--------------------------------------------------------------------------------------

       
        
         ///=====================================================================================
        ///
        /// <summary>
        /// расположем все элементы меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void resizeItems()
        {
            int iTop = 0;
            foreach (var obj in childs)
            {
                var item = obj as AActionMenu;

                //if (obj.visible || obj != mButCaption)
                if (item != null && item.visiblePopup || (obj.visible && obj == mButCaption))
                {
                    obj.top = iTop;
                    iTop += obj.height;
                }
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Нажали на кнопку показа выезжающего меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void onClickMenu()
        {
            if (mState != ETypeState.hide)
            {
                hide();
            }
            else
            {
                show();
            }

        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Сигнал начало показа меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public event eventVoid signal_show;
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Покажем меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void show()
        {
            if (mState == ETypeState.show || mState == ETypeState.showAnimat)
            {
                return;
            }

            if (mState == ETypeState.hide && mResizeItemShow)
            {
                resizeItems();
                mResizeItemShow = false;
            }


            mState = ETypeState.showAnimat;
            mAnimation = TimeSpan.FromMilliseconds(cAnimation);
            mItemAnimation = 1; //анимируем элемент

            if (signal_show != null)
            {
                signal_show();
            }

        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Сигнал начало скрытия меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public event eventVoid signal_hide;
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Скроем меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void hide()
        {
            if (mState == ETypeState.hide || mState == ETypeState.hideAnimat)
            {
                return;
            }

            mState = ETypeState.hideAnimat;
            mAnimation = TimeSpan.FromMilliseconds(cAnimation);
            mItemAnimation = countChilds - 1; //анимируем элемент с конца

            if (signal_hide != null)
            {
                signal_hide();
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// обработка данных ввода с тачпада и кнопок, возвращаем true когда произошла обработка
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override bool onHandleInput(AInputDevice input)
        {
            if (base.onHandleInput(input))
            {
                return true;
            }

            if (input.touchIndex() >= 0 && (mState == ETypeState.show || mState == ETypeState.showAnimat))
            {
                hide();
            }
            return false;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Обновление логики у контрола
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onUpdate(TimeSpan gameTime)
        {
            base.onUpdate(gameTime);

            switch (mState)
            {
                case ETypeState.showAnimat:
                    {
                        mAnimation -= gameTime;
                        if (mAnimation.TotalMilliseconds > 0)
                        {
                            return;
                        }
                        mAnimation = TimeSpan.FromMilliseconds(cAnimation);
                        var action = child(mItemAnimation) as AActionMenu;
                        if (action == null)
                        {
                            mState = ETypeState.show;
                            return;
                        }
                        action.showTo(0, action.top);
                        mItemAnimation++; //следующий элемент
                        break;
                    }

                //скроем
                case ETypeState.hideAnimat:
                    {
                        mAnimation -= gameTime;
                        if (mAnimation.TotalMilliseconds > 0)
                        {
                            return;
                        }
                        mAnimation = TimeSpan.FromMilliseconds(cAnimation);
                        var action = child(mItemAnimation) as AActionMenu;
                        if (action == null || mItemAnimation <= 0)
                        {
                            mState = ETypeState.hide;
                            return;
                        }
                        action.hideTo(-action.width, action.top);
                        mItemAnimation--; //следующий элемент
                        break;
                    }


            }

        }
        ///--------------------------------------------------------------------------------------



        
         ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка контрола
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onRender(ASpriteBatch spriteBatch)
        {
            base.onRender(spriteBatch);
        }








    }
    ///------------------------------------------------------------------------------------------
    ///------------------------------------------------------------------------------------------
    ///------------------------------------------------------------------------------------------
    ///------------------------------------------------------------------------------------------
    ///------------------------------------------------------------------------------------------
    ///------------------------------------------------------------------------------------------
    ///------------------------------------------------------------------------------------------
    ///------------------------------------------------------------------------------------------
    ///------------------------------------------------------------------------------------------
    ///------------------------------------------------------------------------------------------
    ///------------------------------------------------------------------------------------------







    
     ///=========================================================================================
    ///
    /// <summary>
    /// Кнопка один пункт меню
    /// </summary>
    /// 
    ///------------------------------------------------------------------------------------------
    public class AActionMenu
            :
                AControlButton
    {
        ///--------------------------------------------------------------------------------------
        private const int cWidth = 130; //размер центральной кнопки
        private const int cHeight = 130; //высота кнопки
        ///--------------------------------------------------------------------------------------

        
        ///--------------------------------------------------------------------------------------
        private const int cImgWidth     = 128; //размер картинки кнопки
        private const int cImgHeight    = 128;
        private const int cImgIcon      = 64; //размер иконки
        ///--------------------------------------------------------------------------------------

        private readonly uint mSpriteIconID         = 0; //спрайт иконки
        private readonly uint mSpriteIconCheckID    = 0; //спрайт переключенной кнопки
        private readonly uint mSpriteButtonID       = 0; //спрайт самой кнопки


        
        ///--------------------------------------------------------------------------------------
        private readonly AAnimationShowTween mAnimation = new AAnimationShowTween(800, tweener.exponential.easeOut);


        private Vector2 mDest = Vector2.Zero; //куда нужно анимироватся
        private Vector2 mSourc = Vector2.Zero; //начальная позиция
        private bool    mAnimActive = false; //активная анимация


        private string mText = string.Empty; //надпись на кнопке
        private SpriteFont mFontText = null; //используемый шрифт
        private Vector2 mSizeText = Vector2.Zero;

        private bool mVisiblePopup = true; //признак показа в качестве вслявающей подсказки
        ///--------------------------------------------------------------------------------------

        

         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AActionMenu()
        {

        }
        ///--------------------------------------------------------------------------------------




         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AActionMenu(AFrame parent, uint spriteIcon, uint spriteIconCheck)
            :
                base(parent)
        {
            mSpriteIconID = spriteIcon;
            mSpriteIconCheckID = spriteIconCheck;
            mSpriteButtonID = sprite.gui_button_icon_left;


            mFontText = ATheme.dockmenu_font;//AFonts.small;
            resizeItem();
        }
        ///--------------------------------------------------------------------------------------




         ///=====================================================================================
        ///
        /// <summary>
        /// установка текста
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public string text
        {
            get
            {
                return mText;
            }
            set
            {
                mText = value;
                resizeItem();
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// флаг показа пункта меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool visiblePopup
        {
            get
            {
                return mVisiblePopup;
            }
            set
            {
                bool save = mVisiblePopup;
                mVisiblePopup = value;

                if (mVisiblePopup != save)
                {
                    var menu = parent as ADockMenu;
                    menu.resizeItemShow();
                }
            }
        }
        ///--------------------------------------------------------------------------------------



         ///=====================================================================================
        ///
        /// <summary>
        /// вычесление размера кнопки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void resizeItem()
        {
            Point sz = new Point(cWidth, cHeight);
            mSizeText = mFontText.MeasureString(mText);


            if (mSpriteIconID != 0)
            {
                sz.X = (int)mSizeText.X + cWidth - 0;
            }
            else
            {
                sz.X = (int)mSizeText.X + cWidth - 60;
            }

            size = sz;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Обработка нажатия на кнопку
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public event ADockMenu.eventClick signal_actionClick;
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// нажатие на кнопку
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onClick()
        {
            if (signal_actionClick != null)
            {
                signal_actionClick(this);
            }
            ADockMenu menu = parent as ADockMenu;
            if (menu != null)
            {
                menu.hide();
            }
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// покажем с анимацией пункт меню в указанную точку
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void showTo(int iLeft, int iTop)
        {
            visible = true;
            mSourc = new Vector2(left, top);
            mDest = new Vector2(iLeft, iTop);
            mAnimation.show();
            mAnimActive = true;

            if (!mVisiblePopup)
            {
                mAnimation.setHide();
                mAnimActive = false;
                visible = false;
            }
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// скроем с анимацией пункт меню в указанную точку
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void hideTo(int iLeft, int iTop)
        {
            mSourc = new Vector2(left, top);
            mDest = new Vector2(iLeft, iTop);
            if (visible)
            {
                mAnimation.hide();
                mAnimActive = true;
                return;
            }
            mAnimActive = false;
            setPosition(iLeft, iTop);
        }
        ///--------------------------------------------------------------------------------------




        
         ///=====================================================================================
        ///
        /// <summary>
        /// Обновление логики у контрола
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onUpdate(TimeSpan gameTime)
        {
            base.onUpdate(gameTime);
        }         
        ///--------------------------------------------------------------------------------------
            



            
         ///=====================================================================================
        ///
        /// <summary>
        /// анимация
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void animation(TimeSpan gameTime)
        {
            if (!mAnimActive)
            {
                return;
            }
            mAnimation.update(gameTime);

            float fAnim = mAnimation;
            if (mAnimation.isHiding())
            {
                fAnim = 1.0f - fAnim;
            }

            Vector2 pos = ((mDest - mSourc) * fAnim) + mSourc;


            setPosition((int)pos.X, (int)pos.Y);
            if (mAnimation.isHide())
            {
                mAnimActive = false;
                visible = false;
                setPosition((int)mDest.X, (int)mDest.Y);
            }
            else
            if (mAnimation.isShow())
            {
                mAnimActive = false;
                setPosition((int)mDest.X, (int)mDest.Y);
            }

        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка контрола
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onRender(ASpriteBatch spriteBatch)
        {
            animation(spriteBatch.gameTime);

            float fAlpha = alpha;
            float fScale = 1.0f;
            float fScaleIcon = 1.0f;

            Rectangle srcRect = new Rectangle(0, 0, cImgWidth, cImgHeight);
            Color colorSprite = Color.White;
            Color colorText = ATheme.dockmenu_text;

            //кнопка нажата, выведем другой тип картинок
            if (m_pushDown)
            {
                srcRect = new Rectangle(cImgWidth, 0, cImgWidth, cImgHeight);
                fScale = 1.03f;
                fScaleIcon = 1.2f;
                colorSprite = ATheme.dockmenu_push;
                colorText   = ATheme.dockmenu_push;
            }

            //кнопка заблокирована, то поменяем прозрачность
            if (!m_enabled)
            {
                srcRect = new Rectangle(cImgWidth * 2, 0, cImgWidth, cImgHeight);
                colorSprite = Color.Gray;
            }


            uint spriteIcon = mSpriteIconID;
            if (m_checkbox && mSpriteIconCheckID != 0)
            {
                spriteIcon = mSpriteIconCheckID;
            }

            int screenLeft      = this.screenLeft;
            int screenTop       = this.screenTop;
            int screenWidth     = this.screenWidth;
            int screenHeight    = this.screenHeight;
            int screenRight     = this.screenRight;

            //отрисуем кнопки
            Vector2 pos = new Vector2(screenRight - cWidth / 2, screenTop + cHeight / 2);
            if (mSpriteButtonID != 0)
            {
                //основной правй элемент
                spriteBatch.Draw(spriteBatch.getSprite(mSpriteButtonID), pos, srcRect, Color.White, 0, new Vector2(cImgWidth / 2, cImgHeight / 2 - 3), fScale, SpriteEffects.None, 0.1f);

                //растяжка элемента
                float fStretch = screenWidth - cWidth + 1;
                if (fStretch > 0)
                {

                    Vector2 posStretch = new Vector2(screenLeft, pos.Y);
                    Rectangle scrStretch = new Rectangle(srcRect.Left, srcRect.Top, 10, srcRect.Height);
                    spriteBatch.Draw(spriteBatch.getSprite(mSpriteButtonID), posStretch, scrStretch, Color.White, 0, new Vector2(0, cImgHeight / 2 - 3), new Vector2(fScale * fStretch / 10, fScale), SpriteEffects.None, 0.11f);
                }

                if (mText != null && mText != string.Empty)
                {
                    Vector2 posText = new Vector2(rect.Left + ((mSizeText.Y + mSizeText.X) / 2.0f), pos.Y);
                    spriteBatch.DrawString(mFontText, mText, posText + new Vector2(2, 2) * fScale, ATheme.dockmenu_shadow, 0.0f, mSizeText / 2, fScale, SpriteEffects.None, 0.01f);
                    spriteBatch.DrawString(mFontText, mText, posText, colorText, 0.0f, mSizeText / 2, fScale, SpriteEffects.None, 0.0f);
                }
            }

            if (spriteIcon != 0)
            {
                spriteBatch.Draw(spriteBatch.getSprite(spriteIcon), pos, null, colorSprite, 0, new Vector2(cImgIcon / 2), fScaleIcon, SpriteEffects.None, 0.05f);
            }


            //spriteBatch.primitives.drawBorder(rect, 2, Color.Blue);
        }
        ///--------------------------------------------------------------------------------------


    }



}
