#region Using framework
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion


namespace Pluton.GUI
{
    ///------------------------------------------------------------------------------------------
    using Pluton.SystemProgram;
    using Pluton.SystemProgram.Devices;
    ///------------------------------------------------------------------------------------------







     ///=========================================================================================
    ///
    /// <summary>
    /// табулятор
    /// вкладки
    /// </summary>
    /// 
    ///------------------------------------------------------------------------------------------
    public class ATabs
    {
        ///--------------------------------------------------------------------------------------
        private const int cTabWidth                 = ATheme.tabs_imgWidth; //размер табов
        private const int cTabHeight                = ATheme.tabs_imgHeight;
        private readonly Rectangle cTabActive       = new Rectangle(0, 0, cTabWidth, cTabHeight);
        private readonly Rectangle cTabInactive     = new Rectangle(0, cTabHeight, cTabWidth, cTabHeight);
        private const int cSizeIcon                 = ATheme.tabs_sizeIcon;  //размер иконок в табе
        ///--------------------------------------------------------------------------------------
       
        private readonly AWidget            mParent = null;
        private readonly List<uint>         mTabsIcon   = new List<uint>();     //иконки табуляционный вкладок
        private readonly List<AFrame>       mTabsFrame  = new List<AFrame>();   //фреймы вкладок
        private readonly List<int>          mTabsLeft   = new List<int>();      //позиция табуляторов
        
        
        private int     mIndex      = -1;       //текущий выбранный фрейм
        private bool    mPushDown   = false;    //удержание нажатия на кнопку
        private int     mLastIndex  = -1;       //предуыщий выбранный фрейм

