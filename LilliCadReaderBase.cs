using System;
using System.Collections.Generic;
using System.IO;

namespace LilliCad_Sample {
    class LilliCadReaderBase {
        protected double mScale = 1.0;
        protected double mOriginX = 0.0;
        protected double mOriginY = 0.0;
        readonly float[][] mDotPattern = new float[][] {
            new float[]{0},
            new float[]{1.25f,1.25f},
            new float[]{2.5f,2.5f},
            new float[]{3.75f,1.25f},
            new float[]{3.75f,1.25f, 1.25f,1.25f},
            new float[]{6.25f,2.5f, 2.5f,2.5f},
            new float[]{3.25f,1.25f, 1.25f,1.25f, 1.25f,1.25f},
            new float[]{8.0f,2.5f, 1.25f,2.5f, 1.25f,2.5f},
            new float[]{0.625f, 1.875f},
            new float[]{0}
        };
        readonly Dictionary<string, Func<StreamReader, CadShape>> mShapeParseerMap;

        public LilliCadReaderBase() {
            mShapeParseerMap = new Dictionary<string, Func<StreamReader, CadShape>>(){
                {"LINE", ParseLine },
                {"POLYGON", ParsePolygon },
                {"SPLINE", ParseSpline },
                {"SPLINELOOP", ParseSplineLoop },
                {"TEXT", ParseText },
                {"MULTITEXT", ParseMultiText },
                {"RECT", ParseRect },
                {"CIRCLE", ParseCircle },
                {"ELLIPSE", ParseEllipse },
                {"ARC", ParseArc },
                {"FAN", ParseFan },
                {"MARK", ParseMark },
                {"SIZE", ParseSize },
                {"RADIUS", ParseRadius },
                {"DIAMETER", ParseDiameter },
                {"ANGLE", ParseAngle },
                {"LABEL", ParseLabel },
                {"BALLOON", ParseBalloon },
                //{"GROUP", ParseGroup },
                //{"BITMAP", ParseBitmap },
            };
        }

        public CadShape ReadShape(StreamReader sr) {
            string a;
            a = sr.ReadLine();
            if (mShapeParseerMap.ContainsKey(a)) {
                return mShapeParseerMap[a](sr);
            }
            return null;
        }

