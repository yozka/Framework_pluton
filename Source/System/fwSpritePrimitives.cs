#region Using framework
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

using Pluton.Collections;

namespace Pluton.SystemProgram
{
    ///------------------------------------------------------------------------------------






     ///=====================================================================================
    ///
    /// <summary>
    /// Примитивы для отрисовки базовых элементов
    /// 
    /// </summary>
    /// 
    /// -----------------------------------------------------------------------------------------
    public class ASpritePrimitives
    {
        ///--------------------------------------------------------------------------------------
        /// <summary>
        /// базовые текстуры
        /// </summary>
        static public Texture2D textureWhite;
        ///--------------------------------------------------------------------------------------


        
        
        ///--------------------------------------------------------------------------------------
        /// <summary>
        /// указатель на базовые функции работы с экраном
        /// </summary>
        private ASpriteBatch m_spriteBatch = null;
        ///--------------------------------------------------------------------------------------










         ///=====================================================================================
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        /// 
        /// -------------------------------------------------------------------------------------
        public ASpritePrimitives(ASpriteBatch spriteBatch)
        {
            m_spriteBatch = spriteBatch;

            textureWhite = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            textureWhite.SetData(new[] { Color.White });

        }
        ///--------------------------------------------------------------------------------------










        
         ///=====================================================================================
        ///
        /// <summary>
        /// орисовка линии
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void drawLineBlurred(Vector2 A, Vector2 B, float thickness, Color color)
        {
            Texture2D lightningSegment  = m_spriteBatch.getSprite(sprite.primitives_lightningSegment);
            Texture2D halfCircle        = m_spriteBatch.getSprite(sprite.primitives_halfCircle);

            Vector2 capOrigin = new Vector2(halfCircle.Width, halfCircle.Height / 2f);
            Vector2 middleOrigin = new Vector2(0, lightningSegment.Height / 2f);


            Vector2 tangent = B - A;
            float rotation = (float)Math.Atan2(tangent.Y, tangent.X);

            const float ImageThickness = 8 / 4;
            float thicknessScale = thickness / ImageThickness;

            Vector2 middleScale     = new Vector2(tangent.Length(), thicknessScale);

       
            m_spriteBatch.Draw(lightningSegment, A, null, color, rotation, middleOrigin, middleScale, SpriteEffects.None, 0f);
            m_spriteBatch.Draw(halfCircle, A, null, color, rotation, capOrigin, thicknessScale, SpriteEffects.None, 0f);
            m_spriteBatch.Draw(halfCircle, B, null, color, rotation + MathHelper.Pi, capOrigin, thicknessScale, SpriteEffects.None, 0f);
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// орисовка сегмента линии без закругления п краям
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void drawLineSegmentBlurred(Vector2 A, Vector2 B, float thickness, Color color)
        {
            Texture2D lightningSegment = m_spriteBatch.getSprite(sprite.primitives_lightningSegment);
            Vector2 middleOrigin = new Vector2(0, lightningSegment.Height / 2f);

            
            Vector2 tangent = B - A;
            float rotation = (float)Math.Atan2(tangent.Y, tangent.X);

            const float ImageThickness = 8 / 4;
            float thicknessScale = thickness / ImageThickness;

            Vector2 middleScale = new Vector2(tangent.Length(), thicknessScale);


            m_spriteBatch.Draw(lightningSegment, A, null, color, rotation, middleOrigin, middleScale, SpriteEffects.None, 0f);
        }
        ///--------------------------------------------------------------------------------------





 
         ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка прямоугольника
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void drawRectangle(Rectangle rect, Color color)
        {
            m_spriteBatch.Draw(textureWhite, rect, color);
        }
        ///--------------------------------------------------------------------------------------




         ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка прямоугольника
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void drawRectangle(Rectangle rect, Color color, float depth)
        {
            m_spriteBatch.Draw(textureWhite, rect, null, color, 0, Vector2.Zero, SpriteEffects.None, depth);
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка бордюра
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void drawBorder(Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor)
        {
            // Draw top line
            m_spriteBatch.Draw(textureWhite, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);

            // Draw left line
            m_spriteBatch.Draw(textureWhite, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y + thicknessOfBorder, thicknessOfBorder, rectangleToDraw.Height - thicknessOfBorder * 2), borderColor);

            // Draw right line
            m_spriteBatch.Draw(textureWhite, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder),
                                            rectangleToDraw.Y + thicknessOfBorder,
                                            thicknessOfBorder,
                                            rectangleToDraw.Height - thicknessOfBorder * 2), borderColor);
            // Draw bottom line
            m_spriteBatch.Draw(textureWhite, new Rectangle(rectangleToDraw.X,
                                            rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder,
                                            rectangleToDraw.Width,
                                            thicknessOfBorder), borderColor);
        }
        ///--------------------------------------------------------------------------------------





        ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка бордюра
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void drawLine(Vector2 A, Vector2 B, float thickness, Color color)
        {
            Vector2 tangent = B - A;
            float rotation = (float)Math.Atan2(tangent.Y, tangent.X);

            const float ImageThickness = 8 / 4;
            float thicknessScale = thickness / ImageThickness;

            Vector2 middleScale = new Vector2(tangent.Length(), thicknessScale);


            m_spriteBatch.Draw(textureWhite, A, null, color, rotation, new Vector2(0, 0.5f), middleScale, SpriteEffects.None, 0f);
        }
        ///--------------------------------------------------------------------------------------




         ///=====================================================================================
        ///
        /// <summary>
        /// отрисовка затеняющего экрана
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void drawFadeScreen(float alpha)
        {
            Point size = ASpriteBatch.viewPort;

            m_spriteBatch.Draw(textureWhite,
                              new Rectangle(0, 0, size.X, size.Y),
                              Color.Black * alpha);
        }


    }

}