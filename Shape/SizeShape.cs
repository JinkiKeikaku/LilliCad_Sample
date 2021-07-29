using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LilliCad_Sample {
    class SizeShape : SizeShapeBase {
        public CadPoint[] Points { get; } = new CadPoint[5];
        public override string ToString() {
            return $"Size(TEXT({Text}) FH({FontHeight}) FN({FontName}) ";
        }
    }
}
