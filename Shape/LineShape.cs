using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LilliCad_Sample {
    class LineShape : CadShape{
        public CadPoint P0 { get; set; }
        public CadPoint P1 { get; set; }
        public LineStyle LineStyle { get; set; }
        public ArrowStyle StartArrow { get; set; }
        public ArrowStyle EndArrow { get; set; }

        public override string ToString() {
            return $"Line(P0{P0} P1{P1} LineStyle{LineStyle} Arrow0{StartArrow} Arrow1{EndArrow}";
        }
    }
}
