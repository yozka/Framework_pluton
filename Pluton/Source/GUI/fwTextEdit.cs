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
    /// Редактируемая кнопка
    /// 
    /// </summary>
    /// 
    ///------------------------------------------------------------------------------------------
    public class ATextEdit
                    :
                        AControlButton
    {
        ///--------------------------------------------------------------------------------------
        public const int cWidth = 290;
        public const int cHeight = 64;
        ///--------------------------------------------------------------------------------------


      
        protected string mText = string.Empty; //текст редактируемый
        
        private bool    mInputText      = false; //нужно ввести текст
        private string  mTitle          = string.Empty; //Загаловок ввода текста
        private string  mDescription    = string.Empty; //Описание текста ввода
        private bool    mPassword       = false; //признак того что вводим пароль
        ///--------------------------------------------------------------------------------------









         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public ATextEdit(AFrame parent, int left, int top)
            : base(parent, left, top, cWidth, cHeight)
        {
            
        }
        ///--------------------------------------------------------------------------------------







        ///=====================================================================================
        ///
        /// <summary>
        /// установка текста
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
        /// Название заголовка для воода текста
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public string title
        {
            get
            {
                return mTitle;
            }
            set
            {
                mTitle = value;
            }
        }
        ///--------------------------------------------------------------------------------------




         ///=====================================================================================
        ///
        /// <summary>
        /// Название описания для ввода текста
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public string description
        {
            get
            {
                return mDescription;
            }
            set
            {
                mDescription = value;
            }
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Признак того что редактируем пароль
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool password
        {
            get
            {
                return mPassword;
            }
            set
            {
                mPassword = value;
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


           

            Color colorText = Color.White;
            Color colorSprite = Color.White;

            //кнопка нажата, выведем другой тип картинок
            if (m_pushDown)
            {
               
                //colorText = new Color(255, 108, 0);
            }

            //кнопка заблокирована, то поменяем прозрачность
            if (!m_enabled)
            {
                colorText = colorText * 0.5f;
            }

            if (m_checkbox)
            {
                colorText = new Color(255, 108, 0);
            }

            //отрисуем кнопки
            Vector2 pos = screenLeftTop;
            
            int screenWidth = this.screenWidth;
            int screenHeight = this.screenHeight;
            //spriteBatch.Draw(spriteBatch.getSprite(sprite.gui_button_text), pos, scrButton, colorSprite, 0, new Vector2(cImgWidth / 2, cImgHeight / 2), scale, SpriteEffects.None, 0);



            //spriteBatch.flush();
            //отрисуем название
            SpriteFont font = AFonts.small;
            int iMarginLeft = 10;            
            if (mPassword && mText.Length > 0)
            {
                Vector2 sz = font.MeasureString("****");
                Vector2 sw = new Vector2(screenWidth, screenHeight);
                sz = pos + new Vector2(iMarginLeft, (sw.Y - sz.Y) / 2);
                spriteBatch.DrawString(font, "****", sz, colorText);
            }
            else
            {
                Vector2 sz = font.MeasureString(mText);
                Vector2 sw = new Vector2(screenWidth, screenHeight);
                sz = pos + new Vector2(iMarginLeft, (sw.Y - sz.Y) / 2);
                spriteBatch.DrawString(font, mText, sz, colorText);
            }


            spriteBatch.primitives.drawBorder(screenRect, 2, Color.Blue);
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
            mInputText = true;
        }
        ///--------------------------------------------------------------------------------------




        
         ///=====================================================================================
        ///
        /// <summary>
        /// обработка нажатий, если обработка удачная то возвращаем true
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override bool onHandleInput(AInputDevice input)
        {
            bool ret = base.onHandleInput(input);
            if (!ret && mInputText)
            {
                mInputText = false;
                input.showInputBox(mTitle, mDescription, mPassword ? "" : mText, slot_inputText);
            }
            return ret;
        }
        ///--------------------------------------------------------------------------------------




        
         ///=====================================================================================
        ///
        /// <summary>
        /// обработка нажатий, если обработка удачная то возвращаем true
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void slot_inputText(string value)
        {
            if (value != null && value != string.Empty)
            {
                text = value;
            }
        }



    }
}
