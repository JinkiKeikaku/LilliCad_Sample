using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LilliCad_Sample {
    class RectShape : CadShape{
        public CadPoint P0 { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public LineStyle LineStyle { get; set; }
        public FaceColor FaceColor { get; set; }
        public override string ToString() {
            return $"Rect(P0{P0} Size({Width}, {Height}) LineStyle{LineStyle} FaceColor{FaceColor}";
        }

    }
}
