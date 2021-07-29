using System;
using System.IO;
using System.Text;

namespace LilliCad_Sample {
    class LilliCadReader : LilliCadReaderBase{

        public string Read(StreamReader sr) {
            var sb = new StringBuilder();
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
//                            ParsePaperInfo(sr, sb);
                        }
                        break;
                    case "ORIGIN": {
//                            ParseOrigin(sr, sb);
                        }
                        break;
                    case "GRID": {
//                            ParseGrid(sr, sb);
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
