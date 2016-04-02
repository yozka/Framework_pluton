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
    /// ������ ����� ������
    /// 
    /// </summary>
    /// 
    ///------------------------------------------------------------------------------------------
    public class AButtonIcon
                    :
                        AControlButton
    {
        ///--------------------------------------------------------------------------------------
        public const int cWidth = 130;
        public const int cHeight = 130;
        ///--------------------------------------------------------------------------------------


        ///--------------------------------------------------------------------------------------
        private const int cImgWidth     = 128;
        private const int cImgHeight    = 128;
        private const int cImgIcon      = 64; //������ ������
        ///--------------------------------------------------------------------------------------


        private uint mSpriteIconID         = 0; //������ ������
        private uint mSpriteIconCheckID    = 0; //������ ������������� ������
        private uint mSpriteButtonID       = 0; //������ ����� ������

        private float mScale = 1.0f; //������� ������ ������
        ///--------------------------------------------------------------------------------------











         ///=====================================================================================
        ///
        /// <summary>
        /// �����������
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
        /// ����������� 2
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
        /// ����������� 3
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
        /// ����������� 5
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
        /// �����������
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
        /// ������ ������
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
        /// ������������� ������
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
        /// ��������� ��������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onRender(ASpriteBatch spriteBatch, Rectangle rect)
        {
            //base.onRender(gameTime, spriteBatch, rect);
            float alpha = this.alpha;

            float fScale = 1.0f;
            float fScaleIcon = 1.0f;

            Rectangle srcRect = new Rectangle(0, 0, cImgWidth, cImgHeight);
            Color colorSprite = Color.White;

            //������ ������, ������� ������ ��� ��������
            if (m_pushDown)
            {
                srcRect = new Rectangle(cImgWidth, 0, cImgWidth, cImgHeight);
                fScale = 1.03f;
                fScaleIcon = 1.2f;
                colorSprite = new Color(254, 236, 175);
            }

            //������ �������������, �� �������� ������������
            if (!m_enabled)
            {
                srcRect = new Rectangle(cImgWidth * 2, 0, cImgWidth, cImgHeight);
                colorSprite = Color.Gray;
            }


            uint spriteIcon = mSpriteIconID;
            if (m_checkbox && mSpriteIconCheckID != 0)
            {
                spriteIcon = mSpriteIconCheckID;
            }


            fScale = fScale * mScale;
            fScaleIcon = fScaleIcon * mScale;


            //�������� ������
            Vector2 pos = rect.Center.toVector2();
            if (mSpriteButtonID != 0)
            {
                spriteBatch.Draw(spriteBatch.getSprite(mSpriteButtonID), pos, srcRect, Color.White * alpha, 0, new Vector2(cImgWidth / 2, cImgHeight / 2), fScale, SpriteEffects.None, 0.3f);
            }

            if (spriteIcon != 0)
            {
                spriteBatch.Draw(spriteBatch.getSprite(spriteIcon), pos + new Vector2(0, -3), null, colorSprite * alpha, 0, new Vector2(cImgIcon / 2), fScaleIcon, SpriteEffects.None, 0.2f);
            }


            //spriteBatch.primitives.drawBorder(rect, 2, Color.Blue);
        }
        ///--------------------------------------------------------------------------------------













    }
}