        private int     mScreenTop = 0; //верхняя позиция где находятся табы
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public ATabs(AWidget parent)
        {
            this.mParent = parent;
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// поиск виджета по его имени, это сам виджет, либо его потомков
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AWidget findWidgetName(string name)
        {
            foreach (AWidget child in mTabsFrame)
            {
                AWidget find = child.findWidgetName(name);
                if (find != null)
                {
                    return find;
                }
            }
            return null;
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Добавление виджета в табулятор
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void addTab(uint icon, AFrame frame)
        {
            if (mIndex < 0)
            {
                //установим текущую страницу
                //поумолчанию будет первая
                mIndex = 0;
            }

            mTabsIcon.Add(icon);
            mTabsFrame.Add(frame);
            mTabsLeft.Add(0);
            if (frame != null)
            {
                frame.setParent(mParent);
                frame.width = mParent.contentWidth;
                frame.height = mParent.contentHeight;
            }
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Смена имени в закладке табулятора
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void setTabIcon(uint icon, int column)
        {
            if (column >= 0 && column < mTabsIcon.Count)
            {
                mTabsIcon[column] = icon;
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Установка активного виджета 
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void setIndex(int index)
        {
            if (index >= 0 && index < mTabsFrame.Count)
            {
                mIndex = index;

                if (mLastIndex != mIndex)
                {
                    if (signal_change != null)
                    {
                        signal_change(mIndex, mLastIndex);
                    }
                    mLastIndex = mIndex;
                }
            }
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// возврат активного таба
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public int indexTab
        {
            get
            {
                return mIndex;
            }
        }
        ///--------------------------------------------------------------------------------------





 
         ///=====================================================================================
        ///
        /// <summary>
        /// Возвратим текущий установленный виджет
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AFrame currentTab()
        {
            if (mIndex >= 0 && mIndex < mTabsFrame.Count)
            {
                return mTabsFrame[mIndex];
            }
            return null;
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// возвврат координат отрисовки вкладки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public Rectangle getScreenBoundTab(int index)
        {
            if (index >= 0 && index < mTabsLeft.Count)
            {
                return new Rectangle(mTabsLeft[index], mScreenTop, cTabWidth, cTabHeight);
            }
            return Rectangle.Empty;
        }
        ///--------------------------------------------------------------------------------------





        ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка виджета
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void draw(ASpriteBatch spriteBatch)
        {
            
            //рисуем заголовк табов
            spriteBatch.begin();
            
            
            int iCount = mTabsFrame.Count;

            Vector2 ptZero = mParent.screenLeftTop + mParent.contentLeftTop + ATheme.tabs_shift; //начальная точка
            Vector2 ptTabs = ptZero + new Vector2((mParent.contentWidth - iCount * cTabWidth) / 2, 0);

            float padding = ptTabs.X - ptZero.X + 1;

            //отрисовка табов
            mScreenTop = (int)ptTabs.Y;
            for (int i = 0; i < iCount; i++)
            {
                spriteBatch.Draw(spriteBatch.getSprite(ATheme.tabs_spriteID), ptTabs, i == mIndex ? cTabActive : cTabInactive, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);

                float scale = (i == mIndex) ? 0.65f : 0.6f;
                spriteBatch.Draw(spriteBatch.getSprite(mTabsIcon[i]), ptTabs + ATheme.tabs_shiftIcon, null, Color.White, 0.0f, new Vector2(cSizeIcon / 2), scale, SpriteEffects.None, 0.0f);

                mTabsLeft[i] = (int)ptTabs.X;
                ptTabs.X += cTabWidth;
            }
            
            //отрисовка полосок
            if (padding > 0.0f)
            {
                spriteBatch.Draw(spriteBatch.getSprite(ATheme.tabs_spriteID), ptZero, new Rectangle(0, 0, 1, cTabHeight), Color.White, 0.0f, Vector2.Zero, new Vector2(padding, 1.0f), SpriteEffects.None, 0.4f);
                spriteBatch.Draw(spriteBatch.getSprite(ATheme.tabs_spriteID), ptTabs, new Rectangle(0, 0, 1, cTabHeight), Color.White, 0.0f, Vector2.Zero, new Vector2(padding, 1.0f), SpriteEffects.None, 0.4f);
            

            }

            spriteBatch.end();
            //
            


            //рисуем текущий фрейм
            if (mIndex >= 0 && mIndex < mTabsFrame.Count)
            {
                var frame = mTabsFrame[mIndex];
                if (frame != null)
                {
                    frame.draw(spriteBatch);
                }
            }
            //
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// обработка данных ввода с тачпада и кнопок, возвращаем true когда произошла обработка
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool handleInput(AInputDevice input)
        {
            for (int i = 0; i < mTabsLeft.Count; i++)
            {
                if (input.containsRectangle(getScreenBoundTab(i)) >= 0)
                {
                    mPushDown = true;
                    mIndex = i;
                    return true;
                }
            }

            //нажали на вкладку и отпустили
            if (mPushDown)
            {
                if (mLastIndex != mIndex)
                {
                    if (signal_change != null)
                    {
                        signal_change(mIndex, mLastIndex);
                    }
                    mLastIndex = mIndex;

                    gSound.play(AFrameworkSettings.sound_tabs);
                }

            }
            //




            //передача управелния
            mPushDown = false;
            if (mIndex >= 0 && mIndex < mTabsFrame.Count)
            {
                var frame = mTabsFrame[mIndex];
                if (frame != null)
                {
                    frame.onHandleInput(input);
                }
            }
            return false;
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// изменение размеров
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void resize()
        {
            int maringTop = cTabHeight + (int)ATheme.tabs_shift.Y;
            Rectangle rect = new Rectangle(0, maringTop, mParent.contentWidth, mParent.contentHeight - maringTop);
            foreach(var frame in mTabsFrame)
            {
                if (frame != null)
                {
                    frame.rect = rect;
                }
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Обновление логики у контрола
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void update(TimeSpan gameTime)
        {
            if (mIndex >= 0 && mIndex < mTabsFrame.Count)
            {
                var frame = mTabsFrame[mIndex];
                if (frame != null)
                {
                    frame.onUpdate(gameTime);
                }
            }
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Сигнал на смену табов
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public delegate void eventChange(int index, int lastIndex);
        public event eventChange signal_change;
        ///--------------------------------------------------------------------------------------







    }
    ///------------------------------------------------------------------------------------------

}
