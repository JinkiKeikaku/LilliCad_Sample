using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LilliCad_Sample {
    class CadShape {
        public static readonly int LILLICAD_NULL_COLOR = 0x1000000;//(RGB(255,255,255) + 1)
        public static string ToColorString(int c) {
            if (c == LILLICAD_NULL_COLOR) return "(NULL_COLOR)";
            var r = c & 255;
            var g = (c >> 8) & 255;
            var b = (c >> 16) & 255;
            return $"({r},{g},{b})";
        }
    }
}
