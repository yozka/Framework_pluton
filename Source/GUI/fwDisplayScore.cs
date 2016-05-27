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
    /// виджет который показывает очки
    /// </summary>
    /// 
    ///------------------------------------------------------------------------------------------
    public class ADisplayScore
                :
                    AWidget
    {
        ///--------------------------------------------------------------------------------------





        ///--------------------------------------------------------------------------------------
        private string  mText = string.Empty;         //сам текст
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public ADisplayScore(AWidget parent)
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
        public ADisplayScore(AWidget parent, int left, int top)
            :
            base(parent, left, top, ATheme.displayScore_width, ATheme.displayTime_height)
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
        public void setScore(int score)
        {
            mText = string.Format("{0}", score);
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


            spriteBatch.Draw(spriteBatch.getSprite(ATheme.displayScore_spriteID), pos, null, Color.White * alpha, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);

            pos += ATheme.displayScore_shift;
            spriteBatch.DrawString(AFonts.normal, mText, pos, Color.White * alpha, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);

            //spriteBatch.primitives.drawBorder(rect, 2, Color.Blue);
    
        }
        ///--------------------------------------------------------------------------------------










    }
}
