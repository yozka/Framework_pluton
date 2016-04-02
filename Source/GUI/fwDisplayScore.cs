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
        public const int cWidth = 288;
        public const int cHeight = 76;
        ///--------------------------------------------------------------------------------------





        ///--------------------------------------------------------------------------------------
        private string  mText = string.Empty;         //сам текст
        private Vector2 mPosText = Vector2.Zero;
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
            base(parent, left, top, cWidth, cHeight)
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

            Vector2 sz = AFonts.normal.MeasureString(mText);
            mPosText = new Vector2(73, 30) + (new Vector2(204, 30) - sz) / 2;
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
            //по умолчанию, выравнивание идет слева
            Vector2 pos = new Vector2(rect.X, rect.Y);



            spriteBatch.Draw(spriteBatch.getSprite(ATheme.displayScore_spriteID), pos, new Rectangle(0, 0, cWidth, cHeight), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(AFonts.normal, mText, pos + mPosText, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);


            //spriteBatch.primitives.drawBorder(rect, 2, Color.Blue);
        }
        ///--------------------------------------------------------------------------------------










    }
}
