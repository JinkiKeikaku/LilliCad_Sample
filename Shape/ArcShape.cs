namespace LilliCad_Sample {
    class ArcShape : CadShape{
        public CadPoint P0 { get; set; }
        public double Radius { get; set; }
        public double StartAngle { get; set; }
        public double EndAngle { get; set; }
        public LineStyle LineStyle { get; set; }
        public FaceColor FaceColor { get; set; }
        public ArrowStyle StartArrow { get; set; }
        public ArrowStyle EndArrow { get; set; }
        public override string ToString() {
            return $"Arc(P0{P0} R({Radius}) Angle({StartAngle}, EndAngle)LineStyle{LineStyle} FaceColor{FaceColor}";
        }
    }
}
