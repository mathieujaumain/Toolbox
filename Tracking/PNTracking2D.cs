using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathieuJaumain.Tools.Physic2D;
using MathieuJaumain.Tools.Math2D;

namespace MathieuJaumain.Tools.Tracking
{
    /// <summary>
    ///  Proportional navigation - correction par rapport 
    /// </summary>
    public class PNTracking2D: AbstractTracker2D
    {
        private Point2D _PreviousCameraPos = null;
        private Point2D _PreviousTargetPos = null;

        public static double NAVIGATION_CONSTANT = 3;
        public static double BEARING_CORRECTION_CONSTANT = 2;

        public event EventHandler OnTrackingStart;
        public event TurretKineticResultDelegate NewTrackingKineticOutputComputed;
        public event TurretKineticResultDelegate NewTrackingStaticOutputComputed;
        public event EventHandler OnTrackingStop;

        public override void TrackingJob(double elapsed)
        {
            Point2D cameraPos = Camera.Position;
            Vector2D cameraBearing = Camera.Bearing;
            Point2D targetPos = Target.Position;
            Vector2D currentLos = targetPos - cameraPos;

            if (_PreviousCameraPos != null && _PreviousTargetPos != null )
            {
                Vector2D previousLos = _PreviousTargetPos - _PreviousCameraPos;
                double LOS_rate = previousLos.AngleTo(currentLos) / (elapsed * 1000); // = rad par secondes
                double PN = NAVIGATION_CONSTANT * LOS_rate;

                double target2bearing = currentLos.AngleTo(cameraBearing);
                double correction = BEARING_CORRECTION_CONSTANT * target2bearing;

                double newAzimutSpeed = Math.Sign(target2bearing) * PN - correction;

                NewTrackingKineticOutputComputed(newAzimutSpeed, 0);
            }

            _PreviousCameraPos = cameraPos;
            _PreviousTargetPos = targetPos;
        }
    }
}
