using System.Collections.Generic;

namespace LilliCad_Sample {
    class LabelShape : CadShape{
        public LineStyle LineStyle { get; set; }
        public ArrowStyle Arrow { get; set; }
        public FaceColor FaceColor { get; set; }
        public string Text { get; set; }
        public string FontName { get; set; }
        public double FontHeight { get; set; }
        public int TextStyle { get; set; }
        public double TextGap { get; set; }
        public int TextColor { get; set; }
        public List<CadPoint> Points { get; } = new();
        public override string ToString() {
            return $"Label(Points.Count({Points.Count}) TEXT({Text}) FH({FontHeight}) FN({FontName}";
        }

    }
}
