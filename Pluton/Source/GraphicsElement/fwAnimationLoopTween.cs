using System;
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
    /// проигрывает зацикленную анимацию
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AAnimationLoopTween
    {
        ///--------------------------------------------------------------------------------------
        private tweeningFunction        mTween  = null;
        private float                   mSpeed  = 0;    //скорость перемещения
        private float                   mDiff   = 0;    //текущее изменение от 0 до 1;
        private ETypeState              mState  = ETypeState.forward;
        private float                   mValue  = 0; 
        ///--------------------------------------------------------------------------------------




        ///--------------------------------------------------------------------------------------
        /// <summary>
        /// Текущее деййствия состояние машины
        /// </summary>
        protected enum ETypeState
        {
            /// <summary>
            /// анимируем в одну сторону
            /// </summary>
            forward,

            /// <summary>
            /// анимируем вдругую сторону
            /// </summary>
            backward

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
        public AAnimationLoopTween(float speed, tweeningFunction tween)
        {
            mSpeed = 1 / speed;
            mTween = tween;
        }
        ///--------------------------------------------------------------------------------------







        ///=====================================================================================
        ///
        /// <summary>
        /// возвратить  текущее значение анимации от 0..1
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public static implicit operator float(AAnimationLoopTween p)
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
                            mState = ETypeState.backward;
                        }
                        mValue = mTween(mDiff, 0, 1, 1);
                        break;
                    }

                case ETypeState.backward:
                    {
                        mDiff -= mSpeed * (float)gameTime.TotalMilliseconds;
                        if (mDiff < 0)
                        {
                            mDiff = 0;
                            mState = ETypeState.forward;
                        }
                        mValue = mTween(mDiff, 0, 1, 1);
                        break;
                    }
            }
        }
        ///--------------------------------------------------------------------------------------









    }//AAnimationLoop
}