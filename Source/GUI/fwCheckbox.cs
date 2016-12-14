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
    /// Чек бокс
    /// 
    /// </summary>
    /// 
    ///------------------------------------------------------------------------------------------
    public class ACheckbox
                    :
                        AControlButton
    {
        ///--------------------------------------------------------------------------------------
        private const int cWidth        = ATheme.checkbox_width;
        private const int cHeight       = ATheme.checkbox_height;
        ///--------------------------------------------------------------------------------------


        ///--------------------------------------------------------------------------------------
        private const int cImgWidth     = ATheme.checkbox_imgWidth;
        private const int cImgHeight    = ATheme.checkbox_imgHeight;
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
        public ACheckbox(AFrame parent, string text, int left, int top)
            : base(parent, left, top, cWidth, cHeight)
        {
            mText = text;
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


            uint spriteID = m_checkbox ? ATheme.checkbox_chekedSpriteID : ATheme.checkbox_emptySpriteID;

            Color colorText = ATheme.checkbox_color;
            Color colorSprite = Color.White;

            //кнопка нажата, выведем другой тип картинок
            if (m_pushDown)
            {
                colorText = ATheme.checkbox_colorPushDown;
            }

            //кнопка заблокирована, то поменяем прозрачность
            if (!m_enabled)
            {
                colorText = colorText * 0.5f;
                colorSprite = colorText * 0.5f;
            }

            int screenLeft      = this.screenLeft;
            int screenTop       = this.screenTop;
            int screenWidth     = this.screenWidth;
            int screenHeight    = this.screenHeight;

            //отрисуем кнопки
            float scale = 1.0f;
            Vector2 pos = new Vector2(screenLeft + cImgWidth / 2, screenTop + (screenHeight - cImgHeight) / 2 + cImgHeight / 2);
            spriteBatch.Draw(spriteBatch.getSprite(spriteID), pos, null, colorSprite, 0, new Vector2(cImgWidth / 2, cImgHeight / 2), scale, SpriteEffects.None, 0.5f);



            //отрисуем название
            SpriteFont font = AFonts.normal;
            Vector2 sz = font.MeasureString(mText);
            Vector2 sw = new Vector2(screenWidth, screenHeight);
            sz = (sw - sz) / 2;
            spriteBatch.DrawString(font, mText, new Vector2(screenLeft + cImgWidth + 10, screenTop + 0 + sz.Y), colorText, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);


        }
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
            checkbox = !checkbox;
        }
        ///--------------------------------------------------------------------------------------










    }
}
