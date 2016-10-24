#region Using framework
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion


//старый

#if RENDER_DEBUG
using System.Diagnostics;
#endif


namespace Pluton.GUI
{
    ///------------------------------------------------------------------------------------------
    using Pluton;
    using Pluton.SystemProgram;
    using Pluton.SystemProgram.Devices;
    using Pluton.GraphicsElement;
    ///------------------------------------------------------------------------------------------








     ///=========================================================================================
    ///
    /// <summary>
    /// контрл вертикального скроллинга
    /// 
    /// </summary>
    /// 
    ///------------------------------------------------------------------------------------------
    public class AScrollVertical
                :
                    AWidget
    {

        ///--------------------------------------------------------------------------------------
        private const int cMargin = 20; //отступы  слева и справа для управляющих элементов



        private float   mBegin = 0;             //начало отрисовки строчек (впикселях)
        private float   mEnd = 0;               //конец отрисовки в пикселях
        private Point   mToch = Point.Zero;     //последнии координаты
        private bool    mScroll = false;        //скроллинг
        private bool    mInputItem = false;     //идет опрос клавишь у данных
        private int     mInputCount = 0;        //количество пропусков для передачи управления потомкам

        private int     mVerticalSpacing = 0;   //отступы между горизонтальными элементами
        ///--------------------------------------------------------------------------------------

        private bool    mMove = false; //движение скролинга
        private float   mMoveStart = 0;
        private float   mMoveStop = 0;
        private AAnimationShowTween mMoveAnimation = new AAnimationShowTween(400, tweener.quadratic.easeInOut);
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AScrollVertical(AWidget parent, int left, int top, int width, int height)
            :
            base(parent, left, top, width, height)
        {

        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// расстояние между элементами
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public int verticalSpacing
        {
            get
            {
                return mVerticalSpacing;
            }
            set
            {
                mVerticalSpacing = value;
            }
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Возратим количество элементов
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected virtual int onItemCount()
        {
            return 0;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// возвратим высоту элемента
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected virtual int onItemHeight()
        {
            return 0;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка одного элемента
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected virtual void onRenderItem(ASpriteBatch spriteBatch, Rectangle rect, int index)
        {
        }
        ///--------------------------------------------------------------------------------------












         ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка контрола с учетом располжения родителя
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onRender(ASpriteBatch spriteBatch, Rectangle rect)
        {
            int clipMargin = 00;
            int clipTop = rect.Top - clipMargin;
            int clipHeight = rect.Height + clipMargin * 2;
            if (clipHeight + clipTop > ASpriteBatch.viewPort.Y)
            {
                clipHeight = ASpriteBatch.viewPort.Y - clipTop;
            }
            spriteBatch.beginClipping(new Rectangle(rect.Left, clipTop, rect.Width, clipHeight));

            //отрисова item
            int iCountItem = onItemCount();
            int iHeightItem = onItemHeight();
            if (iHeightItem == 0)
            {
                iHeightItem = 1;
            }
            int iHeightSpace = iHeightItem + mVerticalSpacing;

            int iBegin = (int)(mBegin / iHeightSpace);
            int iEnd = (int)(mEnd / iHeightSpace);
            Rectangle itemRect = new Rectangle(rect.Left, (int)(rect.Top - mBegin) % iHeightSpace + cMargin, rect.Width, iHeightItem);
            for (int i = iBegin; i <= iEnd; i++)
            {
                if (i >= 0 && i < iCountItem)
                {
                    onRenderItem(spriteBatch, itemRect, i);
                }
                itemRect.Y += iHeightSpace;
            }
            //

            spriteBatch.endClipping();


            //отрисовка плашек
            //левая плашка
            //spriteBatch.Draw(spriteBatch.getSprite(sprite.gui_scroll_horizontal_margin), new Rectangle(rect.Left - 8, rect.Top, 16, rect.Height), Color.White);
            //


            //правая плашка
            int iRightIcon = rect.Right - 8;
            //spriteBatch.Draw(spriteBatch.getSprite(sprite.gui_scroll_horizontal_margin), new Rectangle(iRightIcon, rect.Top, 16, rect.Height), Color.White);
            //

            
            //spriteBatch.primitives.drawBorder(rect, 2, Color.Red);

        }
        ///--------------------------------------------------------------------------------------








        ///=====================================================================================
        ///
        /// <summary>
        /// опрос одного элемента
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected virtual bool onHandleInputItem(AInputDevice input, int index)
        {
            return false;
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

            //1 обработка нажатие pushdown
            int index = input.containsRectangle(screenRect);
            if (index >= 0)
            {
                Point pt = input.touch(index);
                if (mInputItem)
                {
                    int iItemHeight = onItemHeight() + mVerticalSpacing;

                    int id = (pt.Y + (int)mBegin - screenTop) / (iItemHeight == 0 ? 1 : iItemHeight);
                    mInputItem = (id >= 0 && id < onItemCount()) ? onHandleInputItem(input, id) : false;

#if RENDER_DEBUG
                    Debug.WriteLine(id);
#endif
                    return true;
                }

                if (mToch.isZero())
                {
                    //нажали первый раз
                    mInputCount = 0;
                    mInputItem = false;
                    mScroll = false;
                    mToch = pt;
                    return true;
                }
                Point ptDiff = pointExt.sub(pt, mToch);
                
                //нету сдвигов
                bool isScrolling = pointExt.toVector2(ptDiff).Length() > 2 ? true : false;
                if (!mScroll && !isScrolling)
                {
                    mInputCount++;
                    if (mInputCount > 3)
                    {
                        mInputItem = true;
                    }
                }


                if (pointExt.isZero(ptDiff))
                {
                    //скроллинга нет
                    return true;
                }

                //начался сдвиг
                if (isScrolling)
                {
                    mScroll = true;
                }
                mToch = pt;
                scrolling(ptDiff);


                //уничтожим обработанный индекс
                input.release(index);
                return true;
            }
            if (!pointExt.isZero(mToch))
            {
                mToch = Point.Zero;
                mInputItem = false;
                mScroll = false;
                mInputCount = 0;
                onHandleInputItem(input, -1);//потеря фокуса
            }
            return false;
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// скроллированние данных
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void scrolling(Point ptDiff)
        {
            mMove = false;
            mBegin += ptDiff.Y;
            mEnd += ptDiff.Y;
            clipping();
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// скроллированние данных
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void clipping()
        {
            int iCountItem = onItemCount();
            int iHeightItem = onItemHeight();
            int iHeightSpace = iHeightItem + mVerticalSpacing;

            int iHeightClient = height; //ширина контрола
            int iHeightContent = iCountItem * iHeightSpace - mVerticalSpacing + cMargin + cMargin;
            
       
            if (mBegin < 0)
            {
                mBegin = 0;
                mEnd = iHeightContent;
            }

            if (mEnd > iHeightContent)
            {
                mEnd = iHeightContent;
                mBegin = mEnd - iHeightClient;
                if (mBegin < 0)
                {
                    mBegin = 0;
                }
            }

            if (mEnd - mBegin > iHeightClient)
            {
                mEnd = mBegin + iHeightClient;
            }
      

            //обновление положения item
            //отрисова item
            if (iHeightSpace == 0)
            {
                iHeightSpace = 1;
            }

            int iBegin = (int)(mBegin / iHeightSpace);
            int iEnd = (int)(mEnd / iHeightSpace);
            Rectangle itemRect = new Rectangle(left, top + ((int)(mBegin) % iHeightSpace) + cMargin, width, iHeightItem);
            //Rectangle itemRect = new Rectangle(left, top + (int)mBegin + cMargin, width, iHeightItem);
            for (int i = iBegin; i <= iEnd; i++)
            //for (int i = 0; i < iCountItem; i++)
            {
                if (i >= 0 && i < iCountItem)
                {
                    onUpdatePositionItem(itemRect, i);
                }
                itemRect.Y += iHeightSpace;
            }
            //

        }
        ///--------------------------------------------------------------------------------------







        ///=====================================================================================
        ///
        /// <summary>
        /// изменение размеров одного элемента
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected virtual void onUpdatePositionItem(Rectangle rectItem, int index)
        {

        }
        ///--------------------------------------------------------------------------------------







        ///=====================================================================================
        ///
        /// <summary>
        /// пересборка
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected void refresh()
        {
            mBegin = 0;
            mEnd = onItemCount() * (onItemHeight() + mVerticalSpacing) - mVerticalSpacing;

            int iCountItem = onItemCount();
            int iHeightItem = onItemHeight();
            int iHeightSpace = iHeightItem + mVerticalSpacing;

            int iHeightClient = height; //ширина контрола
            int iHeightContent = iCountItem * iHeightSpace - mVerticalSpacing + cMargin + cMargin;

            mEnd = iHeightContent;

            /*
            if (mEnd > iHeightContent)
            {
                mEnd = iHeightContent;
                mBegin = mEnd - iHeightClient;
                if (mBegin < 0)
                {
                    mBegin = 0;
                }
            }
            */

            if (mEnd > iHeightClient)
            {
                mEnd = iHeightClient;
            }
             
             
            scrolling(Point.Zero);
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// скроллируем в центр с указанным индексом
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected void scrollCenter(int index)
        {
            refresh();
            scrolling(new Point(0, index * (onItemHeight() + mVerticalSpacing) - mVerticalSpacing));

        }
        ///--------------------------------------------------------------------------------------





        ///=====================================================================================
        ///
        /// <summary>
        /// покажем текущий индекс который находится на экране
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected void visibleIndex(ref int indexBegin, ref int indexEnd)
        {
            int iCountItem = onItemCount();
            int iHeightItem = onItemHeight();
            if (iHeightItem == 0)
            {
                iHeightItem = 1;
            }
            int iHeightSpace = iHeightItem + mVerticalSpacing;

            indexBegin = (int)(mBegin / iHeightSpace);
            indexEnd = (int)(mEnd / iHeightSpace);
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// скроллируем в центр с указанным индексом
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected void scrollCenterMove(int index)
        {
            int itemHeight = onItemHeight();
            mMove = true;
            mMoveStart = mBegin;
            mMoveStop = index * (itemHeight + mVerticalSpacing) - mVerticalSpacing - itemHeight / 2;
            mMoveAnimation.setHide();
            mMoveAnimation.show();
        }
        ///--------------------------------------------------------------------------------------




        ///=====================================================================================
        ///
        /// <summary>
        /// Обновление логики у контрола
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onUpdate(TimeSpan gameTime)
        {
            if (mMove)
            {
                mMoveAnimation.update(gameTime);
                float fd = mMoveAnimation;

                mBegin = (mMoveStop - mMoveStart) * fd + mMoveStart;
                mEnd = mBegin + width;
                clipping();

                if (mMoveAnimation.isShow())
                {
                    mMove = false;
                }
            }
        }
        ///--------------------------------------------------------------------------------------





    }
}
