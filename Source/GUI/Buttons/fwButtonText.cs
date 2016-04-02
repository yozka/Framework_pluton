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
    /// ������ c ��������
    /// 
    /// </summary>
    /// 
    ///------------------------------------------------------------------------------------------
    public class AButtonText
                    :
                        AControlButton
    {
        ///--------------------------------------------------------------------------------------
        public const int cWidth     = ATheme.buttonText_width;
        public const int cHeight    = ATheme.buttonText_height;
        ///--------------------------------------------------------------------------------------


        ///--------------------------------------------------------------------------------------
        private const int cImgWidth     = ATheme.buttonText_imgWidth;
        private const int cImgHeight    = ATheme.buttonText_imgHeight;
        ///--------------------------------------------------------------------------------------


        protected string mText; //����� ������
        ///--------------------------------------------------------------------------------------









        ///=====================================================================================
        ///
        /// <summary>
        /// �����������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AButtonText(AFrame parent, string text, int left, int top)
            : base(parent, left, top, cWidth, cHeight)
        {
            mText = text;
            mSoundClick = AFrameworkSettings.sound_buttonText;
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// ��������� ������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public string text
        {
            get
            {
                return mText;
            }
            set
            {
                mText = value;
            }
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


            Rectangle scrButton = new Rectangle(0, 0, cImgWidth, cImgHeight);

            float scale = 1.0f;
            Color colorText = ATheme.buttonText_color;
            
            //������ ������, ������� ������ ��� ��������
            if (m_pushDown)
            {
                scrButton = new Rectangle(0, cImgHeight * 1, cImgWidth, cImgHeight);
                scale = 1.02f;
            }

            //������ �������������, �� �������� ������������
            if (!m_enabled)
            {
                scrButton = new Rectangle(0, cImgHeight * 2, cImgWidth, cImgHeight);
                colorText = colorText * 0.5f;
            }

            if (m_checkbox)
            {
                colorText = ATheme.buttonText_colorCheck;
            }

            //�������� ������
            Vector2 pos = rect.Center.toVector2();
            spriteBatch.Draw(spriteBatch.getSprite(ATheme.buttonText_spriteID), pos, scrButton, Color.White, 0, new Vector2(cImgWidth / 2, cImgHeight / 2), scale, SpriteEffects.None, 0.5f);

     

            //�������� ��������
            SpriteFont font = AFonts.normal;
            Vector2 sz = font.MeasureString(mText);
            Vector2 sw = new Vector2(rect.Width, rect.Height);
            sz = (sw - sz) / 2 + new Vector2(rect.Left, rect.Top) + ATheme.buttonText_shift;
            spriteBatch.DrawString(font, mText, sz, colorText);
        }
        ///--------------------------------------------------------------------------------------





  








    }
}
