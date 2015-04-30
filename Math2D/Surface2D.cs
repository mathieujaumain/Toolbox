using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathieuJaumain.Tools.Math2D
{

    /// <summary>
    /// Classe décrivant un segment de surface plane.
    /// </summary>
    public class Surface2D
    {
        private List<Point2D> _Vertices = new List<Point2D>();
        public static double ZERO_APPROX = 0.00001;

        /// <summary>
        /// L'ensemble des sommets décrivatn la surface. 
        /// </summary>
        public List<Point2D> Vertices
        {
            get { return _Vertices; }
            set { _Vertices = value; }
        }

        public Surface2D(){}

        public Surface2D(List<Point2D> vertices)
        {
            Vertices.AddRange(vertices);
        }

        /// <summary>
        /// Indique si la surface comporte moins de 3 points ou si les vertices sont égales.
        /// </summary>
        public bool IsNull
        { 
            get
            { 
                if(Vertices.Count > 2)
                {
                    bool res = false;
                    for (int i = 0; i < Vertices.Count -1; i++)
                    {
                        res &= (Vertices[i] - Vertices[i + 1]).IsNull;
                        if (res)
                            return res;
                    }
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Test si le point est situé à l'intérieur de la surface.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool IsInside(Point2D p)
        {
          if (Vertices.Count < 3)
                return false;
            int leftCount = 0, rightCount = 0;
            double Yray = p.Y;
            Point2D inter = new Point2D();
            int count = Vertices.Count -1;
            Ray2D arrete;

            for (int i = 0; i < count; i++)
            {
                arrete = new Ray2D(new Point2D(Vertices[i].X, Vertices[i].Y), 
                    new Point2D(Vertices[i+1].X, Vertices[i+1].Y));
                if (arrete.IsRayIntersectingOnXAxis(Yray, out inter))
                {
                    if (inter.X > p.X)
                    {
                        rightCount += 1;
                    }
                    else
                    {
                        leftCount += 1;
                    }
                }
            }

            // Dernière arrete
            arrete = new Ray2D(new Point2D(Vertices[count].X, Vertices[count].Y), 
                new Point2D(Vertices[0].X, Vertices[0].Y));
            if (arrete.IsRayIntersectingOnXAxis(Yray, out inter))
            {
                if (inter.X > p.X)
                {
                    rightCount += 1;
                }
                else
                {
                    leftCount += 1;
                }
            }

            if (rightCount % 2 != 0 && leftCount % 2 != 0)
                return true;

            return false;
        
        }


    }
}
