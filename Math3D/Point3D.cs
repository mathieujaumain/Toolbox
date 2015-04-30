using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathieuJaumain.Tools.Math3D
{
    public class Point3D
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }


        public static double ZERO_APPROX = 0.00001;
        public static Point3D NULL_VECTOR = new Point3D(0, 0, 0);

        public Point3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Point3D(Point3D v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        public Point3D() : this(NULL_VECTOR) { }

        public Vector3D Add(Point3D v)
        {
            return new Vector3D(X + v.X, Y + v.Y, Z + v.Z);
        }

        public Vector3D Sub(Point3D v)
        {
            return new Vector3D(X - v.X, Y - v.Y, Z - v.Z);
        }

        public Point3D Add(Vector3D v)
        {
            return new Point3D(X + v.X, Y + v.Y, Z + v.Z);
        }

        public Point3D Sub(Vector3D v)
        {
            return new Point3D(X - v.X, Y - v.Y, Z - v.Z);
        }

        public double Lenght
        {
            get { return Math.Sqrt(X * X + Y * Y + Z * Z); }
        }


        public static Vector3D operator +(Point3D v, Point3D u)
        {
            return v.Add(u);
        }

        public static Vector3D operator -(Point3D v, Point3D u)
        {
            return v.Sub(u);
        }

        public static Point3D operator +(Point3D v, Vector3D u)
        {
            return v.Add(u);
        }

        public static Point3D operator -(Point3D v, Vector3D u)
        {
            return v.Sub(u);
        }

        public double Dot(Point3D v)
        {
            return (X * v.X) + (Y * v.Y) + (Z * v.Z);
        }

        public double Dot(Vector3D v)
        {
            return (X * v.X) + (Y * v.Y) + (Z * v.Z);
        }

        public static double operator *(Point3D v, Point3D u)
        {
            return v.Dot(u);
        }

        public static double operator *(Vector3D v, Point3D u)
        {
            return u.Dot(v);
        }

        public static double operator *(Point3D v, Vector3D u)
        {
            return v.Dot(u);
        }

        public Vector3D Cross(Point3D v)
        {
            return new Vector3D(Y * v.Z - Z * v.Y, Z * v.X - X * v.Z, X * v.Y - Y * v.X);
        }

        public Vector3D Cross(Vector3D v)
        {
            return new Vector3D(Y * v.Z - Z * v.Y, Z * v.X - X * v.Z, X * v.Y - Y * v.X);
        }

        public static Vector3D operator ^(Point3D v, Vector3D u)
        {
            return v.Cross(u);
        }

        public static Vector3D operator ^(Point3D v, Point3D u)
        {
            return v.Cross(u);
        }

        public Vector3D Multiply(double a)
        {
            return new Vector3D(X * a, Y * a, Z * a);
        }

        public static Vector3D operator *(double a, Point3D u)
        {
            return u.Multiply(a);
        }

        public static Vector3D operator *(Point3D u, double a)
        {
            return u.Multiply(a);
        }

        /// <summary>
        /// Test la colinéarité entre le vecteur this et le point u.
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public bool IsColinearTo(Point3D u)
        {
            return (this ^ u).Lenght < ZERO_APPROX;
        }

        /// <summary>
        /// Test la colinéarité entre le vecteur this et le vecteur u.
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public bool IsColinearTo(Vector3D u)
        {
            return (this ^ u).Lenght < ZERO_APPROX;
        }

        public override bool Equals(object obj)
        {
            if (obj is Point3D)
            {
                Point3D v = obj as Point3D;
                if ((this - v).Lenght < ZERO_APPROX)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determine l'angle entre le vecteur this et le vecteur v.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public double AngleTo(Point3D v)
        {
            double dot = this * v;
            if (Math.Abs(dot) > ZERO_APPROX)
            {
                return Math.Acos(dot / (Lenght * v.Lenght)) * Math.Sign(dot);
            }
            return 0;
        }

        /// <summary>
        /// Determine l'angle entre le vecteur this et le vecteur v.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public double AngleTo(Vector3D v)
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
    }
}
