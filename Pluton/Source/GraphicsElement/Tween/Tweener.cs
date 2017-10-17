using System;
using Microsoft.Xna.Framework;

namespace tweener
{
    public delegate float TweeningFunction(float timeElapsed, float start, float change, float duration);

    public class ATweener
    {
        public ATweener(float from, float to, float duration, TweeningFunction tweeningFunction)
        {
            mFrom = from;
            mPosition = from;
            mChange = to - from;
            mTweeningFunction = tweeningFunction;
            mDuration = duration;
        }


        #region Properties
        private float mPosition;
        public float position
        {
            get
            {
                return mPosition;
            }
            set
            {
                mPosition = value;
            }
        }

        private float mFrom;
        protected float from
        {
            get
            {
                return mFrom;
            }
            set
            {
                mFrom = value;
            }
        }

        private float mChange;
        protected float change
        {
            get
            {
                return mChange;
            }
            set
            {
                mChange = value;
            }
        }

        private float mDuration;
        protected float duration
        {
            get
            {
                return mDuration;
            }
        }

        private float mElapsed = 0.0f;
        protected float elapsed
        {
            get
            {
                return mElapsed;
            }
            set
            {
                mElapsed = value;
            }
        }

        private bool mRunning = true;
        public bool running
        {
            get { return mRunning; }
            protected set { mRunning = value; }
        }

        private TweeningFunction mTweeningFunction;
        protected TweeningFunction tweeningFunction
        {
            get
            {
                return mTweeningFunction;
            }
        }

        public delegate void endHandler();
        public event endHandler signal_ended;
        #endregion

        #region Methods
        public bool update(TimeSpan gameTime)
        {
            if (!running || (elapsed == duration))
            {
                return false;
            }
            position = tweeningFunction(elapsed, from, change, duration);
            elapsed += (float)gameTime.TotalMilliseconds;
            if (elapsed >= duration)
            {
                elapsed = duration;
                position = from + change;
                onEnd();
            }
            return true;
        }

        protected void onEnd()
        {
            if (signal_ended != null)
            {
                signal_ended();
            }
        }

        public void start()
        {
            running = true;
        }

        public void stop()
        {
            running = false;
        }

        public void reset()
        {
            elapsed = 0.0f;
            from = position;
        }

        public void reset(float to)
        {
            change = to - position;
            reset();
        }

        public void reverse()
        {
            elapsed = 0.0f;
            change = -change + (from + change - position);
            from = position;
        }

        #endregion
    }
}
