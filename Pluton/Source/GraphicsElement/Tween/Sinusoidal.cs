using System;
using System.Collections.Generic;
using System.Text;

namespace tweener
{
    public static class sinusoidal
    {
        public static float easeIn(float t, float b, float c, float d)
        {
		    return -c * (float)Math.Cos(t/d * (Math.PI / 2)) + c + b;
	    }
        public static float easeOut(float t, float b, float c, float d)
        {
            return c * (float)Math.Sin(t / d * (Math.PI / 2)) + b;
	    }
        public static float easeInOut(float t, float b, float c, float d)
        {
            return -c / 2 * ((float)Math.Cos(Math.PI * t / d) - 1) + b;
	    }
    }
}