        CadShape ParseLine(StreamReader sr) {
            var s = new LineShape();
            var tokens = ReadTokens(sr);
            int pos = 0;
            (s.P0, pos) = ConvertPoint(tokens, pos);
            (s.P1, pos) = ConvertPoint(tokens, pos);
            (s.LineStyle, pos) = ConvertLineStyle(tokens, pos);
            (s.StartArrow, pos) = ConvertArrowStyle(tokens, pos);
            (s.EndArrow, _) = ConvertArrowStyle(tokens, pos);
            return s;
        }
        CadShape ParseMark(StreamReader sr) {
            var s = new MarkShape();
            var tokens = ReadTokens(sr);
            int pos = 0;
            (s.P0, pos) = ConvertPoint(tokens, pos);
            s.Radius = ConvertDouble(tokens, pos++);
            return s;
        }
        CadShape ParseCircle(StreamReader sr) {
            var s = new CircleShape();
            var tokens = ReadTokens(sr);
            int pos = 0;
            (s.P0, pos) = ConvertPoint(tokens, pos);
            s.Radius = ConvertDouble(tokens, pos++);
            (s.LineStyle, pos) = ConvertLineStyle(tokens, pos);
            (s.FaceColor, _) = ConvertFaceColor(tokens, pos);
            return s;
        }
        CadShape ParseArc(StreamReader sr) {
            var s = new ArcShape();
            var tokens = ReadTokens(sr);
            int pos = 0;
            (s.P0, pos) = ConvertPoint(tokens, pos);
            s.Radius = ConvertDouble(tokens, pos++);
            s.StartAngle = ConvertDouble(tokens, pos++);
            s.EndAngle = ConvertDouble(tokens, pos++);
            (s.LineStyle, pos) = ConvertLineStyle(tokens, pos);
            (s.FaceColor, pos) = ConvertFaceColor(tokens, pos);
            (s.StartArrow, pos) = ConvertArrowStyle(tokens, pos);
            (s.EndArrow, _) = ConvertArrowStyle(tokens, pos);
            return s;
        }
        CadShape ParseFan(StreamReader sr) {
            var s = new FanShape();
            var tokens = ReadTokens(sr);
            int pos = 0;
            (s.P0, pos) = ConvertPoint(tokens, pos);
            s.Radius = ConvertDouble(tokens, pos++);
            s.StartAngle = ConvertDouble(tokens, pos++);
            s.EndAngle = ConvertDouble(tokens, pos++);
            (s.LineStyle, pos) = ConvertLineStyle(tokens, pos);
            (s.FaceColor, _) = ConvertFaceColor(tokens, pos);
            return s;
        }
        CadShape ParseEllipse(StreamReader sr) {
            var s = new EllipseShape();
            var tokens = ReadTokens(sr);
            int pos = 0;
            (s.P0, pos) = ConvertPoint(tokens, pos);
            s.RX = ConvertDouble(tokens, pos++);
            s.RY = ConvertDouble(tokens, pos++);
            (s.LineStyle, pos) = ConvertLineStyle(tokens, pos);
            (s.FaceColor, _) = ConvertFaceColor(tokens, pos);
            return s;
        }
        CadShape ParsePolygon(StreamReader sr) {
            string[] tokens;
            var s = new PolygonShape();
            tokens = ReadTokens(sr);
            int pos = 0;
            (s.LineStyle, pos) = ConvertLineStyle(tokens, pos);
            (s.FaceColor, pos) = ConvertFaceColor(tokens, pos);
            s.Flag = ConvertInt(tokens, pos++);
            (s.StartArrow, pos) = ConvertArrowStyle(tokens, pos);
            (s.EndArrow, _) = ConvertArrowStyle(tokens, pos);
            ConvertPoints(sr, s.Points);
            return s;
        }
        CadShape ParseSpline(StreamReader sr) {
            string[] tokens;
            var s = new SplineShape();
            tokens = ReadTokens(sr);
            int pos = 0;
            (s.LineStyle, pos) = ConvertLineStyle(tokens, pos);
            (s.StartArrow, pos) = ConvertArrowStyle(tokens, pos);
            (s.EndArrow, pos) = ConvertArrowStyle(tokens, pos);
            //LilliCad Ver1.4.8zまでは面色を保存していなかった
            if (tokens.Length > pos) {   
                (s.FaceColor, _) = ConvertFaceColor(tokens, pos);
            }
            ConvertPoints(sr, s.Points);
            return s;
        }
        CadShape ParseSplineLoop(StreamReader sr) {
            string[] tokens;
            var s = new SplineLoopShape();
            tokens = ReadTokens(sr);
            int pos = 0;
            (s.LineStyle, pos) = ConvertLineStyle(tokens, pos);
            (s.FaceColor, _) = ConvertFaceColor(tokens, pos);
            ConvertPoints(sr, s.Points);
            return s;
        }
        CadShape ParseRect(StreamReader sr) {
            var s = new RectShape();
            var tokens = ReadTokens(sr);
            int pos = 0;
            (s.P0, pos) = ConvertPoint(tokens, pos);
            s.Width = ConvertDouble(tokens, pos++);
            s.Height = ConvertDouble(tokens, pos++);
            (s.LineStyle, pos) = ConvertLineStyle(tokens, pos);
            (s.FaceColor, _) = ConvertFaceColor(tokens, pos);
            return s;
        }
        CadShape ParseText(StreamReader sr) {
            var s = new TextShape();
            string[] tokens;
            tokens = ReadTokens(sr);
            int pos = 0;
            (s.P0, pos) = ConvertPoint(tokens, pos);
            s.Width = ConvertDouble(tokens, pos++);
            s.Height = ConvertDouble(tokens, pos++);
            s.FontHeight = ConvertDouble(tokens, pos++);
            s.FontWidth = ConvertDouble(tokens, pos++);
            s.Angle = ConvertDouble(tokens, pos++);
            s.TextStyle = ConvertInt(tokens, pos++);
            s.TextBasis = ConvertInt(tokens, pos++);
            s.TextColor = ConvertInt(tokens, pos++);
            (s.LineStyle, pos) = ConvertLineStyle(tokens, pos);
            (s.FaceColor, _) = ConvertFaceColor(tokens, pos);
            s.FontName = ReadSingleString(sr);
            s.Text = ReadString(sr);
            return s;
        }
        CadShape ParseMultiText(StreamReader sr) {
            var s = new MultiTextShape();
            string[] tokens;
            tokens = ReadTokens(sr);
            int pos = 0;
            (s.P0, pos) = ConvertPoint(tokens, pos);
            s.Width = ConvertDouble(tokens, pos++);
            s.Height = ConvertDouble(tokens, pos++);
            s.FontHeight = ConvertDouble(tokens, pos++);
            s.FontWidth = ConvertDouble(tokens, pos++);
            s.Angle = ConvertDouble(tokens, pos++);
            s.TextStyle = ConvertInt(tokens, pos++);
            s.TextBasis = ConvertInt(tokens, pos++);
            s.TextColor = ConvertInt(tokens, pos++);
            s.TextFormat = ConvertInt(tokens, pos++);
            (s.LineStyle, pos) = ConvertLineStyle(tokens, pos);
            (s.FaceColor, _) = ConvertFaceColor(tokens, pos);
            s.FontName = ReadSingleString(sr);
            s.Text = ReadString(sr);
            return s;
        }
        CadShape ParseSize(StreamReader sr) {
            var s = new SizeShape();
            string[] tokens;
            tokens = ReadTokens(sr);
            int pos = 0;
            for(int i = 0; i < 5; i++) {
                (s.Points[i], pos) = ConvertPoint(tokens, pos);
            }
            ParseSizeBase(sr, s, tokens, pos);
            return s;
        }
        CadShape ParseRadius(StreamReader sr) {
            var s = new RadiusShape();
            string[] tokens;
            tokens = ReadTokens(sr);
            int pos = 0;
            (s.P0, pos) = ConvertPoint(tokens, pos);
            s.Radius = ConvertDouble(tokens, pos++);
            s.Angle = ConvertDouble(tokens, pos++);
            s.TR = ConvertDouble(tokens, pos++);
            ParseSizeBase(sr, s, tokens, pos);
            return s;
        }
        CadShape ParseDiameter(StreamReader sr) {
            var s = new DiameterShape();
            string[] tokens;
            tokens = ReadTokens(sr);
            int pos = 0;
            (s.P0, pos) = ConvertPoint(tokens, pos);
            s.Radius = ConvertDouble(tokens, pos++);
            s.Angle = ConvertDouble(tokens, pos++);
            s.TR = ConvertDouble(tokens, pos++);
            ParseSizeBase(sr, s, tokens, pos);
            return s;
        }
        CadShape ParseAngle(StreamReader sr) {
            var s = new AngleShape();
            string[] tokens;
            tokens = ReadTokens(sr);
            int pos = 0;
            (s.P0, pos) = ConvertPoint(tokens, pos);
            s.Radius = ConvertDouble(tokens, pos++);
            for(int i = 0; i < 3; i++) {
                s.Angles[i] = ConvertDouble(tokens, pos++);
            }
            ParseSizeBase(sr, s, tokens, pos);
            return s;
        }
        CadShape ParseLabel(StreamReader sr) {
            var s = new LabelShape();
            string[] tokens;
            tokens = ReadTokens(sr);
            int pos = 0;
            (s.LineStyle, pos) = ConvertLineStyle(tokens, pos);
            (s.Arrow, pos) = ConvertArrowStyle(tokens, pos);
            (s.FaceColor, _) = ConvertFaceColor(tokens, pos);
            s.Text = ReadString(sr);
            s.FontName = ReadString(sr);    //ReadSingleString()ではない
            tokens = ReadTokens(sr);
            pos = 0;
            s.FontHeight = ConvertDouble(tokens, pos++);
            s.TextStyle = ConvertInt(tokens, pos++);
            s.TextGap = ConvertDouble(tokens, pos++);
            s.TextColor = ConvertInt(tokens, pos++);
            ConvertPoints(sr, s.Points);
            return s;
        }

