using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LilliCad_Sample {
    class FanShape : CadShape {
        public CadPoint P0 { get; set; }
        public double Radius { get; set; }
        public double StartAngle { get; set; }
        public double EndAngle { get; set; }
        public LineStyle LineStyle { get; set; }
        public FaceColor FaceColor { get; set; }
        public override string ToString() {
            return $"Fan(P0{P0} R({Radius}) Angle({StartAngle}, {EndAngle})LineStyle{LineStyle} FaceColor{FaceColor}";
        }
    }
}
