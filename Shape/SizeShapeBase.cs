using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LilliCad_Sample {
    class SizeShapeBase : CadShape{
        public LineStyle LineStyle { get; set; }
        public int Flag { get; set; }
        public int TextColor { get; set; }
        public FaceColor FaceColor { get; set; }
        public string FontName { get; set; }
        public double FontHeight { get; set; }
        public SizeStyle SizeStyle { get; set; }
        public ArrowStyle Arrow { get; set; }
        public string Text { get; set; }
    }

}
