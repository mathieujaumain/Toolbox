using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathieuJaumain.Tools.Math2D
{
    /// <summary>
    /// Classe representant un point dans un espace 2D
    /// </summary>
    public class Point2D
    {

        public static double ZERO_APPROX = 0.00001; //Approximation salvatrice

        public double X { get; set; }
        public double Y { get; set; }

         public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Point2D(Point2D v)
        {
            X = v.X;
            Y = v.Y;
        }

        public Point2D() : this(0,0) { }

        public Vector2D Add(Point2D v)
        {
            return new Vector2D(X + v.X, Y + v.Y);
        }

        public Point2D Add(Vector2D v)
        {
            return new Point2D(X + v.X, Y + v.Y);
        }

        public Vector2D Sub(Point2D v)
        {
            return new Vector2D(X - v.X, Y - v.Y);
        }

        public Point2D Sub(Vector2D v)
        {
            return new Point2D(X - v.X, Y - v.Y);
        }

        /// <summary>
        /// Longueur du vecteur.
        /// </summary>
        public double Lenght
        {
            get { return Math.Sqrt(X * X + Y * Y); }
        }

        public static Vector2D operator +(Point2D v, Point2D u)
        {
            return v.Add(u);
        }

        public static Vector2D operator -(Point2D v, Point2D u)
        {
            return v.Sub(u);
        }

        public static Point2D operator +(Point2D v, Vector2D u)
        {
            return v.Add(u);
        }

        public static Point2D operator -(Point2D v, Vector2D u)
        {
            return v.Sub(u);
        }

        public double Dot(Point2D v)
        {
            return X * v.X + Y * v.Y;
        }

        public double Dot(Vector2D v)
        {
            return X * v.X + Y * v.Y;
        }

        public double Det(Point2D v)
        {
            return (X * v.Y) - (Y * v.X);
        }

        public double Det(Vector2D v)
        {
            return (X * v.Y) - (Y * v.X);
        }

        public static double operator *(Point2D v, Point2D u)
        {
            return v.Dot(u);
        }

        public static double operator *(Point2D v, Vector2D u)
        {
            return v.Dot(u);
        }

        public static double operator *(Vector2D u, Point2D v)
        {
            return v.Dot(u);
        }

        public Point2D Multiply(double a)
        {
            return new Point2D(X * a, Y * a); 
        }

        public static Point2D operator *(double a, Point2D u)
        {
            return u.Multiply(a);
        }

        public static Point2D operator *(Point2D u, double a)
        {
            return u.Multiply(a);
        }

        /// <summary>
        /// Test la colinéarité de l'instance this à un autre vecteur point.
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public bool IsColinearTo(Point2D u)
        {
            return Math.Abs(Det(u)) < ZERO_APPROX;
        }

        /// <summary>
        /// Test la colinéarité de l'instance this à un vecteur u.
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public bool IsColinearTo(Vector2D u)
        {
            return Math.Abs(Det(u)) < ZERO_APPROX;
        }

        public override bool Equals(object obj)
        {
            if (obj is Point2D)
            {
                Point2D v = obj as Point2D;
                if ((this - v).Lenght < ZERO_APPROX)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determine l'angle orienté entre ces deux point avec le point (0,0) pour origine. 
        /// </summary>
        /// <param name="v"></param>
        /// <returns>Angle orienté this -> v en radian. </returns>
        public double AngleTo(Point2D v)
        {
            double dot = this * v;
            if (Math.Abs(dot) > ZERO_APPROX)
            {
                return Math.Acos(dot / (Lenght * v.Lenght)) * Math.Sign(dot);
            }
                return 0;
        }

        /// <summary>
        /// Determine l'angle orienté entre this et le vecteur v. 
        /// </summary>
        /// <param name="v"></param>
        /// <returns>Angle orienté this -> v en radian. </returns>
        public double AngleTo(Vector2D v)
        {
            double dot = this * v;
            if (Math.Abs(dot) > ZERO_APPROX)
            {
                return Math.Acos(dot / (Lenght * v.Lenght)) * Math.Sign(dot);
            }
            return 0;
        }

        public Vector2D ToVector()
        {
            return new Vector2D(X, Y);
        }


        /// <summary>
        /// Indique si le point est l'origine de l'espace.
        /// </summary>
        public bool IsNull
        {
            get { return this.Lenght < ZERO_APPROX; }
        }
    
    }
}
