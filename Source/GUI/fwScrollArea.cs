#region Using framework
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion




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
    public class AScrollArea
                :
                    AWidget
    {

        ///--------------------------------------------------------------------------------------
        private const int   cDynamicTimeWait        = 100; //время ожидания реакции на движение скролинга
                                                           //после этого времени, идет управление внутренними контралами

        ///--------------------------------------------------------------------------------------
        private AScrollBar  mScrollBar              = new AScrollBar();

        private AWidget     mContentWidget          = null; //сам виджет для отрисовки и показа
        private Rectangle   mViewPort               = Rectangle.Empty; //квадрат ограничивающий просмотр


        private bool        mScrollSkip             = false; //пропускать динамическое скроллинг

        private bool        mGrable                 = false; //идет режим захвата и движения скроллинга
        private Vector2     mFirstTouch             = Vector2.Zero; //первая стартовая кнопка нажатия
        private Vector2     mFirstWidget            = Vector2.Zero; //положение виджета старотовое
        
        private bool        mDynamics               = false; //динамическое двжиение скролинга
        private Vector2     mDynamicsTouch          = Vector2.Zero; //начальная точка соприкосновение
        private TimeSpan    mDynamicsTime           = TimeSpan.Zero;//время отчета для динамического листинга

        private bool        mHome                   = false;
        private Vector2     mHomePositionBegin      = Vector2.Zero;
        private Vector2     mHomePositionDirect     = Vector2.Zero;
        private AAnimationOnceTween mHomeAnimation  = new AAnimationOnceTween(1000, tweener.bounce.easeOut);
        
        private Vector2[]   mBoostPosition          = new Vector2[5];  //точки для подсчета ускорения   
        private bool        mBoost                  = false;
        private Vector2     mBoostPositionBegin     = Vector2.Zero;
        private Vector2     mBoostPositionDirect    = Vector2.Zero;
        private AAnimationOnceTween mBoostAnimation = new AAnimationOnceTween(500, tweener.exponential.easeOut);

        private int         mScrollWheel            = 0;
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AScrollArea(AWidget parent)
            :
            this(parent, 0, 0, 0, 0)
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
        public AScrollArea(AWidget parent, int left, int top, int width, int height)
            :
            base(parent, left, top, width, height)
        {

        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// установка виджета для скроллинга
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AWidget contentWidget
        {
            set
            {
                if (mContentWidget != null)
                {
                    mContentWidget.setParent(null);
                }
                mContentWidget = value;
                mContentWidget.setParent(this);
                updateViewPort();
                scrollToHome();
            }

            get
            {
                return mContentWidget;
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// возвратим высоту элемента
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AScrollBar scrollBar
        {
            set
            {
                mScrollBar = value;
                if (mScrollBar == null)
                {
                    mScrollBar = new AScrollBar();
                }
                updateViewPort();
                scrollToHome();
            }
            get
            {
                return mScrollBar;
            }

        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Пересчитаем всю позицию данных
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void updateViewPort()
        {
            //подсчитаем изменения для скроллинга
            mScrollBar.onMargin(this);
            mViewPort = screenContentRect;
            mScrollBar.onPositionWidget(this);
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
            updateViewPort();
            scrollToHome();
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
            if (mContentWidget != null)
            {
                spriteBatch.beginClipping(mViewPort);
                mContentWidget.render(spriteBatch);
                spriteBatch.endClipping();
            }

            mScrollBar.onRender(this, spriteBatch);
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
            if (mContentWidget == null)
            {
                mGrable = false;
                return false;
            }


            if (mScrollSkip && input.touchIndex() < 0)
            {
                mScrollSkip = false;
            }


            bool bEventInput = false;
            bool bScrollInput = mScrollSkip;
            bool bResetGrable = false;

            if (mGrable)
            {
                //скроллинг захватиил управление
                bScrollInput = true;
                bEventInput = scrollInput(input);
            }
            else
            {
                //проверяем захват в движении
                if (!mScrollSkip)
                {
                    bEventInput = scrollDynamic(input, ref bResetGrable);
                }
            }



            //передаем управление виджету
            if (!bEventInput)
            {
                //смотрим чтобы нажатие на кнопку не перешли границы
                //просомтра
                if (input.touchIndex() < 0 || input.containsRectangle(mViewPort) >= 0)
                {
                    bEventInput = mContentWidget.onHandleInput(input);
                    if (bEventInput)
                    {
                        mScrollSkip = true;
                    }
                }
            }
            //

            //далее идет пост скроллинг
            //если управелние свободное
            if (!bEventInput && !bScrollInput)
            {
                //обрабатываем скроллинг
                bEventInput = scrollInput(input);

            }

            if (bResetGrable)
            {
                mGrable = false;
            }


            //обработка колесика мышки
            if (!bEventInput)
            {
                bEventInput = wheel(input);
            }

            return bEventInput;
        }
        ///--------------------------------------------------------------------------------------





           
         ///=====================================================================================
        ///
        /// <summary>
        /// обработка скроллинга колесиком мышки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private bool wheel(AInputDevice input)
        {
            int wheel = input.wheelValue();
            int diff = wheel - mScrollWheel;
            mScrollWheel = wheel;

            if (diff == 0)
            {
                return false;
            }


            Vector2 pos = input.touchMouse();
            if (!mViewPort.Contains(pos))
            {
                return false;
            }


            Vector2 diffVector = new Vector2(0, diff * 0.2f);

            Vector2 posWidget = mContentWidget.leftTop.toVector2();
            Vector2 pt = mScrollBar.onScrollTouch(this, posWidget, diffVector);
            mContentWidget.leftTop = pt.toPoint();

            mDynamics = false;
            mBoost = false;
            mHome = false;

            scrollToHome();
            return true;
        }
        ///--------------------------------------------------------------------------------------




        
         ///=====================================================================================
        ///
        /// <summary>
        /// обработка скроллинга в динамике
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private bool scrollDynamic(AInputDevice input, ref bool resetGrable)
        {
            int index = input.containsRectangle(screenContentRect);
            if (index >= 0)
            {
                Vector2 pt = input.touch(index).toVector2();

                if (!mDynamics)
                {
                    mDynamics = true;
                    mDynamicsTouch = pt;
                    mDynamicsTime = TimeSpan.FromMilliseconds(cDynamicTimeWait);
                }


                Vector2 diff = pt - mDynamicsTouch;
                if (diff.Length() > 10)
                {
                    mDynamics = false;
                    scrollInput(input);
                    return true;
                }


                if (mDynamicsTime.TotalMilliseconds > 0)
                {
                    //пока обрабатываем движения
                    resetGrable = true; //но неделаем захвата
                    return true;
                }


            }
            mDynamics = false;
            return false;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// обработка скроллинга
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private bool scrollInput(AInputDevice input)
        {
            int index = input.containsRectangle(mViewPort);
            if (index >= 0)
            {
                //нажали в пределах виджета
                Vector2 pos = input.touch(index).toVector2();
                if (!mGrable)
                {
                    mGrable = true;
                    mFirstTouch = pos;
                    mFirstWidget = mContentWidget.leftTop.toVector2();
                }
                scrollTouch(pos);
                return true;
            }
            else
            {
                //отпустили нажатие
                //либо проверяем нажатие за пределы
                index = input.touchIndex();
                if (index >= 0 && mGrable)
                {
                    scrollTouch(input.touch(index).toVector2());
                    return true;
                }

                scrollStop();
            }
            return false;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// скролирование относительной позиции
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void scrollTouch(Vector2 posTouch)
        {
            Vector2 diff = posTouch - mFirstTouch;
            Vector2 pt = mScrollBar.onScrollTouch(this, mFirstWidget, diff);
            mContentWidget.leftTop = pt.toPoint();


            //добавим точки
            appendBoost(pt);
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// создание точек для ускорения
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void appendBoost(Vector2 ptBoost)
        {
            int len = mBoostPosition.Length;
            for (int i = len - 1; i > 0; i--)
            {
                mBoostPosition[i] = mBoostPosition[i - 1];
            }
            mBoostPosition[0] = ptBoost;
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// остановка скролинга
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void scrollStop()
        {
            mDynamics = false;
            if (mGrable)
            {
                mGrable = false;
                mHome = false;
                mBoost = false;
                
                scrollToBoost();
                //scrollToHome();
            }
        }
        ///--------------------------------------------------------------------------------------




        
         ///=====================================================================================
        ///
        /// <summary>
        /// ускорение, когда остановили движение
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void scrollToBoost()
        {
            //return;
            
            
            int count = mBoostPosition.Length;


            Vector2 va = mBoostPosition[0];
            if (va.isZero())
            {
                scrollToHome();
                return;
            }

            int id = count - 1;
            Vector2 vb = mBoostPosition[id];
            while (vb.isZero())
            {
                id--;
                if (id < 0)
                {
                    scrollToHome();
                    return;
                }
                vb = mBoostPosition[id];
            }

            Vector2 direct = va - vb;
            float len = direct.Length();
            if (va.isZero() || vb.isZero() || len < 5)
            {
                scrollToHome();
                return;
            }

            mBoostPositionBegin = mContentWidget.leftTop.toVector2();
            mBoostPositionDirect = direct * 2.0f;
            mBoost = true;

            float contentLen = contentSize.toVector2().Length();
            float time = (contentLen - mBoostPositionDirect.Length()) * 2.0f;

            mBoostAnimation.startOnce(MathHelper.Clamp(time, 500, 3000));

            for (int i = 0; i < count; i++)
            {
                mBoostPosition[i] = Vector2.Zero;
            }
        }
        ///--------------------------------------------------------------------------------------





        ///=====================================================================================
        ///
        /// <summary>
        /// начало движение виджета домой
        /// посе его остановки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void scrollToHome()
        {
            if (mContentWidget == null)
            {
                return;
            }
            Vector2 pt = mScrollBar.onCorrectPosition(this);

            mHomePositionBegin = mContentWidget.leftTop.toVector2();
            mHomePositionDirect = pt - mHomePositionBegin;
            if (mHomePositionDirect.isZero())
            {
                mHome = false;
                return;
            }
            mHome = true;
            mHomeAnimation.startOnce();
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
            if (mContentWidget != null)
            {
                mContentWidget.onUpdate(gameTime);
            }

            mScrollBar.onUpdate(this, gameTime);

            //таймер динамичного скролинга
            if (mDynamics)
            {
                mDynamicsTime -= gameTime;
            }
            
       

            //ускорение
            if (mBoost)
            {
                if (mGrable)
                {
                    mBoost = false;
                }
                else
                {
                    mBoostAnimation.update(gameTime);
                    Vector2 pt = mBoostPositionBegin + mBoostPositionDirect * mBoostAnimation;
                    mContentWidget.leftTop = pt.toPoint();
                    if (mBoostAnimation.isStop())
                    {
                        mBoost = false;
                        scrollToHome();
                    }
                }
            }
            //


            //переход в домашнию позицию
            if (mHome)
            {
                if (mGrable)
                {
                    mHome = false;
                }
                else
                {

                    mHomeAnimation.update(gameTime);
                    Vector2 pt = mHomePositionBegin + mHomePositionDirect * mHomeAnimation;
                    mContentWidget.leftTop = pt.toPoint();
                    if (mHomeAnimation.isStop())
                    {
                        mHome = false;
                    }
                }
            }
        }
        ///--------------------------------------------------------------------------------------



















    }
}
