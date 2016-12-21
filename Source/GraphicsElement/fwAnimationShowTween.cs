﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;




namespace Pluton.GraphicsElement
{
    ///--------------------------------------------------------------------------------------
    using Pluton.SystemProgram;
    ///--------------------------------------------------------------------------------------




    ///--------------------------------------------------------------------------------------
    public delegate float tweeningFunction(float timeElapsed, float start, float change, float duration);
    ///--------------------------------------------------------------------------------------








    ///=====================================================================================
    ///
    /// <summary>
    /// Энимационный элемент
    /// показывает или скрывает анимацию
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AAnimationShowTween
    {
        ///--------------------------------------------------------------------------------------
        private tweeningFunction    mTween  = null;
        private float               mSpeed  = 0;   //скорость перемещения
        private float               mDiff   = 0;  //текущее изменение от 0 до 1;
        private ETypeState          mState  = ETypeState.hide;
        private float               mValue  = 0.0f;
        ///--------------------------------------------------------------------------------------




        ///--------------------------------------------------------------------------------------
        /// <summary>
        /// Текущее деййствия состояние машины
        /// </summary>
        protected enum ETypeState
        {
            /// <summary>
            /// скрыто
            /// </summary>
            hide,

            /// <summary>
            /// скрывает и анимирует 
            /// </summary>
            hideAnimat,


            /// <summary>
            /// активно показывает
            /// </summary>
            show,

            /// <summary>
            /// анимация при показе
            /// </summary>
            showAnimat

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
        public AAnimationShowTween(float speed, tweeningFunction tween)
        {
            mTween = tween;
            mSpeed = 1 / speed;
        }
        ///--------------------------------------------------------------------------------------







        ///=====================================================================================
        ///
        /// <summary>
        /// возвратить  текущее значение анимации от 0..1
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public static implicit operator float(AAnimationShowTween p)
        {
            return p.mValue;
        }
        ///--------------------------------------------------------------------------------------








        ///=====================================================================================
        ///
        /// <summary>
        /// проверка на том что анимация закончена 
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isHide()
        {
            return mState == ETypeState.hide ? true : false;
        }
        ///--------------------------------------------------------------------------------------








        ///=====================================================================================
        ///
        /// <summary>
        /// анимация завершена показам анимаированного элемента
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isShow()
        {
            return mState == ETypeState.show ? true : false;
        }
        ///--------------------------------------------------------------------------------------





        ///=====================================================================================
        ///
        /// <summary>
        /// првоерка, идет ли анимация показа элемента
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isShowing()
        {
            return mState == ETypeState.showAnimat ? true : false;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// првоерка, идет ли анимация показа элемента
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isActive()
        {
            return mState == ETypeState.showAnimat || mState == ETypeState.hideAnimat ? true : false;
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// начало показа анимации
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void show()
        {
            mState = ETypeState.showAnimat;
        }
        ///--------------------------------------------------------------------------------------




    

         ///=====================================================================================
        ///
        /// <summary>
        /// начало показа анимации
        /// speed - скорость продолжительности анимации в милисикундах
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void show(float speed)
        {
            mState = ETypeState.showAnimat;
            mSpeed = 1 / speed;
        }
        ///--------------------------------------------------------------------------------------









        ///=====================================================================================
        ///
        /// <summary>
        /// скрыть анимацию
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void hide()
        {
            mState = ETypeState.hideAnimat;
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// скрыть анимацию
        /// speed - скорость продолжительности анимации в милисикундах
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void hide(float speed)
        {
            mState = ETypeState.hideAnimat;
            mSpeed = 1 / speed;
        }
        ///--------------------------------------------------------------------------------------





        ///=====================================================================================
        ///
        /// <summary>
        /// скрыть анимацию
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void hide(float speed, tweeningFunction tween)
        {
            mState = ETypeState.hideAnimat;
            mTween = tween;
            mSpeed = 1 / speed;
        }
        ///--------------------------------------------------------------------------------------





        ///=====================================================================================
        ///
        /// <summary>
        /// проверка что идет анимация скрытости
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isHiding()
        {
            return mState == ETypeState.hideAnimat ? true : false;
        }
        ///--------------------------------------------------------------------------------------







        ///=====================================================================================
        ///
        /// <summary>
        /// скрыть анимацию немедленно
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void setHide()
        {
            mDiff = 0;
            mState = ETypeState.hide;
        }
        ///--------------------------------------------------------------------------------------







        ///=====================================================================================
        ///
        /// <summary>
        /// открыть анимацию немедленно
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void setShow()
        {
            mDiff = 1;
            mState = ETypeState.show;
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
                case ETypeState.showAnimat:
                    {
                        mDiff += mSpeed * (float)gameTime.TotalMilliseconds;
                        if (mDiff > 1)
                        {
                            mDiff = 1;
                            mState = ETypeState.show;
                        }

                        mValue = mTween(mDiff, 0, 1, 1);
                        break;
                    }

                case ETypeState.hideAnimat:
                    {
                        mDiff -= mSpeed * (float)gameTime.TotalMilliseconds;
                        if (mDiff < 0)
                        {
                            mDiff = 0;
                            mState = ETypeState.hide;
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
        /// возвратим занчение не твиванное
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public float valueSource
        {
            get
            {
                return mDiff;
            }
        }




    }//AAnimationShowTween
}