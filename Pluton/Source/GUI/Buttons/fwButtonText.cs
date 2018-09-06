#region Using framework
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion


using Pluton;
using Pluton.SystemProgram;
using Pluton.SystemProgram.Devices;


namespace Pluton.GUI
{







    ///=========================================================================================
    ///
    /// <summary>
    /// Кнопка c надписью
    /// 
    /// </summary>
    /// 
    ///------------------------------------------------------------------------------------------
    public class AButtonText
                    :
                        AControlButton
    {
        ///--------------------------------------------------------------------------------------
        public const int cWidth     = ATheme.buttonText_width;
        public const int cHeight    = ATheme.buttonText_height;
        ///--------------------------------------------------------------------------------------


        ///--------------------------------------------------------------------------------------
        private const int cImgWidth     = ATheme.buttonText_imgWidth;
        private const int cImgHeight    = ATheme.buttonText_imgHeight;
        ///--------------------------------------------------------------------------------------

        private readonly uint mSpriteID = ATheme.buttonText_spriteID;


        private string      mText       = string.Empty; //текст кнопки
        private Vector2     mTextOrigin = Vector2.Zero; //централизация текста у кнопки
        private Vector2     mTextScale  = Vector2.Zero; //размер текста чтобыы вс янадпись влезла
        private SpriteFont  mFont       = null;         //шрифт
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AButtonText(AFrame parent, string text, int left, int top)
            : this(parent, text, left, top, ATheme.buttonText_spriteID)
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
        public AButtonText(AFrame parent, string text, int left, int top, uint spriteID)
            : base(parent, left, top, cWidth, cHeight)
        {
            mSpriteID = spriteID;

            mFont = ATheme.buttonText_font;

            this.text = text;
            refresh();
            mSoundClick = AFrameworkSettings.sound_buttonText;
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
                mText = value.toParser();
                refresh();
            }
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
            Vector2 sz = mFont.MeasureString(mText);
            mTextOrigin = sz / 2;
            

            float fw = ATheme.buttonText_textWidth / sz.X;
            float fh = ATheme.buttonText_textHeight / sz.Y;

            float fs = Math.Min(fw, fh);
            fs = MathHelper.Clamp(fs, 0.5f, 1.0f);

            mTextScale = new Vector2(fs);
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
            //base.onRender(gameTime, spriteBatch, rect);
            Vector2 screenCenter = this.screenCenter;

            Rectangle scrButton = new Rectangle(0, 0, cImgWidth, cImgHeight);
            float fAlpha = alpha;
            float fScale = 1.0f;
            Color colorText = ATheme.buttonText_color;
            
            //кнопка нажата, выведем другой тип картинок
            if (m_pushDown)
            {
                scrButton = new Rectangle(0, cImgHeight * 1, cImgWidth, cImgHeight);
                fScale = ATheme.buttonText_pushdownScale;//1.02f;
            }

            //кнопка заблокирована, то поменяем прозрачность
            if (!m_enabled)
            {
                scrButton = new Rectangle(0, cImgHeight * 2, cImgWidth, cImgHeight);
                colorText = colorText * 0.5f;
            }

            if (m_checkbox)
            {
                colorText = ATheme.buttonText_colorCheck;
            }

            //отрисуем кнопки
            Vector2 pos = screenCenter;
            spriteBatch.Draw(spriteBatch.getSprite(mSpriteID), pos + ATheme.buttonText_imgShift, scrButton, Color.White * fAlpha, 0, new Vector2(cImgWidth / 2, cImgHeight / 2), fScale, SpriteEffects.None, 0.5f);

     

            //отрисуем название
            Vector2 ptPos = screenCenter + ATheme.buttonText_shift;


            //отрисовка тени на словом
#pragma warning disable 162
            if (ATheme.buttonText_isShadow)
            {
                spriteBatch.DrawString(mFont, mText, ptPos + ATheme.buttonText_shiftShadow, ATheme.buttonText_colorShadow * fAlpha, 0.0f, mTextOrigin, mTextScale * fScale * ATheme.buttonText_scaleShadow, SpriteEffects.None, 0.1f);
            }
#pragma warning restore 162

            spriteBatch.DrawString(mFont, mText, ptPos, colorText * fAlpha, 0.0f, mTextOrigin, mTextScale * fScale, SpriteEffects.None, 0.0f);
            
        }
        ///--------------------------------------------------------------------------------------





  








    }
}
