namespace LilliCad_Sample {
    class MultiTextShape : CadShape{
        public CadPoint P0 { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double FontHeight { get; set; }
        public double FontWidth { get; set; }
        public double Angle { get; set; }
        public int TextStyle { get; set; }
        public int TextBasis { get; set; }
        public int TextColor { get; set; }
        public int TextFormat { get; set; }
        public LineStyle LineStyle { get; set; }
        public FaceColor FaceColor { get; set; }
        public string FontName { get; set; }
        public string Text { get; set; }
        public override string ToString() {
            return $"MultiText(P0{P0} TS({TextStyle}) FM({TextFormat}) TEXT({Text}) FH({FontHeight}) Angle({Angle}) FN({FontName}) ";
        }
    }
}
