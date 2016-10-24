#region Using framework
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion



//старый

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
    /// контрл горизонтально скроллинга
    /// 
    /// </summary>
    /// 
    ///------------------------------------------------------------------------------------------
    public class AScrollHorizontal
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

        private int     mHorizontalSpacing = 0; //отступы между горизонтальными элементами
        ///--------------------------------------------------------------------------------------

        private bool mMove = false; //движение скролинга
        private float mMoveStart = 0;
        private float mMoveStop = 0;
        private AAnimationShowTween mMoveAnimation = new AAnimationShowTween(400, tweener.quadratic.easeInOut);
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AScrollHorizontal(AWidget parent, int left, int top, int width, int height)
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
        public int horizontalSpacing
        {
            get
            {
                return mHorizontalSpacing;
            }
            set
            {
                mHorizontalSpacing = value;
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
        /// возвратим ширину элемента
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected virtual int onItemWidth()
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
            int iWidthItem = onItemWidth();
            if (iWidthItem == 0)
            {
                iWidthItem = 1;
            }
            int iWidthSpace = iWidthItem + mHorizontalSpacing;

            int iBegin = (int)(mBegin / iWidthSpace);
            int iEnd = (int)(mEnd / iWidthSpace);
            Rectangle itemRect = new Rectangle((int)(rect.Left - mBegin) % iWidthSpace + cMargin, rect.Top, iWidthItem, rect.Height);
            for (int i = iBegin; i <= iEnd; i++)
            {
                if (i >= 0 && i < iCountItem)
                {
                    onRenderItem(spriteBatch, itemRect, i);
                }
                itemRect.X += iWidthSpace;
            }
            //

            spriteBatch.endClipping();

            
            //отрисовка плашек
            //левая плашка //sprite.gui_scroll_horizontal_margin
            spriteBatch.Draw(spriteBatch.getSprite(ATheme.scrollHorizontal_marginID), new Rectangle(rect.Left - 8, rect.Top, 16, rect.Height), Color.White);
            //


            //правая плашка //sprite.gui_scroll_horizontal_margin
            int iRightIcon = rect.Right - 8;
            spriteBatch.Draw(spriteBatch.getSprite(ATheme.scrollHorizontal_marginID), new Rectangle(iRightIcon, rect.Top, 16, rect.Height), Color.White);
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
                    int iItemWidth = onItemWidth() + mHorizontalSpacing;

                    int id = (pt.X + (int)mBegin - screenLeft) / (iItemWidth == 0 ? 1 : iItemWidth);
                    mInputItem = (id >= 0 && id < onItemCount()) ? onHandleInputItem(input, id) : false;
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
                Point ptDiff = pointExt.sub(mToch, pt);

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
            mBegin += ptDiff.X;
            mEnd += ptDiff.X;
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
            int iWidthItem = onItemWidth();
            int iWidthSpace = iWidthItem + mHorizontalSpacing;

            int iWidthClient = width; //ширина контрола
            int iWidthContent = iCountItem * iWidthSpace - mHorizontalSpacing + cMargin + cMargin;
            if (mBegin < 0)
            {
                mBegin = 0;
                mEnd = iWidthContent;
            }

            if (mEnd > iWidthContent)
            {
                mEnd = iWidthContent;
                mBegin = mEnd - iWidthClient;
                if (mBegin < 0)
                {
                    mBegin = 0;
                }
            }

            if (mEnd - mBegin > iWidthClient)
            {
                mEnd = mBegin + iWidthClient;
            }


            //обновление положения item
            //отрисова item
            if (iWidthSpace == 0)
            {
                iWidthSpace = 1;
            }

            int iBegin = (int)(mBegin / iWidthSpace);
            int iEnd = (int)(mEnd / iWidthSpace);
            Rectangle itemRect = new Rectangle((int)(0 - mBegin) % iWidthSpace + cMargin, 0, iWidthItem, height);
            for (int i = iBegin; i <= iEnd; i++)
            {
                if (i >= 0 && i < iCountItem)
                {
                    onUpdatePositionItem(itemRect, i);
                }
                itemRect.X += iWidthSpace;
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
            mEnd = onItemCount() * (onItemWidth() + mHorizontalSpacing) - mHorizontalSpacing;

            int iWidthClient = width; //ширина контрола
            if (mEnd > iWidthClient)
            {
                mEnd = iWidthClient;
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
            scrolling(new Point(index * (onItemWidth() + mHorizontalSpacing) - mHorizontalSpacing, 0));

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
            int iWidthItem = onItemWidth();
            if (iWidthItem == 0)
            {
                iWidthItem = 1;
            }
            int iWidthSpace = iWidthItem + mHorizontalSpacing;

            indexBegin = (int)(mBegin / iWidthSpace);
            indexEnd = (int)(mEnd / iWidthSpace);
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
            int itemWidth = onItemWidth();
            mMove = true;
            mMoveStart = mBegin;
            mMoveStop = index * (itemWidth + mHorizontalSpacing) - mHorizontalSpacing - itemWidth / 2;
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
