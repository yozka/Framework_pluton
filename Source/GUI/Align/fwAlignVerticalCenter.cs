#region Using framework
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
    /// Штуковина, которая вертикально располаживает виджеты
    /// </summary>
    /// 
    ///------------------------------------------------------------------------------------------
    public class AAlignVerticalCenter
            :
                AAlignFrame
    {

        ///--------------------------------------------------------------------------------------
        private readonly bool mResizeWidth = false;
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор 1
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AAlignVerticalCenter(AFrame frame)
            :
                base(frame)
        {

        }
        ///--------------------------------------------------------------------------------------





        ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор 2
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AAlignVerticalCenter(AFrame frame, bool resizeWidth)
            :
                base(frame)
        {
            mResizeWidth = resizeWidth;
        }
        ///--------------------------------------------------------------------------------------







        ///=====================================================================================
        ///
        /// <summary>
        /// пересборка содержимого фрейма
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onResize()
        {
            //сначала выесним общий размер всех контролов
            int iHeightAll = 0;
            int iCount = 0;
            foreach (AWidget obj in frame.childs)
            {
                if (obj is ADockwidgetButton)
                {
                    continue;
                }
                iHeightAll += obj.height;
                iCount++;
            }

            if (iCount == 0)
            {
                return;
            }

            //пробежимся по всем системным кнопкам, и выставим им позицию по умолчанию
            int iWidth = frame.contentWidth;
            int iTop = 0;
            int iHeight = frame.contentHeight / iCount;

            foreach (AWidget obj in frame.childs)
            {
                if (obj is ADockwidgetButton)
                {
                    continue;
                }

                if (mResizeWidth)
                {
                    obj.left = 0;
                    obj.width = iWidth;
                }
                else
                {
                    obj.left = (iWidth - obj.width) / 2;
                }


                obj.top = iTop;

                int hg = Math.Max(iHeight, obj.height);
                iTop += hg;
            }
        }
        ///--------------------------------------------------------------------------------------














    }
}
