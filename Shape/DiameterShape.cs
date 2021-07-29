using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LilliCad_Sample {
    class DiameterShape : SizeShapeBase {
        public CadPoint P0 { get; set; }
        public double Radius { get; set; }
        public double Angle { get; set; }
        public double TR { get; set; }
        public override string ToString() {
            return $"Diameter(TEXT({Text}) FH({FontHeight}) FN({FontName}) ";
        }
    }
}
