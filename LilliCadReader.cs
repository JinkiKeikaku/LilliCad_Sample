using System;
using System.IO;
using System.Text;

namespace LilliCad_Sample {
    class LilliCadReader : LilliCadReaderBase{

        public string Read(StreamReader sr) {
            var sb = new StringBuilder();
            var line = sr.ReadLine();
            if(line != "$$LilliCadText$$") throw new FileFormatException("Not LilliCad format");
            int version = ConvertInt(ReadTokens(sr), 0);
            sb.AppendLine($"lcd file version {version}");

            string sec = "";
            while (!sr.EndOfStream) {
                int c = sr.Peek();
                if (c == '[') {
                    var buf = sr.ReadLine();
                    var i = buf.IndexOf(']');
                    if (i < 0) throw new FileFormatException();
                    sec = buf[1..i];
                }
                switch (sec) {
                    case "PAPER": {
                            var paperName = ReadSingleString(sr);
                            var paperInfo = ReadSingleString(sr);
                            string[] tokens;
                            tokens = ReadTokens(sr);
                            var w = ConvertDouble(tokens, 0);
                            var h = ConvertDouble(tokens, 1);
                            var scaleName = ReadSingleString(sr);
                            tokens = ReadTokens(sr);
                            var scale = ConvertDouble(tokens, 0);
                            tokens = ReadTokens(sr);
                            int horz = ConvertInt(tokens, 0);
                            int basis = ConvertInt(tokens, 1);
                            sb.AppendLine($"Paper {paperName} {paperInfo}");
                            sb.AppendLine($"\tWidth:{w} Height:{h}");
                            sb.AppendLine($"\tScaleName {scaleName}  scale{scale}");
                        }
                        break;
                    case "ORIGIN": {
                            string[] tokens;
                            tokens = ReadTokens(sr);
                            var x = ConvertDouble(tokens, 0);
                            var y = ConvertDouble(tokens, 1);
                            sb.AppendLine($"Grid origin ({x}, {y})");
                        }
                        break;
                    case "GRID": {
                            string[] tokens;
                            tokens = ReadTokens(sr);
                            var x = ConvertDouble(tokens, 0);
                            var y = ConvertDouble(tokens, 1);
                            sb.AppendLine($"Grid size ({x}, {y})");
                        }
                        break;
                    case "TOOL": {
                            //TOOLセクション。ツールの共通設定です。
                            //つぎのセクションまで読み飛ばします。
                        }
                        break;
                    case "TOOLS":
                        //TOOLSセクション。各ツールの設定値が入っています。
                        //つぎのセクションまで読み飛ばします。
                        break;
                    case "LAYERS": {
                            //LAYERSセクション。
                            var tokens = ReadTokens(sr);
                            var sel = ConvertInt(tokens, 0);// 選択されていたレイヤー  ex 0
                            tokens = ReadTokens(sr);
                            var num = ConvertInt(tokens, 0);//レイヤーがいくつあるか(使わない)   ex 2
                            sb.AppendLine($"LAYERS(Select({sel}) Size({num}))");
                        }
                        break;
                    case "LAYER": {
                            string[] tokens;
                            string name = ReadSingleString(sr);
                            tokens = ReadTokens(sr);
                            int flag = ConvertInt(tokens, 0);
                            tokens = ReadTokens(sr);//  図形数。
                            int num = ConvertInt(tokens, 0);
                            sb.AppendLine($"\tLAYER(Name({name}) Flag({flag}) Shape size({num}))");
                            while (!sr.EndOfStream) {
                                if (Char.IsLetter((char)sr.Peek())) {
                                    //行頭が英字なら図形
                                    var shape = ReadShape(sr);
                                    if (shape != null) {
                                        sb.Append("\t\t");
                                        sb.AppendLine(shape.ToString());
                                    }
                                } else {
                                    if (sr.Peek() == '[') break;    //行頭が[で次のセクション開始
                                    sr.ReadLine();  //行読み飛ばし
                                }
                            }
                        }
                        break;
                    case "EOF":
                        return sb.ToString();
                    default:
                        sr.ReadLine();  //わからなければ行を読み飛ばし
                        break;
                }
                sec = "";
            }
            return  sb.ToString();
        }

    }
}
