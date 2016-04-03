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
    /// текстовая метка
    /// </summary>
    /// 
    ///------------------------------------------------------------------------------------------
    public class ALabel
                :
                    AWidget
    {

        ///--------------------------------------------------------------------------------------
        private SpriteFont      mFonts      = AFonts.normal;    //шрифт которым будет рисоватся контрол
        private Color           mColor      = Color.White;      //цвет текста
        private string          mText       = string.Empty;     //сам текст
        private enAlign         mHrzAlign   = enAlign.left;     //выравнивание текста по горизонтали,  по умолчанию 
        private enAlign         mVrtAlign   = enAlign.top;      //выравнивание текста по вертикали,    по умолчанию 


        private int             mTextWidth  = 0;                    //размер текста в пикселях
        private int             mTextHeight = 0;
        private float           mScaleText      = 1.0f; //маштаб текста
        private bool            mScaleTextAuto  = false; //автоматический маштаб текста

        private Color           mShadowColor    = Color.White; //цвет тени
        private bool            mShadow         = false; //активность тени
        private int             mShadowLength   = 2; //глубина тени
        ///--------------------------------------------------------------------------------------



        /// <summary>
        /// выравнивание текста
        /// </summary>
        public enum enAlign
        {
            left,
            top,
            right,
            center,
            centerAuto,
            auto            //автоматический расширить

        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор 1
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public ALabel(AWidget parent, string text, SpriteFont fonts, Color color, enAlign hrzAlign, enAlign vrtAlign)
            :
            base(parent, 0, 0, 0, 0)
        {
            mFonts = fonts;
            mText = text;
            mColor = color;
            mHrzAlign = hrzAlign;
            mVrtAlign = vrtAlign;
            refresh();
        }
        ///--------------------------------------------------------------------------------------





        
         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор 2
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public ALabel(AWidget parent, string text, SpriteFont fonts, Color color, enAlign hrzAlign)
            :
            this(parent, text, fonts, color, hrzAlign, enAlign.auto)
        {


        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// Установка шрифта
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void setFont(SpriteFont fonts)
        {
            mFonts = fonts;
            refresh();
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// Установка цвета текста
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void setColor(Color color)
        {
            mColor = color;
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Установка цвета текста тени
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void setShadow(Color color, int length)
        {
            if (length <= 0)
            {
                mShadow = false;
                return;
            }

            mShadowColor = color;
            mShadowLength = length;
            mShadow = true;
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// Установка текста
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void setText(string text)
        {
            mText = text;
            refresh();
        }
        ///--------------------------------------------------------------------------------------




        ///=====================================================================================
        ///
        /// <summary>
        /// текст
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
                setText(value);
            }
        }
        ///--------------------------------------------------------------------------------------




         ///=====================================================================================
        ///
        /// <summary>
        /// Размер текста
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public float scaleText
        {
            get
            {
                return mScaleText;
            }
            set
            {
                mScaleText = value;
                refresh();
            }
        }
        ///--------------------------------------------------------------------------------------


        
         ///=====================================================================================
        ///
        /// <summary>
        /// Размер текста автоматический
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool scaleTextAuto
        {
            get
            {
                return mScaleTextAuto;
            }
            set
            {
                mScaleTextAuto = value;
                refresh();
            }
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// пересчитаем размеры контрола 
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public Vector2 sizeText
        {
            get
            {
                return mFonts.MeasureString(mText) * mScaleText;
            }
        }
        ///--------------------------------------------------------------------------------------





        ///=====================================================================================
        ///
        /// <summary>
        /// пересчитаем размеры контрола 
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void refresh()
        {
            mText = mText.toParser();

            Vector2 szActual = mFonts.MeasureString(mText);
            if (mScaleTextAuto)
            {
                float stw = contentWidth / szActual.X;
                float sth = contentHeight / szActual.Y;
                mScaleText = MathHelper.Clamp(Math.Min(stw, sth), 0.1f, 1.0f);
            }


            Vector2 sz = szActual * mScaleText;
            mTextWidth = (int)sz.X;
            mTextHeight = (int)sz.Y;


            //выравнивание погоризонтали
            if (mHrzAlign == enAlign.centerAuto)
            {
                width = (parent == null) ? mTextWidth : parent.contentWidth;
            }



            //выравнивание по вертикали
            if (mVrtAlign == enAlign.auto)
            {
                height = mTextHeight;
            }


            /*
            width = (parent == null) ? mTextWidth : parent.contentWidth;
            height = (int)sz.Y;
            */

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

            switch (mHrzAlign)
            {
                case enAlign.center:
                case enAlign.centerAuto:
                    {
                        //выравнивание поцентру
                        pos.X += (rect.Width - mTextWidth) / 2;
                        break;
                    }
                case enAlign.right:
                    {
                        pos.X += rect.Width - mTextWidth;
                        break;
                    }
            }

            switch (mVrtAlign)
            {
                case enAlign.center:
                    {
                        //выравнивание поцентру
                        pos.Y += (rect.Height - mTextHeight) / 2;
                        break;
                    }
            }

            float alpha = this.alpha;
            if (mShadow)
            {
                spriteBatch.DrawString(mFonts, mText, pos + new Vector2(mShadowLength) * mScaleText, mShadowColor * alpha, 0.0f, Vector2.Zero, mScaleText, SpriteEffects.None, 0.1f);
            }
            spriteBatch.DrawString(mFonts, mText, pos, mColor * alpha, 0.0f, Vector2.Zero, mScaleText, SpriteEffects.None, 0.0f);

            //spriteBatch.primitives.drawBorder(rect, 2, Color.Blue);
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// изменение размеров и позиция виджета
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onResize(bool changeLeft, bool changeTop, bool changeWidth, bool changeHeight)
        {
            base.onResize(changeLeft, changeTop, changeWidth, changeHeight);
            if (changeWidth || changeHeight)
            {
                refresh();
            }
        }
        ///--------------------------------------------------------------------------------------





    }
}
