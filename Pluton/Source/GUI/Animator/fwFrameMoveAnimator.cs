#region Using framework
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion




namespace Pluton.GUI
{
    ///------------------------------------------------------------------------------------------
    using Pluton;
    using Pluton.SystemProgram;
    using Pluton.SystemProgram.Devices;
    using Pluton.GraphicsElement;
    using tweener;




     ///=========================================================================================
    ///
    /// <summary>
    /// Анимация фрейма
    /// 
    /// </summary>
    /// 
    ///------------------------------------------------------------------------------------------
    public class AFrameMoveAnimator
    {

        ///--------------------------------------------------------------------------------------
        private readonly ATweener           mTween = null;
        private readonly List<AWidget>      mWidgets = new List<AWidget>();
        private readonly Dictionary<AWidget, Vector2> mPosition = new Dictionary<AWidget, Vector2>();

        private Vector2 mDirect = Vector2.Zero;
        private float mBegin = 0;
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор 0
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AFrameMoveAnimator(ATweener tween)
        {
            mTween = tween;
            mTween.signal_ended += slot_ended;
            mBegin = mTween.position;
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
            if (mTween.update(gameTime))
            {
                float anim = mTween.position;
                Vector2 pos = mDirect * (1.0f - anim);

                foreach (var obj in mWidgets)
                {
                    var pt = mPosition[obj] + pos;
                    obj.setPosition((int)pt.X, (int)pt.Y);
                    obj.alpha = anim;
                }

            }
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// запуск анимации
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void transition(List<AWidget> widgets, Vector2 direct)
        {
            mDirect = direct;

            foreach (var obj in widgets)
            {
                if (!mWidgets.Contains(obj))
                {
                    mWidgets.Add(obj);
                    mPosition[obj] = new Vector2(obj.left, obj.top);
                }
            }

            mTween.position = mBegin;
           
            mTween.reset();
            mTween.start();

            update(TimeSpan.Zero);
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// навигация по страницам вперед
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void slot_ended()
        {
            if (signal_transition != null)
            {
                signal_transition(mWidgets);
            }
            mWidgets.Clear();
            mPosition.Clear();
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// сигнал что закончилась анимация
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public delegate void eventTransition(List<AWidget> widgets);
        public event eventTransition signal_transition;
        ///--------------------------------------------------------------------------------------







    }
}
