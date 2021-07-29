using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LilliCad_Sample {
    class EllipseShape : CadShape{
        public CadPoint P0 { get; set; }
        public double RX { get; set; }
        public double RY { get; set; }
        public LineStyle LineStyle { get; set; }
        public FaceColor FaceColor { get; set; }
        public override string ToString() {
            return $"Ellipse(P0{P0} RX({RX}) RY({RY}) LineStyle{LineStyle} FaceColor{FaceColor}";
        }
    }
}
