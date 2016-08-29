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
    /// Виджет заголовок
    /// 
    /// </summary>
    /// 
    ///------------------------------------------------------------------------------------------
    public class ATitle
                :
                    AWidget
    {

        ///--------------------------------------------------------------------------------------
        private EStyleTytle mStyle          = EStyleTytle.none;
        private uint        mSpriteID       = 0;
        private int         mCaptionWidth   = 0;
        private int         mCaptionHeight  = 0;
        private Point       mOrigin         = Point.Zero;
        private Vector2     mShiftText      = Vector2.Zero;
        
        private bool        mFrontLocation  = true; //расположение заголовка, спереди он или сзади
        private SpriteFont  mFont           = null; //шрифт

        private string      mText           = string.Empty;
        private Vector2     mTextOrigin     = Vector2.Zero; //центр текста
        private Vector2     mTextScale      = Vector2.Zero; //размерность текста
        ///--------------------------------------------------------------------------------------





        ///--------------------------------------------------------------------------------------
        ///тип заголовка
        public enum EStyleTytle
        {
            none,
            tytle_A,
            tytle_B,
            tytle_C
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор 1
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public ATitle(AWidget parent, EStyleTytle style)
            :
            base(parent)
        {
            setStyle(style);
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// установка данных по умолчанию
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void setStyle(EStyleTytle style)
        {
            mStyle = style;
            switch (mStyle)
            {
                case EStyleTytle.tytle_A:
                    {
                        mSpriteID       = ATheme.dockwidget_title_A_spriteID;
                        width           = ATheme.dockwidget_title_A_width;
                        height          = ATheme.dockwidget_title_A_height;
                        mCaptionWidth   = ATheme.dockwidget_title_A_captionWidth;
                        mCaptionHeight  = ATheme.dockwidget_title_A_captionHeight;
                        mOrigin         = ATheme.dockwidget_title_A_origin;
                        mShiftText      = ATheme.dockwidget_title_A_shift;
                        mFrontLocation  = ATheme.dockwidget_title_A_frontLocation; //по умолчанию true
                        break;
                    }

                case EStyleTytle.tytle_B:
                    {
                        mSpriteID       = ATheme.dockwidget_title_B_spriteID;
                        width           = ATheme.dockwidget_title_B_width;
                        height          = ATheme.dockwidget_title_B_height;
                        mCaptionWidth   = ATheme.dockwidget_title_B_captionWidth;
                        mCaptionHeight  = ATheme.dockwidget_title_B_captionHeight;
                        mOrigin         = ATheme.dockwidget_title_B_origin;
                        mShiftText      = ATheme.dockwidget_title_B_shift;
                        mFrontLocation  = ATheme.dockwidget_title_B_frontLocation; //по умолчанию true
                        break;
                    }

                case EStyleTytle.tytle_C:
                    {
                        mSpriteID       = ATheme.dockwidget_title_C_spriteID;
                        width           = ATheme.dockwidget_title_C_width;
                        height          = ATheme.dockwidget_title_C_height;
                        mCaptionWidth   = ATheme.dockwidget_title_C_captionWidth;
                        mCaptionHeight  = ATheme.dockwidget_title_C_captionHeight;
                        mOrigin         = ATheme.dockwidget_title_C_origin;
                        mShiftText      = ATheme.dockwidget_title_C_shift;
                        mFrontLocation  = ATheme.dockwidget_title_C_frontLocation; //по умолчанию true
                        break;
                    }

            }

            mFont = ATheme.dockwidget_title_font;
            refresh();
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Пересчитать размер текста
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void refresh()
        {
            Vector2 ms = mFont.MeasureString(mText);
            mTextOrigin = ms / 2;


            float sw = mCaptionWidth / ms.X;
            float sh = mCaptionHeight / ms.Y;

            float stScale = Math.Min(sw, sh);
            stScale = MathHelper.Clamp(stScale, 0.5f, 1.0f);

            mTextScale = new Vector2(stScale);
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// установа текста заголова
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
                refresh();
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// тип ренедра, впереди или сздани основго виджета
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool frontLocation
        {
            get
            {
                return mFrontLocation;
            }
            set
            {
                mFrontLocation = value;
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// установа расположение. передается центр позиции
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void setPositionCenter(Point ptCenter)
        {
            setPosition(ptCenter.X - width / 2 + mOrigin.X, ptCenter.Y - height / 2 + mOrigin.Y);
        }
        ///--------------------------------------------------------------------------------------
     











         ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка виджета
        /// рисуем заголовок и фон окна
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onRender(ASpriteBatch spriteBatch, Rectangle rect)
        {
            if (mText == null)
            {
                return;
            }

            float alpha = this.alpha;

            Vector2 center = new Vector2(rect.Width, rect.Height) / 2;



            Vector2 pos = rect.Center.toVector2();
            spriteBatch.Draw(spriteBatch.getSprite(mSpriteID), pos, null, Color.White * alpha, 0, center, 1, SpriteEffects.None, 1.0f);



            pos += mShiftText;
            spriteBatch.DrawString(mFont, mText,
                                    pos + ATheme.dockwidget_title_shadowShift, //= new Vector2(-3, +3); defautl
                                    ATheme.dockwidget_title_colorTextShadow * alpha, 
                                    0, 
                                    mTextOrigin, mTextScale, SpriteEffects.None, 0.5f);

            spriteBatch.DrawString(mFont, mText,
                                    pos, 
                                    ATheme.dockwidget_title_colorText * alpha, 
                                    0,
                                    mTextOrigin, mTextScale, SpriteEffects.None, 0.1f);





        }
        ///--------------------------------------------------------------------------------------











    }
}
