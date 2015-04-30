using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MathieuJaumain.Tools.Physic2D;
using MathieuJaumain.Tools.Math2D;

namespace MathieuJaumain.Tools.Tracking
{
    /// <summary>
    /// Uniquement pour des cameras immobiles
    /// </summary>
    public class ProportionalNavigationTracker2D
    {

        public static long UPDATE_FREQUENCY = 500;
        public static double NAVIGATIONAL_CONSTANT = 5;
        private DynamicBody2D _Target;
        private Point2D _PreviousTargetPosition;
        private DynamicBody2D _Camera;
        private bool _isTracking = false;
        private object _lock = new object();

        public event EventHandler OnTrackingStart;
        public event TurretKineticResultDelegate NewTrackingKineticOutputComputed;
        public event TurretKineticResultDelegate NewTrackingStaticOutputComputed;
        public event EventHandler OnTrackingStop;

        public void StartTracking()
        {
            if (!IsTracking)
            {
                _isTracking = true;
                new Thread(TrackThread).Start();
            }
        }

        public void TrackThread()
        {
            OnTrackingStart(this, null);
            Stopwatch timer = new Stopwatch();
            timer.Start();
            double elapsed = 0;
            while (_isTracking && Target != null)
            {

                elapsed = timer.ElapsedMilliseconds;
                if (elapsed > UPDATE_FREQUENCY)
                {
                    TrackJob(elapsed);
                    timer.Restart();
                }
            }
            timer.Stop();
            OnTrackingStop(this, null);
        }

        private void TrackJob(double elapsed)
        {
            
        }



        public void StopTracking()
        {
            _isTracking = false;
        }


        public DynamicBody2D Target
        {
            get
            {
                lock(_lock) return _Target;
            }
            set
            {
                lock (_lock)
                {
                    _Target = value;
                    _PreviousTargetPosition = null;
                }
            }
        }

        public bool IsTracking
        {
            get { lock(_lock) return _isTracking; }
        }
    }
}
