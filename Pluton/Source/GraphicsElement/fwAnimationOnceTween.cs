﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


using Pluton.SystemProgram;


namespace Pluton.GraphicsElement
{
    ///--------------------------------------------------------------------------------------







     ///=====================================================================================
    ///
    /// <summary>
    /// Энимационный элемент
    /// проигрывает одну анимацию - туда от 0 до 1
    /// 
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AAnimationOnceTween
    {
        ///--------------------------------------------------------------------------------------
        private tweeningFunction    mTween  = null;
        private float               mSpeed  = 0;  //скорость перемещения
        private float               mDiff   = 0;  //текущее изменение от 0 до 1;
        private ETypeState          mState  = ETypeState.stop;
        private float               mValue = 0.0f;
        ///--------------------------------------------------------------------------------------




        ///--------------------------------------------------------------------------------------
        /// <summary>
        /// Текущее деййствия состояние машины
        /// </summary>
        protected enum ETypeState
        {
            stop,

            /// <summary>
            /// анимируем в одну сторону
            /// </summary>
            forward
        };
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Constructor
        /// speed - скорость продолжительности анимации в милисикундах
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AAnimationOnceTween(float speed, tweeningFunction tween)
        {
            mTween = tween;
            mSpeed = 1.0f / speed;
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// возвратить  текущее значение анимации от 0..1
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public static implicit operator float(AAnimationOnceTween p)
        {
            return p.mValue;
        }
        ///--------------------------------------------------------------------------------------










         ///=====================================================================================
        ///
        /// <summary>
        /// обработка анимации
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void update(TimeSpan gameTime)
        {
            switch (mState)
            {
                case ETypeState.forward:
                    {
                        mDiff += mSpeed * (float)gameTime.TotalMilliseconds;
                        if (mDiff > 1)
                        {
                            mDiff = 1;
                            mState = ETypeState.stop;
                        }
                        mValue = mTween(mDiff, 0, 1, 1);
                        break;
                    }
            }
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// начало старт анимации
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void startOnce()
        {
            mDiff = 0;
            mState = ETypeState.forward;
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// начало старт анимации
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void startOnce(float speed)
        {
            mSpeed = 1.0f / speed;
            mDiff = 0;
            mState = ETypeState.forward;
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// начало старт анимации
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void reset()
        {
            mDiff = 0;
            mState = ETypeState.stop;
        }
        ///--------------------------------------------------------------------------------------




        ///=====================================================================================
        ///
        /// <summary>
        /// проверка, анимация остановилась илил нет
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isStop()
        {
            return mState == ETypeState.stop ? true : false;
        }






    }//AAnimationLoopOnce
}