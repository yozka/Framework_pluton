﻿#region Using framework
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
    public class AButtonIcon
                    :
                        AControlButton
    {
        ///--------------------------------------------------------------------------------------
        public const int cWidth         = ATheme.buttonIcon_width;      //def 130;
        public const int cHeight        = ATheme.buttonIcon_height;     //def 130;
        ///--------------------------------------------------------------------------------------


        ///--------------------------------------------------------------------------------------
        private const int cImgWidth     = ATheme.buttonIcon_imgWidth;   //def 128;
        private const int cImgHeight    = ATheme.buttonIcon_imgHeight;  //def 128;
        private const int cImgIcon      = ATheme.buttonIcon_imgIcon;    //def 64; //размер иконки
        ///--------------------------------------------------------------------------------------


        private uint mSpriteIconID         = 0; //спрайт иконки
        private uint mSpriteIconCheckID    = 0; //спрайт переключенной кнопки
        private uint mSpriteButtonID       = 0; //спрайт самой кнопки

        private float mScale            = 1.0f; //базовый размер кнопки
        private float mRotationIcon     = 0.0f; //поворот иконки
        private float mScaleIcon        = 1.0f; //размер иконки

        private Vector2 mIconShift      = ATheme.buttonIcon_shift; //смещение иконки
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AButtonIcon(uint spriteIcon)
            :
                this(null, spriteIcon, 0, sprite.gui_button_icon, 0, 0)
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
        public AButtonIcon(AWidget parent, uint spriteIcon)
            : 
                this(parent, spriteIcon, 0, sprite.gui_button_icon, 0, 0)
        {
            
        }
        ///--------------------------------------------------------------------------------------




   
         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор 2
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AButtonIcon(AWidget parent, uint spriteIcon, uint spriteButton)
            :
                this(parent, spriteIcon, 0, spriteButton, 0, 0)
        {
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор 3
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AButtonIcon(AWidget parent, uint spriteIcon, int left, int top)
            :
                this(parent, spriteIcon, 0, sprite.gui_button_icon, left, top)
        {
        }
        ///--------------------------------------------------------------------------------------




        ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор 5
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AButtonIcon(AWidget parent, uint spriteIcon, uint spriteButton, int left, int top)
            :
                this(parent, spriteIcon, 0, spriteButton, left, top)
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
        public AButtonIcon(AWidget parent, uint spriteIcon, uint spriteIconCheck, uint spriteButton, int left, int top)
            : base(parent, left, top, cWidth, cHeight)
        {
            mSpriteIconID           = spriteIcon;
            mSpriteIconCheckID      = spriteIconCheck;
            mSpriteButtonID         = spriteButton;
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
                width = (int)(cWidth * mScale);
                height = (int)(cHeight * mScale);
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Размер иконки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public float scaleIcon
        {
            get
            {
                return mScaleIcon;
            }
            set
            {
                mScaleIcon = value;
            }
        }
        ///--------------------------------------------------------------------------------------




         ///=====================================================================================
        ///
        /// <summary>
        /// поворот иконки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public float rotationIcon
        {
            get
            {
                return mRotationIcon;
            }
            set
            {
                mRotationIcon = value;
            }
        }
        ///--------------------------------------------------------------------------------------



 
         ///=====================================================================================
        ///
        /// <summary>
        /// смещение иконки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public Vector2 iconShift
        {
            get
            {
                return mIconShift;
            }
            set
            {
                mIconShift = value;
            }
        }
        ///--------------------------------------------------------------------------------------



         ///=====================================================================================
        ///
        /// <summary>
        /// устанавливаем кнопку
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void setSpriteButtonID(uint spriteID)
        {
            mSpriteButtonID = spriteID;
        }
        ///--------------------------------------------------------------------------------------




         ///=====================================================================================
        ///
        /// <summary>
        /// устанавливаем иконку
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void setSpriteIconID(uint spriteID)
        {
            mSpriteIconID = spriteID;
        }
        ///--------------------------------------------------------------------------------------




         ///=====================================================================================
        ///
        /// <summary>
        /// устанавливаем иконку
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void setSpriteIconCheckID(uint spriteID)
        {
            mSpriteIconCheckID = spriteID;
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
            float fScaleIcon = 1.0f;

            float fDepth = 0.5f;

            Rectangle srcRect = new Rectangle(0, 0, cImgWidth, cImgHeight);
            Color colorSprite = Color.White;

            //кнопка нажата, выведем другой тип картинок
            if (m_pushDown)
            {
                srcRect = new Rectangle(cImgWidth, 0, cImgWidth, cImgHeight);
                fScale = 1.03f;
                fScaleIcon = 1.2f;
                colorSprite = ATheme.buttonIcon_pushColor;// new Color(254, 236, 175);
            }

            //кнопка заблокирована, то поменяем прозрачность
            if (!m_enabled)
            {
                srcRect = new Rectangle(cImgWidth * 2, 0, cImgWidth, cImgHeight);
                colorSprite = ATheme.buttonIcon_disabledColor;// Color.Gray;
            }


            uint spriteIcon = mSpriteIconID;
            bool check = false;
            if (m_checkbox && mSpriteIconCheckID != 0)
            {
                spriteIcon = mSpriteIconCheckID;
            }
            else
            {
                check = m_checkbox;
            }


            fScale = fScale * mScale;
            fScaleIcon = fScaleIcon * mScale * mScaleIcon;


            //отрисуем кнопки
            Vector2 pos = screenCenter;
            if (mSpriteButtonID != 0)
            {
                spriteBatch.Draw(spriteBatch.getSprite(mSpriteButtonID), pos, srcRect, Color.White * alpha, 0, new Vector2(cImgWidth / 2, cImgHeight / 2), fScale, SpriteEffects.None, fDepth + 0.0003f);
            }

            if (spriteIcon != 0)
            {
                //buttonIcon_shift = new Vector2(0, -3);

                spriteBatch.Draw(spriteBatch.getSprite(spriteIcon), pos + mIconShift * fScale, null, colorSprite * alpha, mRotationIcon, new Vector2(cImgIcon / 2), fScaleIcon, SpriteEffects.None, fDepth + 0.0002f);
            }

            //чекбокс по умолчанию
            if (check && ATheme.buttonIcon_check_spriteID != 0)
            {
                //sprite.gui_circle
                spriteBatch.Draw(spriteBatch.getSprite(ATheme.buttonIcon_check_spriteID), pos + new Vector2(-40 * fScale), null, colorSprite * alpha, 0, new Vector2(64 / 2), fScaleIcon * 0.8f, SpriteEffects.None, fDepth + 0.0001f);
            }

            //spriteBatch.primitives.drawBorder(rect, 2, Color.Blue);
        }
        ///--------------------------------------------------------------------------------------













    }
}
