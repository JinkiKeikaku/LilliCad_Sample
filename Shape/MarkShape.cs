using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LilliCad_Sample {
    class MarkShape : CadShape{
        public CadPoint P0 { get; set; }
        public double Radius { get; set; }
        public override string ToString() {
            return $"Mark(P0{P0} R({Radius})";
        }
    }
}
