using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathieuJaumain.Tools.Math2D;

namespace MathieuJaumain.Tools.Physic2D
{
    public class DynamicBody2D
    {
        public double Mass { get; set; }
        public Point2D Position { get; set; }
        public Vector2D Speed { get; set; }
        public Vector2D Bearing { get; set; }
    }
}
