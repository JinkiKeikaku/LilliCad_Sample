using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LilliCad_Sample {
    class PolygonShape : CadShape{
        public LineStyle LineStyle { get; set; }
        public FaceColor FaceColor { get; set; }
        public int Flag { get; set; }
        public ArrowStyle StartArrow { get; set; }
        public ArrowStyle EndArrow { get; set; }
        public List<CadPoint> Points { get; } = new();
        public override string ToString() {
            return $"Polygon(Points.Count({Points.Count}) LineStyle{LineStyle} FaceColor{FaceColor}  Arrow0{StartArrow} Arrow1{EndArrow}";
        }
    }
}
