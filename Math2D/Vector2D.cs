using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathieuJaumain.Tools.Math2D
{
    /// <summary>
    /// Classe représentant un vecteur dansun espace 2D.
    /// </summary>
    public class Vector2D
    {
        public double X { get; set; }
        public double Y { get; set; }

        public static double ZERO_APPROX = 0.00001;
        public static Vector2D NULL_VECTOR = new Vector2D(0, 0);

        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vector2D(Vector2D v)
        {
            X = v.X;
            Y = v.Y;
        }

        public Vector2D() :this(NULL_VECTOR){}

        public Vector2D Add(Vector2D v)
        {
            return new Vector2D(X + v.X, Y + v.Y);
        }

        public Vector2D Sub(Vector2D v)
        {
            return new Vector2D( X - v.X, Y - v.Y);
        }

        public double Lenght
        {
            get { return Math.Sqrt(X * X + Y * Y); }
        }

        public static Vector2D operator+(Vector2D v, Vector2D u)
        {
            return v.Add(u);
        }

        public static Vector2D operator-(Vector2D v, Vector2D u)
        {
            return v.Sub(u);
        }

        public double Dot(Vector2D v)
        {
            return (X * v.X )+ (Y * v.Y);
        }

        public static double operator*(Vector2D v, Vector2D u)
        {
            return v.Dot(u);
        }

        public double Det(Vector2D v)
        {
            return (X * v.Y) - (Y * v.X);
        }

        public Vector2D Multiply(double a)
        {
            return new Vector2D(X * a, Y * a); 
        }

        public static Vector2D operator *(double a, Vector2D u)
        {
            return u.Multiply(a);
        }

        public static Vector2D operator *( Vector2D u, double a)
        {
            return u.Multiply(a);
        }

        /// <summary>
        /// Test la colinéarité entre le veteur this et le vecteur u.
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public bool IsColinearTo(Vector2D u)
        {
            return Math.Abs(Det(u)) < ZERO_APPROX;
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector2D)
            {
                Vector2D v = obj as Vector2D;
                if ((this - v).Lenght < ZERO_APPROX)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determine l'angle en radian entre le vecteur this et le vecteur.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public double AngleTo(Vector2D v)
        {
            double dot = this * v;
            if (Math.Abs(dot) > ZERO_APPROX)
            {
                return Math.Acos(dot / (Lenght * v.Lenght)) * Math.Sign(dot);
            }
                return 0;
        }

        /// <summary>
        /// Indique si le vecteur this est nul.
        /// </summary>
        public bool IsNull
        {
            get { return this.Lenght < ZERO_APPROX; }
        }

        public void Normalize()
        {
            if(!IsNull)
            {
                X = X / Lenght;
                Y = Y / Lenght;
            }
        }

        public Vector2D Normalized()
        {
            if (!IsNull)
            {
                return new Vector2D(X / Lenght, Y / Lenght);
            }
            return NULL_VECTOR;
        }
    
    }
}
