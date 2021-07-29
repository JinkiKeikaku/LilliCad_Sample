using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LilliCad_Sample {
    class FaceColor {
        public enum Gradation { Solid, Line, Rectangle, Circle };
        public Gradation GradationType { get; set; } //0-3
        public int[] Colors = new int[3] { 0,0,0};
        public float MP { get; set; }  //中間色の位置
        public float Angle { get; set; } = 0.0f;   //  角度（GradationType:1と2で使用）
        public float X { get; set; } = 0.5f;
        public float Y { get; set; } = 0.5f;
        public override string ToString() {
            return $"(Type({GradationType}))";
        }
    }
}
