#region Using framework
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
#endregion



namespace Pluton.Path
{
    ///------------------------------------------------------------------------------------------
    using Pluton;
    using Pluton.Collections;
    ///------------------------------------------------------------------------------------------







     ///=========================================================================================
    ///
    /// <summary>
    /// Система разбития пути и генерация плавного пути с учетом указанной размера сетки
    /// </summary>
    /// 
    ///------------------------------------------------------------------------------------------
    public class APathCurveGenerate
    {
        ///--------------------------------------------------------------------------------------
        /// public
        ///--------------------------------------------------------------------------------------


        ///--------------------------------------------------------------------------------------









         ///=====================================================================================
        ///
        /// <summary>
        /// растояние между двумя точками
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public static float distance(Point p0, Point p1)
        {
            Vector2 p = p0.toVector2() - p1.toVector2();
            return p.Length();
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Рассчитать контрольные точки для 'p1' точку, используя соседние точки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public static Point[] getControlsPoints(Point p0, Point p1, Point p2, float tension)
        {
            // get length of lines [p0-p1] and [p1-p2]
            float d01 = distance(p0, p1);
            float d12 = distance(p1, p2);
            // calculate scaling factors as fractions of total
            float sa = tension * d01 / (d01 + d12);
            float sb = tension * d12 / (d01 + d12);
            // left control point
            int c1x = (int)(p1.X - sa * (p2.X - p0.X));
            int c1y = (int)(p1.Y - sa * (p2.Y - p0.Y));
            // right control point
            int c2x = (int)(p1.X + sb * (p2.X - p0.X));
            int c2y = (int)(p1.Y + sb * (p2.Y - p0.Y));
            // return control points
            return new Point[] { new Point(c1x, c1y), new Point(c2x, c2y) };
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Генерация всех контрольных точек для набора узлов
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public static List<Point> generateControlPoints(List<Point> knots)
        {
            if (knots == null || knots.Count < 3)
                return null;
            List<Point> res = new List<Point>();
            // First control point is same as first knot
            res.Add(knots[0]);


            // generate control point pairs for each non-end knot 
            for (int i = 1; i < knots.Count - 1; ++i)
            {
                Point[] cps = getControlsPoints(knots[i - 1], knots[i], knots[i + 1], 0.5f);
                res.AddRange(cps);
            }
            
            
            // Last control points is same as last knot
            res.Add(knots[knots.Count - 1]);
            return res;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// линейная интерполяция между двумя точками
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public static Point linearInterp(Point p0, Point p1, float fraction)
        {
            int ix = (int)(p0.X + (p1.X - p0.X) * fraction);
            int iy = (int)(p0.Y + (p1.Y - p0.Y) * fraction);
            return new Point(ix, iy);
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// аналог CatmullRom
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public static Point bezierInterp(Point p0, Point p1, Point c0, Point c1, float fraction)
        {
            // calculate first-derivative, lines containing end-points for 2nd derivative
            var t00 = linearInterp(p0, c0, fraction);
            var t01 = linearInterp(c0, c1, fraction);
            var t02 = linearInterp(c1, p1, fraction);
            // calculate second-derivate, line tangent to curve
            var t10 = linearInterp(t00, t01, fraction);
            var t11 = linearInterp(t01, t02, fraction);
            // return third-derivate, point on curve
            return linearInterp(t10, t11, fraction);
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Cоздание несколько точек из сегмента кривой для всего пути
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public static List<Point> generateCurvePoints(List<Point> knots, List<Point> controls, int steps)
        {
            List<Point> res = new List<Point>();
            // start curve at first knot
            res.Add(knots[0]);
            // process each curve segment
            for (int i = 0; i < knots.Count - 1; ++i)
            {
                // get knot points for this curve segment
                Point p0 = knots[i];
                Point p1 = knots[i + 1];
                // get control points for this curve segment
                Point c0 = controls[i * 2];
                Point c1 = controls[i * 2 + 1];
                // calculate 20 points along curve segment
                //int steps = 10;
                for (int s = 1; s < steps; ++s)
                {
                    float fraction = (float)s / steps;
                    res.Add(bezierInterp(p0, p1, c0, c1, fraction));
                }
            }
            return res;
        }
        ///--------------------------------------------------------------------------------------





    }
}