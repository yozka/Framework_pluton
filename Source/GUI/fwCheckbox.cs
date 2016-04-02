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
        static public readonly int cWidth = 260;
        static public readonly int cHeight = 80;
        ///--------------------------------------------------------------------------------------


        ///--------------------------------------------------------------------------------------
        static private readonly int cImgWidth = 64;
        static private readonly int cImgHeight = 64;
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
        protected override void onRender(ASpriteBatch spriteBatch, Rectangle rect)
        {
            //base.onRender(gameTime, spriteBatch, rect);


            uint spriteID = m_checkbox ? sprite.gui_element_checkmark_cheked : sprite.gui_element_checkmark_empty;

            Color colorText = Color.White;
            Color colorSprite = Color.White;

            //кнопка нажата, выведем другой тип картинок
            if (m_pushDown)
            {
                colorText = new Color(255, 108, 0);
            }

            //кнопка заблокирована, то поменяем прозрачность
            if (!m_enabled)
            {
                colorText = colorText * 0.5f;
                colorSprite = Color.DarkGray;
            }


            //отрисуем кнопки
            float scale = 1.0f;
            Vector2 pos = new Vector2(rect.Left + cImgWidth / 2, rect.Top + (rect.Height - cImgHeight) / 2 + cImgHeight / 2);
            spriteBatch.Draw(spriteBatch.getSprite(spriteID), pos, null, colorSprite, 0, new Vector2(cImgWidth / 2, cImgHeight / 2), scale, SpriteEffects.None, 0);



            spriteBatch.flush();
            //отрисуем название
            SpriteFont font = AFonts.normal;
            Vector2 sz = font.MeasureString(mText);
            Vector2 sw = new Vector2(rect.Width, rect.Height);
            sz = (sw - sz) / 2;
            spriteBatch.DrawString(font, mText, new Vector2(rect.Left + cImgWidth + 10, rect.Top + 0 + sz.Y), colorText);


            //spriteBatch.primitives.drawBorder(rect, 2, Color.Blue);
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
