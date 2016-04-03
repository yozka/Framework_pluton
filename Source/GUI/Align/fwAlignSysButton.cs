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
    /// распологаем системные кнопки
    /// </summary>
    /// 
    ///------------------------------------------------------------------------------------------
    public class AAlignSysButton
            :
                AAlignFrame
    {
        ///--------------------------------------------------------------------------------------







        ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AAlignSysButton(AFrame frame)
            :
                base(frame)
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
        protected override void onResize()
        {
            //пробежимся по всем системным кнопкам, и выставим им позицию по умолчанию

            //подсчитаем количество кнопок
            int iCount = 0;
            foreach (AWidget obj in frame.childs)
            {
                if (obj is ADockwidgetButton)
                {
                    iCount++;
                }

                if (obj is ADockwidgetSysButton)
                {
                    obj.top = ATheme.dockwidget_sysTop;
                    obj.left = frame.contentWidth + ATheme.dockwidget_sysLeft;
                }
            }

            if (iCount == 0)
            {
                return;
            }


            int iWidth = frame.contentWidth - (iCount == 2 ? 160 : 130);


            int iw = 0;
            int ix = frame.contentWidth / 2 - ADockwidgetButton.cWidth / 2;
            if (iCount > 1)
            {
                int ic = iCount - 1;
                iw = iWidth / ic;
                ix = (frame.contentWidth  - (iw * ic)) / 2 - ADockwidgetButton.cWidth / 2;
            }

            int iy = frame.height - ADockwidgetButton.cHeight + 15;

            bool wave = false;
            bool waveDown = false;
            int waveY = 0;
            if (iCount > 2)
            {
                wave = true;
            }

            foreach (AWidget obj in frame.childs)
            {
                if (obj is ADockwidgetButton)
                {
                    if (wave)
                    {
                        waveY = waveDown ? 20 : 0;
                        waveDown = !waveDown;
                    }

                    obj.left = ix;
                    obj.top = iy + waveY;
                    ix += iw;
                }


            }
        }
        ///--------------------------------------------------------------------------------------















    }
}
