#region Using framework
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion






namespace Pluton.GraphicsElement
{
    ///--------------------------------------------------------------------------------------
    using Pluton.SystemProgram;
    ///--------------------------------------------------------------------------------------







     ///=====================================================================================
    ///
    /// <summary>
    /// Графический еэлемент, ввиде фоновой, движующейся картинки
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class ABackgroundSprite
    {
        readonly private uint mSpriteID;

        private Color mColor = Color.White;

        private float mRotateX = 0;
        private float mRotateY = 0;
        private float mSpeed = 100.0f;

        private float mRotationAngle = 0f; //угол поворота
        private float mRotationDirect = 0f;//в сторону поворота
        private float mRotationSpeed = 500f;//Скорость поворота
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Constructor 1
        /// передаются размеры объекта, в данном случаи размеры экрана
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public ABackgroundSprite(uint spriteID, float speed, Color color)
        {
            mSpeed = speed;
            mSpriteID = spriteID;
            mColor = color;
        }
        ///--------------------------------------------------------------------------------------












         ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка фонового экрана
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void render(ASpriteBatch spriteBatch, float depth)
        {
            Vector2 viewport = ASpriteBatch.viewPort.toVector2();

            
            Texture2D tSprite = spriteBatch.getSprite(mSpriteID);

            Vector2 sizeTexture = new Vector2(tSprite.Width, tSprite.Height);
            Vector2 sizeTextureDiv2 = sizeTexture / 2;


            Vector2 pos = new Vector2(mRotateX, mRotateY);
            while (pos.Y < viewport.Y)
            {
                while (pos.X < viewport.X)
                {
                    spriteBatch.Draw(tSprite,
                               pos + sizeTextureDiv2, null,
                               mColor,
                               mRotationAngle, sizeTextureDiv2, 1.0f, 0, depth);

                    pos.X += sizeTexture.X;
                }
                pos.Y += sizeTexture.Y;
                pos.X = mRotateX;
            }



            /*
             *  поставим новые координаты
             */
            float shift = (float)spriteBatch.gameTime.Milliseconds / mSpeed;
            mRotateX += shift * 1.3f;
            mRotateY += shift;
            if (mRotateX > 0)
            {
                mRotateX -= sizeTexture.X;
            }
            if (mRotateY > 0)
            {
                mRotateY -= sizeTexture.Y;
            }


            /*
             * Сделаем поворот картинки
             */
            if (mRotationDirect != 0.0f)
            {
                shift = (float)spriteBatch.gameTime.Milliseconds / mRotationSpeed;
                mRotationAngle += mRotationDirect * shift;

                const float rmin = (float)Math.PI * 2;
                if (mRotationAngle > rmin)
                {
                    mRotationAngle -= rmin;
                }
                else
                    if (mRotationAngle < 0)
                    {
                        mRotationAngle += rmin;
                    }

            }
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// Угол поворота картинки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public float rotationAngle
        {
            get
            {
                return mRotationAngle;
            }
            set
            {
                mRotationAngle = value;
            }
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// Направление вращения картинки -1 до 1, 0 = остановить вращение
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public float rotationDirect
        {
            get
            {
                return mRotationDirect;
            }
            set
            {
                mRotationDirect = value;
            }
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// Скорость вращения картинки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public float rotationSpeed
        {
            get
            {
                return mRotationSpeed;
            }
            set
            {
                mRotationSpeed = value;
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Цвет фона
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public Color color
        {
            get
            {
                return mColor;
            }
            set
            {
                mColor = value;
            }
        }

    }//ABackground
}