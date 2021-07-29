namespace LilliCad_Sample {
    class RadiusShape : SizeShapeBase {
        public CadPoint P0 { get; set; }
        public double Radius { get; set; }
        public double Angle { get; set; }
        public double TR { get; set; }
        public override string ToString() {
            return $"Radius(TEXT({Text}) FH({FontHeight}) FN({FontName}) ";
        }

    }
}
