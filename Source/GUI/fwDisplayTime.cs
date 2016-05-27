#region Using framework
using System;
using System.Collections.Generic;
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
    /// Базовый контрол для GUI
    /// виджет который показывает время
    /// </summary>
    /// 
    ///------------------------------------------------------------------------------------------
    public class ADisplayTime
                :
                    AWidget
    {

        ///--------------------------------------------------------------------------------------






        ///--------------------------------------------------------------------------------------
        private string mText = string.Empty;         //сам текст
        ///--------------------------------------------------------------------------------------




         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public ADisplayTime(AWidget parent)
            :
            this(parent, 0, 0)
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
        public ADisplayTime(AWidget parent, int left, int top)
            :
            base(parent, left, top, ATheme.displayTime_width, ATheme.displayTime_height)
        {

        }
        ///--------------------------------------------------------------------------------------








        ///=====================================================================================
        ///
        /// <summary>
        /// Установка очков
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void setTime(TimeSpan time)
        {
            mText = string.Format("{0:00}:{1:00}", time.Minutes, time.Seconds);
        }
        ///--------------------------------------------------------------------------------------







        ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка метки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onRender(ASpriteBatch spriteBatch, Rectangle rect)
        {
            float alpha = this.alpha;
            
            //по умолчанию, выравнивание идет слева
            Vector2 pos = new Vector2(rect.X, rect.Y);


            spriteBatch.Draw(spriteBatch.getSprite(ATheme.displayTime_spriteID), pos, null, Color.White * alpha, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);

            pos += ATheme.displayTime_shift;
            spriteBatch.DrawString(AFonts.normal, mText, pos, Color.White * alpha, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);

            //spriteBatch.primitives.drawBorder(rect, 2, Color.Blue);
        }
        ///--------------------------------------------------------------------------------------










    }
}
