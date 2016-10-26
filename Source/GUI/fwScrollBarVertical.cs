﻿#region Using framework
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
        private EScrollAlign mAlignH = EScrollAlign.none; //выравнивание по горизонтали
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
            area.setMargin(0, 0, 0, 0);
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


            //spriteBatch.Draw(spriteBatch.getSprite(sprite.gui_scroll_horizontal_margin), new Rectangle(iRightIcon, rect.Top, 16, rect.Height), Color.White);
            //


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
            float top = widget.top;
            if (widget.bottom < area.contentHeight)
            {
                top = area.contentHeight - widget.height;
            }

            if (top > 0)
            {
                top = 0;
            }

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
        public override Vector2 onScrollTouch(Vector2 ptWidget, Vector2 ptTouch)
        {
            return ptWidget + new Vector2(0, ptTouch.Y);
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
        /// пересборка
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        /*
        protected void refresh()
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
