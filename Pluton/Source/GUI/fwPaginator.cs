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
    ///------------------------------------------------------------------------------------------







     ///=========================================================================================
    ///
    /// <summary>
    /// Виджет нумерация страниц
    /// 
    /// </summary>
    /// 
    ///------------------------------------------------------------------------------------------
    public class APaginator
                :
                    AFrame
    {

        ///--------------------------------------------------------------------------------------
        private int mPageCount = 0; //количество страниц
        private int mCurrent = -1; //текущая выбранная страница
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор 0
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public APaginator(AWidget parent)
            :
                base(parent)
        {
            setMargin(0, 0, 0, ATheme.pagination_marginBottom);
        }
        ///--------------------------------------------------------------------------------------














         ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка виджета
        /// рисуем заголовок и фон окна
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onDrawBefore(ASpriteBatch spriteBatch)
        {
            spriteBatch.begin();


            const int widthPoint = ATheme.pagination_widthPoint;
            int x = (contentWidth - (mPageCount * widthPoint)) / 2 + screenLeft + widthPoint / 2;
            int y = screenTop + contentHeight;
            Vector2 pos = new Vector2(x, y);

            for (int i = 0; i < mPageCount; i++)
            {
                spriteBatch.Draw(spriteBatch.getSprite(i == mCurrent ? ATheme.pagination_activeSpriteID : ATheme.pagination_spriteID), pos, null, Color.White, 0.0f, new Vector2(ATheme.pagination_size / 2), 1.0f, SpriteEffects.None, 0.0f);
                pos.X += widthPoint;
            }


            //spriteBatch.primitives.drawBorder(rect, 2, Color.Blue);
            //spriteBatch.primitives.drawBorder(new Rectangle(screenLeft + marginLeft, screenTop + marginTop, contentWidth, contentHeight), 2, Color.Red);



            spriteBatch.end();


        }
        ///--------------------------------------------------------------------------------------



        


         ///=====================================================================================
        ///
        /// <summary>
        /// Установим количество страниц
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void setPageCount(int count)
        {
            mPageCount = count;
            if (mCurrent >= mPageCount)
            {
                current = -1;
            }
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// возвратим текущую страницу
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public int current
        {
            get
            {
                return mCurrent;
            }

            set
            {
                if (mCurrent != value)
                {
                    mCurrent = value;
                    if (mCurrent >= mPageCount)
                    {
                        mCurrent = -1;
                    }

                    if (signal_change != null)
                    {
                        signal_change(mCurrent);
                    }
                }
            }
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// сигнал что изменилась текущая страница
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public delegate void eventChange(int index);
        public event eventChange signal_change;
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// проверка, можно илил нет двигатся в перед
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isNext()
        {
            return mCurrent < mPageCount - 1;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// навигация по страницам вперед
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void next()
        {
            if (mCurrent < mPageCount - 1)
            {
                current++;
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// проверка, можно или нет двигатся назад
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isPrev()
        {
            return (mCurrent > 0);
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// навигация по страницам назад
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void prev()
        {
            if (mCurrent > 0)
            {
                current--;
            }
        }
        ///--------------------------------------------------------------------------------------



    }
}