        CadShape ParseBalloon(StreamReader sr) {
            var s = new BalloonShape();
            string[] tokens;
            tokens = ReadTokens(sr);
            int pos = 0;
            (s.LineStyle, pos) = ConvertLineStyle(tokens, pos);
            (s.Arrow, pos) = ConvertArrowStyle(tokens, pos);
            (s.FaceColor, _) = ConvertFaceColor(tokens, pos);
            tokens = ReadTokens(sr);
            pos = 0;
            s.RMin = ConvertDouble(tokens, pos++);
            s.RMax = ConvertDouble(tokens, pos++);
            s.Text = ReadString(sr);
            s.FontName = ReadString(sr);    //ReadSingleString()ではない
            tokens = ReadTokens(sr);
            pos = 0;
            s.FontHeight = ConvertDouble(tokens, pos++);
            s.TextStyle = ConvertInt(tokens, pos++);
            s.TextColor = ConvertInt(tokens, pos++);
            ConvertPoints(sr, s.Points);
            return s;
        }

        void ParseSizeBase(StreamReader sr, SizeShapeBase s, string[] tokens, int pos) {
            (s.LineStyle, pos) = ConvertLineStyle(tokens, pos);
            s.Flag = ConvertInt(tokens, pos++);
            s.TextColor = ConvertInt(tokens, pos++);
            (s.FaceColor, _) = ConvertFaceColor(tokens, pos);
            s.FontName = ReadSingleString(sr);
            tokens = ReadTokens(sr);
            pos = 0;
            s.FontHeight = ConvertDouble(tokens, pos++);
            (s.SizeStyle, pos) = ConvertSizeStyle(tokens, pos);
            (s.Arrow, _) = ConvertArrowStyle(tokens, pos);
            s.Text = ReadString(sr);
        }


