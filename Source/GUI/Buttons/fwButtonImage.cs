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
    /// Кнопка ввиде иконки
    /// 
    /// </summary>
    /// 
    ///------------------------------------------------------------------------------------------
    public class AButtonImage
                    :
                        AControlButton
    {
        ///--------------------------------------------------------------------------------------
        private uint    mImageID        = 0;    //спрайт
        private int     mImageWidth     = 0;
        private int     mImageHeight    = 0;
        private float   mScale          = 1.0f; //базовый размер кнопки
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AButtonImage(AWidget parent, uint imageID, Point size)
            :
                this(parent, imageID, 0, 0, size.X, size.Y)
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
        public AButtonImage(AWidget parent, uint imageID, int left, int top, int width, int height)
            : base(parent, left, top, width, height)
        {
            mImageID = imageID;
            mImageWidth = width;
            mImageHeight = height;

            mSoundClick = AFrameworkSettings.sound_buttonIcon;
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// Размер кнопки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public float scale
        {
            get
            {
                return mScale;
            }
            set
            {
                mScale = value;
                width = (int)(mImageWidth * mScale);
                height = (int)(mImageHeight * mScale);
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
        protected override void onRender(ASpriteBatch spriteBatch)
        {
            //base.onRender(gameTime, spriteBatch, rect);
            float alpha = this.alpha;

            float fScale = 1.0f;

            float fDepth = 0.5f;

            Color colorSprite = Color.White;

            //кнопка нажата, выведем другой тип картинок
            if (m_pushDown)
            {
                fScale = 1.03f;
                colorSprite = ATheme.buttonIcon_pushColor;
            }

            //кнопка заблокирована, то поменяем прозрачность
            if (!m_enabled)
            {
                colorSprite = ATheme.buttonIcon_disabledColor;
            }




            fScale = fScale * mScale;


            //отрисуем кнопки
            Vector2 pos = screenCenter;
            if (mImageID != 0)
            {
                spriteBatch.Draw(spriteBatch.getSprite(mImageID), pos, null, Color.White * alpha, 0, new Vector2(mImageWidth / 2, mImageHeight / 2), fScale, SpriteEffects.None, fDepth + 0.0003f);
            }


            //spriteBatch.primitives.drawBorder(rect, 2, Color.Blue);
        }
        ///--------------------------------------------------------------------------------------













    }
}
