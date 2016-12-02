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
    /// выравнивание контента
    /// </summary>
    /// 
    ///------------------------------------------------------------------------------------------
    public enum EScrollAlign
    {
        none,
        left,
        right,
        center
    }
    ///------------------------------------------------------------------------------------------






     ///=========================================================================================
    ///
    /// <summary>
    /// полоска прокрутки
    /// 
    /// </summary>
    /// 
    ///------------------------------------------------------------------------------------------
    public class AScrollBarVertical
        :
            AScrollBar
    {

        ///--------------------------------------------------------------------------------------
        private EScrollAlign mAlignH    = EScrollAlign.none; //выравнивание по горизонтали
        private bool mRenderTop         = false;
        private bool mRenderBottom      = false;

        private readonly AAnimationShow mAnimTop       = new AAnimationShow(300);
        private readonly AAnimationShow mAnimBottom    = new AAnimationShow(300);
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AScrollBarVertical(EScrollAlign alignHorizontal)
        {
            setAlignHorizontal(alignHorizontal);
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AScrollBarVertical()
            :
            this(EScrollAlign.none)
        {
  
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// установим отступ и главного виджета
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onMargin(AScrollArea area)
        {
            area.setMargin(
                            ATheme.scrollBarVertical_marginLeft,
                            ATheme.scrollBarVertical_marginRight,
                            ATheme.scrollBarVertical_marginTop,
                            ATheme.scrollBarVertical_marginBottom);
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// изменение позиции виджета скроллинга
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onPositionWidget(AScrollArea area)
        {
            var widget = area.contentWidget;
            if (widget == null)
            {
                return;
            }

            //выравнивание окнтента по вертикали
            switch (mAlignH)
            {
                case EScrollAlign.none: break;
                case EScrollAlign.left:
                    {
                        widget.left = 0;
                        break;
                    }
                case EScrollAlign.right:
                    {
                        widget.left = area.contentWidth - widget.width;
                        break;
                    }
                case EScrollAlign.center:
                    {
                        widget.left = (area.contentWidth - widget.width) / 2;
                        break;
                    }
            }
            //

        }
        ///--------------------------------------------------------------------------------------







        ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка контрола с учетом располжения родителя
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onRender(AScrollArea area, ASpriteBatch spriteBatch)
        {
            if (ATheme.scrollBarVertical_marginID == 0)
            {
                return;
            }


            const int cImgHeight = ATheme.scrollBarVertical_imgHeight;

            
            float alpha     = area.alpha;
            int scrLeft     = area.screenLeft;
            int scrTop      = area.screenTop;
            int scrWidth    = area.screenWidth;
            int scrHeight   = area.screenHeight;


            if (mRenderTop)
            {
                mAnimTop.update(spriteBatch.gameTime);
                float anim = mAnimTop;
                if (mAnimTop.isHide())
                {
                    mRenderTop = false;
                }
                
                spriteBatch.Draw(spriteBatch.getSprite(ATheme.scrollBarVertical_marginID),
                                    new Rectangle(scrLeft, scrTop, scrWidth, cImgHeight),
                                    ATheme.scrollBarVertical_imgTop,
                                    Color.White * alpha * anim, 0.0f, Vector2.Zero, SpriteEffects.None, 0.5f);


            }


            if (mRenderBottom)
            {
                mAnimBottom.update(spriteBatch.gameTime);
                float anim = mAnimBottom;
                if (mAnimBottom.isHide())
                {
                    mRenderBottom = false;
                }

                spriteBatch.Draw(spriteBatch.getSprite(ATheme.scrollBarVertical_marginID),
                                    new Rectangle(scrLeft, scrTop + scrHeight - cImgHeight, scrWidth, cImgHeight),
                                    ATheme.scrollBarVertical_imgBottom,
                                    Color.White * alpha * anim, 0.0f, Vector2.Zero, SpriteEffects.None, 0.5f);
            }


            //spriteBatch.primitives.drawBorder(rect, 2, Color.Red);

        }
        ///--------------------------------------------------------------------------------------




        


         ///=====================================================================================
        ///
        /// <summary>
        /// выравнивание контента по горизонтали
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void setAlignHorizontal(EScrollAlign align)
        {
            mAlignH = align;
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// вохвратим координаты для скроллинга
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override Vector2 onCorrectPosition(AScrollArea area)
        {
            var widget = area.contentWidget;
            int top = widget.top;
            if (widget.bottom < area.contentHeight)
            {
                top = area.contentHeight - widget.height;
            }

            if (top >= 0)
            {
                top = 0;
                showRenderTop(false);
            }
            else
            {
                showRenderTop(true);
            }


            int tt = top + widget.height;
            showRenderBottom(tt <= area.contentHeight ? false : true);


            return new Vector2(widget.left, top);
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// вохвратим координаты для скроллинга
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override Vector2 onScrollTouch(AScrollArea area, Vector2 ptWidget, Vector2 ptTouch)
        {
            Vector2 pt = ptWidget + new Vector2(0, ptTouch.Y);

            showRenderTop(pt.Y < 0.0f ? true : false);
            showRenderBottom((pt.Y + area.contentWidget.height) <= area.contentHeight ? false : true);
            return pt;
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// обработка нажатий, если обработка удачная то возвращаем true
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override bool onHandleInput(AScrollArea area, AInputDevice input)
        {
            return false;
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Обновление логики у контрола
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onUpdate(AScrollArea area, TimeSpan gameTime)
        {

        }
        ///--------------------------------------------------------------------------------------









         ///=====================================================================================
        ///
        /// <summary>
        /// покажем или скроем верхнию деку
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void showRenderTop(bool enabled)
        {
            if (!enabled)
            {
                if (mRenderTop)
                {
                    mAnimTop.hide();
                }
                return;
            }

            mRenderTop = true;
            mAnimTop.show();
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// покажем или скроем нижнию деку
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void showRenderBottom(bool enabled)
        {
            if (!enabled)
            {
                if (mRenderBottom)
                {
                    mAnimBottom.hide();
                }
                return;
            }

            mRenderBottom = true;
            mAnimBottom.show();
        }
        ///--------------------------------------------------------------------------------------










        ///=====================================================================================
        ///
        /// <summary>
        /// скроллируем в центр с указанным индексом
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        /*
        protected void scrollCenter(int index)
        {
     
        }*/
        ///--------------------------------------------------------------------------------------





        ///=====================================================================================
        ///
        /// <summary>
        /// покажем текущий индекс который находится на экране
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        /*
        protected void visibleIndex(ref int indexBegin, ref int indexEnd)
        {
           
        }*/
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// скроллируем в центр с указанным индексом
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        /*
        protected void scrollCenterMove(int index)
        {

        }*/
        ///--------------------------------------------------------------------------------------








    }
}