        protected string ReadLine(StreamReader sr) {
            while (!sr.EndOfStream) {
                var line = sr.ReadLine();
                line = line.Trim();
                if (!string.IsNullOrEmpty(line)) return line;
            }
            throw new FileFormatException("Unexpectedly reached the end of the file");
        }
        protected  string ReadSingleString(StreamReader sr) {
            if (sr.Read() >= 0) {   //最初の空白文字除去
                var line = sr.ReadLine();
                if (line != null) return line;
            }
            throw new FileFormatException("Unexpectedly reached the end of the file");
        }
        protected string ReadString(StreamReader sr) {
            string s = "";
            var c = ReadTokens(sr);
            int n = ConvertInt(c, 0);
            if (n <= 0) return "";
            for (int i = 0; i < n - 1; i++) {
                s = s + ReadSingleString(sr) + "\n";
            }
            s += ReadSingleString(sr);
            return s;
        }
        protected string[] ReadTokens(StreamReader sr) {
            string a = ReadLine(sr);
            return a.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }
        protected int ConvertInt(string[] tokens, int pos) {
            return int.Parse(tokens[pos]);
        }
        protected double ConvertDouble(string[] tokens, int pos) {
            return double.Parse(tokens[pos]);
        }
        protected float ConvertFloat(string[] tokens, int pos) {
            return float.Parse(tokens[pos]);
        }
        protected (CadPoint p, int pos) ConvertPoint(string[] tokens, int pos) {
            return (new CadPoint(ConvertDouble(tokens, pos), ConvertDouble(tokens, pos + 1)), pos + 2);
        }
        void ConvertPoints(StreamReader sr, IList<CadPoint> points) {
            string[] tokens;
            tokens = ReadTokens(sr);
            var n = ConvertInt(tokens, 0);
            for (int i = 0; i < n; i++) {
                tokens = ReadTokens(sr);
                (var p, int _) = ConvertPoint(tokens, 0);
                points.Add(p);
            }
        }

        (LineStyle style, int pos) ConvertLineStyle(string[] tokens, int pos) {
            int lc = ConvertInt(tokens, pos++);
            int ls = ConvertInt(tokens, pos++);
            float lw = ConvertFloat(tokens, pos++);
            return (new LineStyle(lc, ls, lw), pos);
        }
        (ArrowStyle style, int pos) ConvertArrowStyle(string[] tokens, int pos) {
            int arrowType = ConvertInt(tokens, pos++);
            float arrowSize = ConvertFloat(tokens, pos++);
            return (new ArrowStyle(arrowType, arrowSize), pos);
        }
        (FaceColor fc, int pos) ConvertFaceColor(string[] tokens, int pos) {
            var fc = new FaceColor();
            if (tokens[pos][0] == 'G' || tokens[pos][0] == 'g') {
                //グラデーション
                switch (tokens[pos++][1]) {
                    case '1': {
                            //Line
                            fc.GradationType = FaceColor.Gradation.Line;
                            fc.Angle = ConvertFloat(tokens, pos++);
                        }
                        break;
                    case '2': {
                            //Rect
                            fc.GradationType = FaceColor.Gradation.Rectangle;
                            fc.Angle = ConvertFloat(tokens, pos++);
                            fc.X = ConvertFloat(tokens, pos++);
                            fc.Y = ConvertFloat(tokens, pos++);
                        }
                        break;
                    case '3': {
                            //Circle
                            fc.X = ConvertFloat(tokens, pos++);
                            fc.Y = ConvertFloat(tokens, pos++);
                        }
                        break;
                    default: {
                            //解析不能
                            throw new FileFormatException("Gradient color format error");
                        }
                }
                int n = ConvertInt(tokens, pos++);
                if (n == 2) {
                    fc.Colors[0] = ConvertInt(tokens, pos++);   //開始色
                    fc.Colors[1] = ConvertInt(tokens, pos++);   //終了色
                } else {
                    //グラデーションは３色までしかないのでn==2でなければ中間色１色
                    fc.Colors[0] = ConvertInt(tokens, pos++);   //開始色
                    fc.Colors[1] = ConvertInt(tokens, pos++);   //中間色
                    fc.Colors[2] = ConvertInt(tokens, pos++);   //終了色
                    fc.MP = ConvertFloat(tokens, pos++); //中間色の位置
                }
            } else {
                fc.GradationType = FaceColor.Gradation.Solid;
                fc.Colors[0] = ConvertInt(tokens, pos++);
            }
            return (fc, pos);
        }
        (SizeStyle, int) ConvertSizeStyle(string[] tokens, int pos) {
            var s = new SizeStyle {
                Linegap = ConvertFloat(tokens, pos++),
                Linejut = ConvertFloat(tokens, pos++),
                Linedrop = ConvertFloat(tokens, pos++),
                Textgap = ConvertFloat(tokens, pos++)
            };
            return (s, pos);
        }

    }
}
