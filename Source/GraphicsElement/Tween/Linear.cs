using System;
using System.Collections.Generic;
using System.Text;

namespace tweener
{
    public static class linear
    {
        public static float easeNone(float t, float b, float c, float d)
        {
            return c * t / d + b;
        }

        public static float easeIn(float t, float b, float c, float d)
        {
            return c * t / d + b;
        }

        public static float easeOut(float t, float b, float c, float d)
        {
            return c * t / d + b;
        }

        public static float easeInOut(float t, float b, float c, float d)
        {
            return c * t / d + b;
	    }
    }
}
