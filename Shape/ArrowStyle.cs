namespace LilliCad_Sample {
    class ArrowStyle {
        public int ArrowType { get; set; }
        public float ArrowSize { get; set; }
        public ArrowStyle(int arrowType, float arrowSize) {
            ArrowType = arrowType;
            ArrowSize = arrowSize;
        }
        public override string ToString() {
            return $"(Type({ArrowType}),Size({ArrowSize}))";
        }
    }
}
