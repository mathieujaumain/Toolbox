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
    public delegate void TurretKineticResultDelegate(double rotationSpeedAzimut, double rotationSpeedSite);
    public delegate void TurretStaticResultDelegate(double azimut, double site);

    public abstract class  AbstractTracker2D
    {
        public static long UPDATE_FREQUENCY = 500;
        private DynamicBody2D _Target;
        private DynamicBody2D _Camera;
        private bool _isTracking = false;
        private object _lock = new object();

        public event EventHandler OnTrackingStart = delegate { };
        public event TurretKineticResultDelegate NewTrackingKineticOutputComputed = delegate {};
        public event TurretKineticResultDelegate NewTrackingStaticOutputComputed = delegate { };
        public event EventHandler OnTrackingStop = delegate { };

        public void StartTracking()
        {
            if (!IsTracking)
            {
                _isTracking = true;
                new Thread(TrackingThread).Start();
            }
        }

        public void TrackingThread()
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
                    TrackingJob(elapsed);
                    timer.Restart();
                }
            }
            timer.Stop();
            OnTrackingStop(this, null);
        }

        public abstract void TrackingJob(double elapsed);

        public void StopTracking()
        {
            _isTracking = false;
        }


        public DynamicBody2D Target
        {
            get { lock(_lock) return _Target; }
            set { lock (_lock) _Target = value; }
        }


        public DynamicBody2D Camera
        {
            get { lock (_lock) return _Camera; }
            set { lock (_lock) _Camera = value; }
        }

        public bool IsTracking
        {
            get { lock(_lock) return _isTracking; }
        }
    }
    
}
