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


        protected string mText; //текст кнопки
        ///--------------------------------------------------------------------------------------









        ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AButtonText(AFrame parent, string text, int left, int top)
            : base(parent, left, top, cWidth, cHeight)
        {
            mText = text;
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
                mText = value;
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
        protected override void onRender(ASpriteBatch spriteBatch, Rectangle rect)
        {
            //base.onRender(gameTime, spriteBatch, rect);


            Rectangle scrButton = new Rectangle(0, 0, cImgWidth, cImgHeight);

            float scale = 1.0f;
            Color colorText = ATheme.buttonText_color;
            
            //кнопка нажата, выведем другой тип картинок
            if (m_pushDown)
            {
                scrButton = new Rectangle(0, cImgHeight * 1, cImgWidth, cImgHeight);
                scale = 1.02f;
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
            Vector2 pos = rect.Center.toVector2();
            spriteBatch.Draw(spriteBatch.getSprite(ATheme.buttonText_spriteID), pos, scrButton, Color.White, 0, new Vector2(cImgWidth / 2, cImgHeight / 2), scale, SpriteEffects.None, 0.5f);

     

            //отрисуем название
            SpriteFont font = AFonts.normal;
            Vector2 sz = font.MeasureString(mText);
            Vector2 sw = new Vector2(rect.Width, rect.Height);
            sz = (sw - sz) / 2 + new Vector2(rect.Left, rect.Top) + ATheme.buttonText_shift;
            spriteBatch.DrawString(font, mText, sz, colorText);
        }
        ///--------------------------------------------------------------------------------------





  








    }
}
