namespace LilliCad_Sample {
    class CadPoint {
        public CadPoint(double x, double y) {
            X = x;
            Y = y;
        }
        public double X { get; set; }
        public double Y { get; set; }
        public override string ToString() {
            return ($"({X:F1}, {Y:F1})");
        }
    }
}
