namespace LilliCad_Sample {
    class AngleShape :  SizeShapeBase{
        public CadPoint P0 { get; set; }
        public double Radius { get; set; }
        public double[] Angles { get; } = new double[3];
        public override string ToString() {
            return $"Angle(TEXT({Text}) FH({FontHeight}) FN({FontName}) ";
        }

    }
}
