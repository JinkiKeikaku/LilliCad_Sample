using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LilliCad_Sample {
    class LineStyle {
        int LineColor { get; set; }
        int LineType { get; set; }
        float LineWidth { get; set; }

        public LineStyle(int lineColor, int lineType, float lineWidth) {
            LineColor = lineColor;
            LineType = lineType;
            LineWidth = lineWidth;
        }

        public override string ToString() {
            var lc = CadShape.ToColorString(LineColor);
            var ls = ((LineType & 128) != 0) ? "Construction" : LineType.ToString();
            return $"(Color{lc}Type({ls}),Width({LineWidth}))";
        }

    }
}
