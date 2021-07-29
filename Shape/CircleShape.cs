using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LilliCad_Sample {
    class CircleShape : CadShape{
        public CadPoint P0 { get; set; }
        public double Radius { get; set; }
        public LineStyle LineStyle { get; set; }
        public FaceColor FaceColor { get; set; }
        public override string ToString() {
            return $"Circle(P0{P0} R({Radius}) LineStyle{LineStyle} FaceColor{FaceColor}";
        }

    }
}
