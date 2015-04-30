using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathieuJaumain.Tools.Math2D
{
    public class Ray2D
    {
        private Point2D A;
        private Point2D B;

        public Ray2D(Point2D a, Point2D b)
        {
            A = a;
            B = b;
        }

        public Ray2D(Point2D a, Vector2D b) : this(a, a + b) { }

        public bool IsRayIntersectingOnXAxis(double Yray, out Point2D intersection)
        {
            if (Yray < Math.Max(A.Y, B.Y) && Yray > Math.Min(A.Y, B.Y))
            {
                if(A.X != B.X) // pour eviter /0
                {
                    double coef = (B.Y - A.Y) / (B.X - A.X);
                    if (coef != 0)
                    {
                        intersection = new Point2D(((Yray - A.Y) / coef) + A.X, Yray); /// Yinter - YA = coef * (Xinter - XA)
                        return true;
                    }
                    else
                    {
                        // les deux rayons sont parallèles
                        intersection = new Point2D(double.NaN, double.NaN);
                        return false;
                    }
                }
                intersection = new Point2D(A.X, Yray);
                return true;
            }
            intersection = new Point2D(double.NaN, double.NaN);
            return false;
        }
    }
}
