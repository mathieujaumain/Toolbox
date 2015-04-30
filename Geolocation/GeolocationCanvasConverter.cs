using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathieuJaumain.Tools.Math2D;

namespace MathieuJaumain.Tools.Geolocation
{
    /// <summary>
    /// Classe permettant de convertir des position géodétique en coordonné planesur une surface (abstraite) de Canvas.
    /// </summary>
    public class GeolocationCanvasConverter
    {
        private double _Ordonate2Latitude = 0;
        private double _Abcis2Longitude = 0;
        private bool _IsReferencingDone = false;
        private Point2D _SphericalReference;
        private Point2D _CanvasReference;

        public GeolocationCanvasConverter(Point2D[] canvasPos, Point2D[] sphericalPos)
        {
            ComputeMap2SphericalConvertion(canvasPos, sphericalPos);
        }


        /// <summary>
        /// Calcule les rapports entre les deux repères, on suppose que les valeurs en jeu sont suffisament petites pour que localement, 
        /// le repère sphérique terrestre soit approximé par un repère lié au plan tangeant de la localité.
        /// </summary>
        /// <param name="canvasPos"></param>
        /// <param name="sphericalPos"></param>
        private void ComputeMap2SphericalConvertion(Point2D[] canvasPos, Point2D[] sphericalPos)
        {
            Vector2D deltaCanvas = canvasPos[0] - canvasPos[1];
            Vector2D deltaSpherical = sphericalPos[0] - sphericalPos[1];

            _Abcis2Longitude = Math.Abs(deltaSpherical.X /deltaCanvas.X );
            _Ordonate2Latitude = Math.Abs(deltaSpherical.Y / deltaCanvas.Y ) * -1; // * -1 car l'axe des ordonées du canvas et la latitude sont de sens contraire

            _SphericalReference = sphericalPos[0];
            _CanvasReference = canvasPos[0];
            _IsReferencingDone = true;
        }


        /// <summary>
        /// (x,y) => (latitude, longitude)
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public Vector2D FromCanvasVector2SphericalVector(Vector2D distance)
        {
            return new Vector2D(distance.X * _Abcis2Longitude, distance.Y * _Ordonate2Latitude);
        }

        /// <summary>
        /// (latitude, longitude) => (x,y)
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Vector2D FromSphericalVector2CanvasVector(Vector2D pos)
        {
            return new Vector2D(pos.X / _Abcis2Longitude, pos.Y / _Ordonate2Latitude);
        }

        /// <summary>
        /// Convertie une positition en latitude longitude en une position du canvas.
        /// </summary>
        /// <param name="sphericalPoint"></param>
        /// <returns></returns>
        public Point2D FromSphericalPoint2CanvasPoint(Point2D sphericalPoint)
        {
            if (IsReferencingDone)
            {
                Vector2D canvasDistance = FromSphericalVector2CanvasVector(sphericalPoint - _SphericalReference);

                return _CanvasReference + canvasDistance;
            }
            throw new MapReferencingException();
        }


        /// <summary>
        /// Convertie une position du canvas en une positition en latitude longitude .
        /// </summary>
        /// <param name="sphericalPoint"></param>
        /// <returns></returns>
        public Point2D FromCanvasPoint2SphericalPoint(Point2D canvasPoint)
        {
            if (IsReferencingDone)
            {
                Vector2D sphericalDistance = FromCanvasVector2SphericalVector(canvasPoint - _CanvasReference);

                return _SphericalReference + sphericalDistance;
            }
            throw new MapReferencingException();
        }

        /// <summary>
        /// Convertie une distance en metre en distance angular sur la surface terrestre. 
        /// Avec une approximation tangentielle.
        /// </summary>
        /// <param name="meters"></param>
        /// <returns></returns>
        public double FromPlaneDistance2SphericalAngle(double meters)
        {
            double earthRadius = 6371000;
            if (meters < 0)
                meters *= -1;
            // Simple trigo avec le triangle :
            //   
            //    meters/2
            //   _______
            //   |     /
            //   |    /
            //   |   /  earthRadius
            //   |_ /
            //   | /
            //   |/
            //

            double theta = Math.Atan(meters / (earthRadius *2)); 
            theta *= (180 / Math.PI);  // conversion en degré
            return theta * 2;
        }

        /// <summary>
        /// Convertie une distance en metre en distance sphérique
        /// </summary>
        /// <param name="meters"></param>
        /// <returns></returns>
        public double FromSphericalPoints2PlaneDistance(Point2D A, Point2D B)
        {
            double earthRadius = 6371000; // Rayon de Picard
            //http://geodesie.ign.fr/contenu/fichiers/Distance_longitude_latitude.pdf
            // Convertion en rad
            A.X *= ( Math.PI/180);
            A.Y *= (Math.PI/180);
            B.X *= (Math.PI/180);
            B.Y *= (Math.PI/180);

            Double val = Math.Sin(A.Y) * Math.Sin(B.Y) + Math.Cos(A.Y) * Math.Cos(B.Y) * Math.Cos(B.X - A.X);

            return Math.Acos(val) * earthRadius;
        }

        public double Abcis2Longitude
        {
            get { return _Abcis2Longitude; }
            set { _Abcis2Longitude = value; }
        }

        public double Ordonate2Latitude
        {
            get { return _Ordonate2Latitude; }
            set { _Ordonate2Latitude = value; }
        }

        public bool IsReferencingDone
        {
            get { return _IsReferencingDone; }
            set { _IsReferencingDone = value; }
        }

        /// <summary>
        /// Point de référence sur le canvas. Selon le répère du canvas.
        /// </summary>
        public Point2D Reference
        {
            get { return _CanvasReference; }
            set { _CanvasReference = value; }
        }
    
    }

    public class MapReferencingException : Exception
    {
        public MapReferencingException()
            : base("The converter is not referenced yet, cannot convert coordinates.")
        {

        }
    }
}
