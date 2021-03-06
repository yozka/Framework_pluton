﻿#region Using framework
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion




namespace Pluton.GUI
{
    ///------------------------------------------------------------------------------------------

    ///------------------------------------------------------------------------------------------






    ///=========================================================================================
    ///
    /// <summary>
    /// модификатор, изменяет рсположение элементов в виджетах
    /// расставляет их, или изменяет их размеры
    /// </summary>
    /// 
    ///------------------------------------------------------------------------------------------
    public class AAlignFrame
    {

        ///--------------------------------------------------------------------------------------
        protected AFrame frame = null; //текущий фрейм
        ///--------------------------------------------------------------------------------------







        ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AAlignFrame(AFrame frame)
        {
            this.frame = frame;
        }
        ///--------------------------------------------------------------------------------------








        ///=====================================================================================
        ///
        /// <summary>
        /// пересборка содержимого фрейма
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void resize()
        {
            onResize();
        }
        ///--------------------------------------------------------------------------------------







        ///=====================================================================================
        ///
        /// <summary>
        /// пересборка содержимого фрейма
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected virtual void onResize()
        {

        }
        ///--------------------------------------------------------------------------------------








        ///=====================================================================================
        ///
        /// <summary>
        /// пересборка содержимого фрейма
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public Rectangle boundingRect()
        {
            //пробежимся по всем системным кнопкам, и выставим им позицию по умолчанию
            int iLeft = 0;
            int iTop = 0;
            int iRight = 0;
            int iBottom = 0;
            bool first = true;

            foreach (AWidget obj in frame.childs)
            {
                if (obj is ADockwidgetButton)
                {
                    continue;
                }
                
                if (first)
                {
                    iLeft = obj.left;
                    iTop = obj.top;
                    iRight = obj.right;
                    iBottom = obj.bottom;
                    first = false;
                    continue;
                }

                iLeft = Math.Min(iLeft, obj.left);
                iTop = Math.Min(iTop, obj.top);
                iRight = Math.Max(iRight, obj.right);
                iBottom = Math.Max(iBottom, obj.bottom);
            }

            return first ? Rectangle.Empty : new Rectangle(iLeft, iTop, iRight - iLeft, iBottom - iTop);
        }
        ///--------------------------------------------------------------------------------------











    }
}
