# LilliCad file format　仕様書

## 概要

LilliCadの図面ファイルは単純なテキスト形式で保存されています。文字コードはシフトＪＩＳです。テキストファイルなのでテキストエディタで開いて内容を確認することができます。

## ファイル仕様

拡張子：lcd

文字コード：shift_jis

改行コード：CRLF("\r\n")もしくはLF("\n")。LilliCadはCRLF("\r\n")で保存しています。

## 構造

- １行目はヘッダー($$LilliCadText$$)です。
- ２行目はファイルバージョンです。ファイルバージョンは"1"です。
- ３行目以降は複数のセクションが続きます。セクション名は[]でくくられます。
- ファイルの終わりは[EOF]セクションです。
- セクション中の要素は（[LAYER]セクション中の図形要素名を除いて）空白文字を行頭に置きます。空白文字は、半角スペース" "またはタブ"\t"です。LilliCadはタブ("\t")を１文字使用しているのでタブ１文字を推奨します。
  ただし、レイヤ名、TEXT図形などのフォント名、表示文字列の行頭の空白は必ず１文字としてください。
- 未知のセクションが現れた場合、読み取ばすことができるように実装することを推奨します。
- [TOOL]及び[TOOLS]セクションは図形描画ツールの設定です。仕様は非公開です。図面の表示には影響しないので読み飛ばしてください。また、保存時はこれらのセクションは無くても構いません。
- 空行について
  
  - 読み込むときは、空行が存在する可能性を配慮してください。
  - 読み込む行が文字列のみの場合（フォント名、テキストなど）行を空けることができません（文字列が空白なのかわからないためです）。
  - 保存時はあえて行を開ける必要はありません。目的は、目視で見やすくするためと思ってください。
- 図形要素は１行目に図形の種類（行頭の空白なし）、２行目以降にパラメータが続きます。パラメータは複数行に及ぶものがあります。
- 図形要素の座標、大きさなどのほとんどは実寸になりますが、矢印の大きさ、線幅などは用紙上の寸法（以下、用紙寸）になりますので注意してください。
- 画像などのバイナリデータはBASE64でテキストに変換しています。なお、フラグによる指定でzlibで圧縮後にBASE64に変換することもできます（例えば、LilliCadの画像は、ヘッダーは未圧縮、データは圧縮されています）。
- 色はWindowsのCOLORREF型にもとづきます（赤：0x000000FF、緑：0x0000FF00、青：0x00FF0000）。ただし、0x01000000は透明色です。
- 未知の図形要素を読み飛ばせるように実装することを推奨します。

### 例
```
$$LilliCadText$$	←ヘッダー

1	←ファイル形式のバージョン
[PAPER]	←セクション
	A4
	297mm×210mm
	512 410
	・・・

[ORIGIN]
	・・・
[GRID]
	・・・
[TOOL]	←読み飛ばしても構わない。保存時はなくてもよい
	・・・
[TOOLS]	←読み飛ばしても構わない。保存時はなくてもよい
	・・・
[LAYERS]
	・・・
[LAYER]
	Layer1
	7
	3

LINE	←図形要素名。行頭のタブ文字は無し
	1000 1000 2000 2000 0 0 0 0 0 0 0	←図形パラメータ
		←空行があってもよい
LINE
	1000 1000 2000 2000 0 0 0 0 0 0 0
TEXT
	14623.8341968912 42512.9533678756 2000 -1000 1000 0 0 0 0 0 0 0 0 16777216
	ＭＳ ゴシック　		←文字列のためこの行の前の行は空けない

​	1
​	TEXT				←文字列のためこの行の前の行は空けない
​	・・・
[LAYER]
​	・・・
[EOF]	←終了
```
## セクション解説

解説中、行頭の数字と：の組は行番号を示す。

[PAPER]セクション
```
 1:[PAPER]
 2:	A4
 3:	  297mm×210mm
 4:	297 210
 5:	1 : 100
 6:	0.01
 7:	0 6
```
1. セクション名
2. 用紙名。A1,A2,A3など。任意。
3. 用紙情報。任意。
4. 用紙サイズ。幅：DOUBLE　高さ：DOUBLE。
5. 縮尺名。任意。
6. 縮尺：INT。
7. 用紙方向　用紙原点：DOUBLE　
   用紙方向：INT　0：水平（標準）　1：垂直
   用紙原点：INT　0：左上　1：中上　2：右上　3：左中　4：中央　5：右中　6：左下（標準）7：中下　8：右下

- 2，3，5は人間が理解しやすいようにつけたもので書式は規定しない。

### [ORIGIN]セクション

 ```
 1:[ORIGIN]
 2:	14850 10500
```
1. セクション名
2. グリッド原点　X:DOUBLE　Y:DOUBLE

- グリッド原点は実寸

### [GRID]セクション
```
 1:[GRID]
 2:	1000 1000
```
1. セクション名
2. グリッド間隔　DX:DOUBLE　DY:DOUBLE

- グリッド間隔は実寸

### [TOOL]セクション

説明省略

### [TOOLS]セクション

説明省略

### [LAYERS]セクション
```
 1:[LAYERS]
 2:	0
 3:	2
```
1. セクション名
2. 選択レイヤナンバー：INT
3. レイヤ数：INT

### [LAYER]セクション
```
 1:[LAYER]
 2:	Layer1
 3:	7
 4:	1
 5:LINE
 6:	10935.6223175966 7360.51502145923 15982.8326180258 8021.45922746781 0 0 0 0 0 0 0
```

1. セクション名
2. レイヤ名。レイヤ名は空行でないこと。
3. フラグ
4. 図形数
5. 図形要素名
6. 図形パラメータ

- LilliCadの図形はレイヤで管理されます。

- LAYERセクションはレイヤの数だけ存在する。

- レイヤは現れた順に下から上へ重ねられます。

- 同じ名前のレイヤが現れた場合の動作は未定義です。LilliCadは同名のレイヤは同じレイヤとみなしますが、SakraCadは別のレイヤとして別名をつけます。

- 図形パラメータは複数行にわたるものがあります。

- 図形要素名と図形パラメータの組は図形数続きます。
   
- レイヤ内での図形の表示順は出現順です。



### [EOF]セクション

- このセクションで終了となります。

## 図形要素解説

基本的に座標などの大きさは実寸になりますが、線幅などは用紙上のサイズになります。その場合、パラメータの数値の種類に(P)と追記します。

### 共通項目

#### 数値

- FLOAT（実数）とINT（整数）があります。
- LilliCadでは座標は倍精度、線幅などは単精度ですが、この仕様書では分けずにFLOATとしています。
- 角度は実数でradianです。

#### 線色・文字色

- WindowsのGDIのCOLORREF型にもとづきます(0BGR)
- ただし、0x01000000は透明色です。

#### 線種

- 0から8の9種類の線種があります。

- 7ビット目は補助線フラグで1なら補助線です。

- LilliCadでは補助線のパターンは8なので線種コードは8＋128で136となります。

- パターン

  0：実線

  1：点線1	(1.25, 1.25)

  2：点線2	(2.5, 2.5)

  3：点線3	(3.75, 1.25)

  4：一点鎖線1　(3.75, 1.25, 1.25, 1.25)

  5：一点鎖線2　(6.25, 2.5, 2.5, 2.5)

  6：二点鎖線1　(3.25, 1.25, 1.25, 1.25, 1.25, 1.25)

  7：二点鎖線2　(8.0 2.5, 1.25, 2.5, 1.25, 2.5)

  8：補助線　(0.625, 1.875)

#### 面色（FACECOLOR）

##### 単色

単色の場合はパラメータの要素は色のみです。

###### パラメータ

```
FC
```

FC:COLOR

##### グラデーション（線）

コードG1が現れます。

###### パラメータ

- 中間色なし(N=2)

```
  G1 A1 N C1 C2
```

- 中間色あり(N=3)

```
  G1 A1 N C1 C0 C2 MP
```

  G1:STRING	識別コード"G1"

  A1:FLOAT	角度（rad）

  N:INT	色数。2か3のみ

  C0:COLOR　中間色。ただし、透明色は指定できない。

  C1:COLOR　開始色。ただし、透明色は指定できない。

  C2:COLOR　終了色。ただし、透明色は指定できない。

  MP:FLOAT	中間色位置（0.0 <= MP <= 1.0）

##### グラデーション（矩形）

コードG2が現れます。

###### パラメータ

- 中間色なし(N=2)

```
  G2 A1 X1 Y1 N C1 C2
```

- 中間色あり(N=3)

```
  G2 A1 X1 Y1 N C1 C0 C2 MP
```

  G2:STRING	識別コード"G2"

  A1:FLOAT	角度（rad）

  X1:FLOAT	中心位置X（0.0 <= X1 <= 1.0）

  Y1:FLOAT	中心位置Y（0.0 <= Y1 <= 1.0）

  N:INT	色数。2か3のみ

  C0:COLOR　中間色。ただし、透明色は指定できない。

  C1:COLOR　開始色。ただし、透明色は指定できない。

  C2:COLOR　終了色。ただし、透明色は指定できない。

  MP:FLOAT	中間色位置（0.0 <= MP <= 1.0）

##### グラデーション（円）

コードG3が現れます。

###### パラメータ

- 中間色なし(N=2)

```
  G3 X1 Y1 N C1 C2
```

- 中間色あり(N=3)

```
  G3 X1 Y1 N C1 C0 C2 MP
```

  G3:STRING	識別コード"G3"

  X1:FLOAT	中心位置X（0.0 <= X1 <= 1.0）

  Y1:FLOAT	中心位置Y（0.0 <= Y1 <= 1.0）

  N:INT	色数。2か3のみ

  C0:COLOR　中間色。ただし、透明色は指定できない。

  C1:COLOR　開始色。ただし、透明色は指定できない。

  C2:COLOR　終了色。ただし、透明色は指定できない。

  MP:FLOAT	中間色位置（0.0 <= MP <= 1.0）

#### 矢印タイプ

- コード

   0：なし

   1：矢印

   2：三角

   3：斜線

   4：X

   5：丸

   6：黒丸

#### バイナリ（BYTES）

- BASE64でコード化
- バイナリデータは圧縮と無圧縮がある。圧縮はzlibを使用。

##### パラメータ
```
N BASE64 C
BASE64STRINGS
```

##### パラメータ詳細

N:サイズ　バイナリデータのサイズ。圧縮時は解凍後のサイズ（元のサイズ）。

BASE64:STRING　コード化方法。常に"BASE64"。

C:INT　圧縮フラグ　0：圧縮なし1：圧縮あり

BASE64STRINGS:STRING　BASE64に変換されたバイト列。72文字を超えたら改行する。

### 図形要素

#### LINE

線分

##### パラメータ

```
X1 Y1 X2 Y2  LC LS LW AT1 AS1 AT2 AS2
```

##### パラメータ詳細

X1:FLOAT　始点X座標

Y1:FLOAT　始点Y座標

X2:FLOAT　終点X座標

Y2:FLOAT　終点Y座標

LC:COLOR　線色

LS:INT　線種

LW:FLOAT(P)　線幅

AT1:INT	始点矢印タイプ

AS1:FLOAT(P)　始点矢印サイズ

AT2:INT	終点矢印タイプ

AS2:FLOAT(P)　終点矢印サイズ

#### CIRCLE

円

##### パラメータ

```
X1 Y1 R LC LS LW FC
```

##### パラメータ詳細

X1:FLOAT　始点X座標

Y1:FLOAT　始点Y座標

R:FLOAT　半径

LC:COLOR　線色

LS:INT　線種

LW:FLOAT(P)　線幅

FC:FACECOLOR　面色

#### ELLIPSE

楕円

##### パラメータ

```
X1 Y1 RX RY LC LS LW FC
```

##### パラメータ詳細

X1:FLOAT　始点X座標

Y1:FLOAT　始点Y座標

RX:FLOAT　横半径

RY:FLOAT　縦半径

LC:COLOR　線色

LS:INT　線種

LW:FLOAT(P)　線幅

FC:FACECOLOR　面色

#### ARC

円弧

##### パラメータ

```
X1 Y1 R SA EA LC LS LW FC AT1 AS1 AT2 AS2
```

##### パラメータ詳細

X1:FLOAT　始点X座標

Y1:FLOAT　始点Y座標

R:FLOAT　横半径

SA:FLOAT　開始角(rad)

EA:FLOAT　終了角(rad)

LC:COLOR　線色

LS:INT　線種

LW:FLOAT(P)　線幅

FC:FACECOLOR　面色

AT1:INT	始点矢印タイプ

AS1:FLOAT(P)　始点矢印サイズ

AT2:INT	終点矢印タイプ

AS2:FLOAT(P)　終点矢印サイズ

#### FAN

扇形

##### パラメータ

```
X1 Y1 R SA EA LC LS LW FC
```

##### パラメータ詳細

X1:FLOAT　始点X座標

Y1:FLOAT　始点Y座標

R:FLOAT　横半径

SA:FLOAT　開始角(rad)

EA:FLOAT　終了角(rad)

LC:COLOR　線色

LS:INT　線種

LW:FLOAT(P)　線幅

FC:FACECOLOR　面色

#### RECT

矩形

##### パラメータ

```
X1 Y1 W H LC LS LW FC
```

##### パラメータ詳細

X1:FLOAT　始点X座標

Y1:FLOAT　始点Y座標

W:FLOAT　幅

H:FLOAT　高さ

LC:COLOR　線色

LS:INT　線種

LW:FLOAT(P)　線幅

FC:FACECOLOR　面色

#### POLYGON

連続線

##### パラメータ

```
LC LS LW FC FL AT1 AS1 AT2 AS2
N
X1 Y1
X2 Y2
・・・
Xn Yn
```

##### パラメータ詳細

LC:COLOR　線色

LS:INT　線種

LW:FLOAT(P)　線幅

FL:INT	フラグ　2または3。3で始点と終点を閉じる

FC:FACECOLOR　面色

AT1:INT	始点矢印タイプ

AS1:FLOAT(P)　始点矢印サイズ

AT2:INT	終点矢印タイプ

AS2:FLOAT(P)　終点矢印サイズ

N:頂点数

X1-Xn:FLOAT	頂点X（N個）

Y1-Yn:FLOAT	頂点Y（N個）

#### SPLINE

全ての頂点を通る両端が開いたスプライン。

##### パラメータ

```
LC LS LW AT1 AS1 AT2 AS2
N
X1 Y1
X2 Y2
・・・
Xn Yn
```

##### パラメータ詳細

LC:COLOR　線色

LS:INT　線種

LW:FLOAT(P)　線幅

AT1:INT	始点矢印タイプ

AS1:FLOAT(P)　始点矢印サイズ

AT2:INT	終点矢印タイプ

AS2:FLOAT(P)　終点矢印サイズ

FC:FACECOLOR　面色（※）

N:頂点数

X1-Xn:FLOAT	頂点X（N個）

Y1-Yn:FLOAT	頂点Y（N個）

- ※FCはLilliCad ver1.4.8zまでは存在しません。互換性を保つために面色がないことも考慮してください。
- 自然スプラインです。

#### SPLINELOOP

全ての頂点を通る両端が閉じたスプライン。

##### パラメータ

```
LC LS LW FC
N
X1 Y1
X2 Y2
・・・
Xn Yn
```

##### パラメータ詳細

LC:COLOR　線色

LS:INT　線種

LW:FLOAT(P)　線幅

FC:FACECOLOR　面色

N:頂点数

X1-Xn:FLOAT	頂点X（N個）

Y1-Yn:FLOAT	頂点Y（N個）

- 自然スプラインです。

#### MARK

点

##### パラメータ

```
X1 Y1 R
```

##### パラメータ詳細

X1:FLOAT　始点X座標

Y1:FLOAT　始点Y座標

R:FLOAT(P)　半径



#### TEXT

１行のテキスト

##### パラメータ

```
X1 Y1 W H FH FW A TS BA TC LC LS LW FC
FN
N
TEXT 
```

##### パラメータ詳細

X1:FLOAT　テキスト配置X座標

Y1:FLOAT　テキスト配置Y座標

W:FLOAT　テキストの表示領域幅

H:FLOAT　テキストの表示領域高さ

FH：FLOAT　フォントサイズ

FW：FLOAT　フォント幅

A：FLOAT	角度

TS:INT	テキストスタイル。以下のフラグのOR。

　1：イタリック　2：ボールド　4：下線　8：取り消し線

​	64：枠線　128：縦書き

BA:INT　テキスト配置基準

​	0：左上　1：中上　2：右上

​	4：左中　5：中央　6：右中

​	8：左下　9：中下　10：右下

TC:COLOR　文字色

LC:COLOR　線色（枠線色）

LS:INT　線種（枠線種）

LW:FLOAT(P)　線幅（枠線幅）

FC:FACECOLOR　面色（背景色）

FN：STRING　フォント名

N：文字列行数（常に1）

TEXT：文字列

- W及びHは回転前の文字列の表示領域サイズ。HはBAに関係なく左上基準で常にマイナスとなる。LilliCadでは参考に値を出力しているため読み込み時に使用していない。

#### MULTITEXT

１行のテキスト

##### パラメータ

```
X1 Y1 W H FH FW A TS BA TC FM LC LS LW FC
FN
N
TEXT 
```

##### パラメータ詳細

X1:FLOAT　テキスト配置X座標

Y1:FLOAT　テキスト配置Y座標

W:FLOAT　テキストの表示領域幅

H:FLOAT　テキストの表示領域高さ

FH：FLOAT　フォントサイズ

FW：FLOAT　フォント幅

A：FLOAT	角度

TS:INT	テキストスタイル。以下のフラグのOR。

　1：イタリック　2：ボールド　4：下線　8：取り消し線

​	64：枠線　128：縦書き

BA:INT　テキスト配置基準

​	0：左上　1：中上　2：右上

​	4：左中　5：中央　6：右中

​	8：左下　9：中下　10：右下

TC:COLOR　文字色

FM:INT	文字のフォーマット。以下のフラグのOR。。

　横方向[0：左　1：中央　2：右]のいずれか

​	縦方向[0：上　4：中央　8：下]のいずれか

​	16：ワードラップ

LC:COLOR　線色（枠線色）

LS:INT　線種（枠線種）

LW:FLOAT(P)　線幅（枠線幅）

FC:FACECOLOR　面色（背景色）

FN：STRING　フォント名

N：文字列行数

TEXT：文字列

- W及びHは回転前の文字列の表示領域サイズ。HはBAに関係なく左上基準で常にマイナスとなる。


#### BITMAP

ビットマップ。WindowsのDIB。

##### パラメータ

```
X1 Y1 W H 
FILEHEADER
INFOHEADER
DATA
```

##### パラメータ詳細

X1, Y1:FLOAT　原点（左下）

W,H:FLOAT　幅と高さ

FILEHEADER:BYTES	BITMAPFILEHEADER

INFOHEADER:BYTES	BITMAPINFOHEADER

DATA:BYTES	ビットマップ本体

#### OLE2

OLE2オブジェクト。

##### パラメータ

```
X1 Y1 W H 
OBJECT
```

##### パラメータ詳細

X1, Y1:FLOAT　原点（左下）

W,H:FLOAT　幅と高さ

OBJECT:BYTES	OLE2オブジェクト

- OBJECTはMFCのCOleClientItemをシリアライズしたものです。MFCを使えば読めると思います。
- 残念ながらOLE2がすでに終わっている感じなのでサポートの必要はないと思います。

#### GROUP

グループ図形。LilliCadのバージョン1.2.0より前はフォーマットが違います。

##### パラメータ

```
N
BF BX BY
SHAPES
```

##### パラメータ詳細

N:INT 図形数

BF:INT　基準点フラグ　0：基準点無効　1：基準点有効

BX,BY:FLOAT	基準点

SHAPES:図形要素	図形要素がN個	

- LilliCadのバージョン1.2.0より前はBF,BX,BYの行が存在せず、Nの次の行からSHAPESが現れました。
- あえて古いバージョンをサポートする必要は無いと思います。
- グループ図形の図形要素にグループ図形が入ることもあります。
- グループ図形内での図形の表示順は出現順です。

#### SIZE

寸法図形。

##### パラメータ

```
X1 Y1 X2 Y2 X3 Y3 X4 Y4 X5 Y5 LC LS LW F TC FC
FN
FH LG LJ LD TG AT AS
N
TEXT
```

##### パラメータ詳細

X1,Y1,X2,Y2:FLOAT	引き出し線位置

X3,Y3,X4,Y4:FLOAT	寸法線位置

X5,Y5:FLOAT	文字配置点（中下基準）

LC:COLOR　線色（枠線色）

LS:INT　線種（枠線種）

LW:FLOAT(P)　線幅（枠線幅）

F:INT	フラグ　Bit0：1　寸法値を自動記入しない（文字列使用）

TC:COLOR　文字色

FC:FACECOLOR　面色（背景色）

FH：FLOAT(P)　フォントサイズ

FN：STRING　フォント名

LG:FLOAT(P)	引出位置の離れ

LJ:FLOAT(P)	寸法線のはみ出し

LD:FLOAT(P)	引き出し線のはみ出し

TG:FLOAT(P)	寸法値文字の寸法線からの離れ

AT:INT	矢印タイプ

AS:FLOAT(P)　矢印サイズ

N：INT	文字列行数（常に1）

TEXT：STRING	文字列

- FH、LG、 LJ、 LD、 TGが用紙寸であることに注意してください。
- TEXTは図形が表示されていた時の文字列が入っています。
  FフラグのBit0が0の場合は自動寸法ですが、TEXTの値を表示するか新たに寸法値を表示するかはアプリの仕様になります。自動寸法では少数点以下の桁数などが変わる可能性があるため、TEXTの文字列を利用したほうがいいかもしれません。

#### RADIUS

半径寸法図形。

##### パラメータ

```
X1 Y1 R A TR LC LS LW F TC FC
FN
FH LG LJ LD TG AT AS
N
TEXT
```

##### パラメータ詳細

X1,Y1:FLOAT	寸法線始点（円の中心）

R:FLOAT	半径

A:FLOAT	角度

TR:FLOAT	文字位置（X１,Y１からの距離）

LC:COLOR　線色（枠線色）

LS:INT　線種（枠線種）

LW:FLOAT(P)　線幅（枠線幅）

F:フラグ　Bit0：1　寸法値を自動記入しない（文字列使用）

TC:COLOR　文字色

FC:FACECOLOR　面色（背景色）

FH:FLOAT(P)　フォントサイズ

FN:STRING　フォント名

LG:FLOAT(P)	引出位置の離れ（未使用）

LJ:FLOAT(P)	寸法線のはみ出し

LD:FLOAT(P)	引き出し線のはみ出し（未使用）

TG:FLOAT(P)	寸法値文字の寸法線からの離れ

AT:INT	矢印タイプ

AS:FLOAT(P)　矢印サイズ

N:INT	文字列行数（常に1）

TEXT:STRING	文字列

- FH、LG、 LJ、 LD、 TGが用紙寸であることに注意してください。
- TEXTは図形が表示されていた時の文字列が入っています。
  FフラグのBit0が0の場合は自動寸法ですが、TEXTの値を表示するか新たに寸法値を表示するかはアプリの仕様になります。自動寸法では少数点以下の桁数などが変わる可能性があるため、TEXTの文字列を利用したほうがいいかもしれません。
- LG,LJ,LD,TGは寸法図形に共通して現れます。半径寸法図形は引き出し線がないためLG及びLJは未使用になります。

#### DIAMETER

直径寸法図形。

##### パラメータ

```
X1 Y1 R A TR LC LS LW F TC FC
FN
FH LG LJ LD TG AT AS
N
TEXT
```

##### パラメータ詳細

X1,Y1:FLOAT	寸法線始点（円の中心）

R:FLOAT	半径

A:FLOAT	角度

TR:FLOAT	文字位置（X１,Y１からの距離）

LC:COLOR　線色（枠線色）

LS:INT　線種（枠線種）

LW:FLOAT(P)　線幅（枠線幅）

F:フラグ　Bit0：1　寸法値を自動記入しない（文字列使用）

TC:COLOR　文字色

FC:FACECOLOR　面色（背景色）

FH:FLOAT(P)　フォントサイズ

FN:STRING　フォント名

LG:FLOAT(P)	引出位置の離れ（未使用）

LJ:FLOAT(P)	寸法線のはみ出し

LD:FLOAT(P)	引き出し線のはみ出し（未使用）

TG:FLOAT(P)	寸法値文字の寸法線からの離れ

AT:INT	矢印タイプ

AS:FLOAT(P)　矢印サイズ

N:INT	文字列行数（常に1）

TEXT:STRING	文字列

- FH、LG、 LJ、 LD、 TGが用紙寸であることに注意してください。
- TEXTは図形が表示されていた時の文字列が入っています。
  FフラグのBit0が0の場合は自動寸法ですが、TEXTの値を表示するか新たに寸法値を表示するかはアプリの仕様になります。自動寸法では少数点以下の桁数などが変わる可能性があるため、TEXTの文字列を利用したほうがいいかもしれません。
- LG,LJ,LD,TGは寸法図形に共通して現れます。半径寸法図形は引き出し線がないためLG及びLJは未使用になります。

#### ANGLE

角度寸法図形。

##### パラメータ

```
X1 Y1 R A1 A2 A3 LC LS LW F TC FC
FN
FH LG LJ LD TG AT AS
N
TEXT
```

##### パラメータ詳細

X1,Y1:FLOAT	中心

R:FLOAT	半径

A1:FLOAT	開始角度

A2:FLOAT	終了角度

A3:FLOAT	文字位置の角度

LC:COLOR　線色（枠線色）

LS:INT　線種（枠線種）

LW:FLOAT(P)　線幅（枠線幅）

F:フラグ　Bit0：1　寸法値を自動記入しない（文字列使用）

TC:COLOR　文字色

FC:FACECOLOR　面色（背景色）

FH:FLOAT(P)　フォントサイズ

FN:STRING　フォント名

LG:FLOAT(P)	引出位置の離れ（未使用）

LJ:FLOAT(P)	寸法線のはみ出し

LD:FLOAT(P)	引き出し線のはみ出し（未使用）

TG:FLOAT(P)	寸法値文字の寸法線からの離れ

AT:INT	矢印タイプ

AS:FLOAT(P)　矢印サイズ

N:INT	文字列行数（常に1）

TEXT:STRING	文字列

- FH、LG、 LJ、 LD、 TGが用紙寸であることに注意してください。
- TEXTは図形が表示されていた時の文字列が入っています。
  FフラグのBit0が0の場合は自動寸法ですが、TEXTの値を表示するか新たに寸法値を表示するかはアプリの仕様になります。自動寸法では少数点以下の桁数などが変わる可能性があるため、TEXTの文字列を利用したほうがいいかもしれません。
- LG,LJ,LD,TGは寸法図形に共通して現れます。半径寸法図形は引き出し線がないためLG及びLJは未使用になります。

#### LABEL

引き出し線。

##### パラメータ

```
LC LS LW AT AS FC
N1
TEXT
N2
FN
FH TS TG TC
N
X1 Y1
X2 Y2
・・・
Xn Yn
```

##### パラメータ詳細

LC:COLOR　線色

LS:INT　線種

LW:FLOAT(P)　線幅

AT:INT	矢印タイプ

AS:FLOAT(P)　矢印サイズ

FC:FACECOLOR　面色

N1:INT	文字列行数（常に1）

TEXT:STRING	文字列

N2:INT	フォント名行数（常に1）

FN:STRING　フォント名

FH:FLOAT　フォントサイズ

TS:INT	テキストスタイル。以下のフラグのOR。

　1：イタリック　2：ボールド　4：下線　8：取り消し線

​	64：枠線　128：縦書き

TG:FLOAT	寸法値文字の寸法線からの離れ

TC:COLOR　文字色

N:頂点数

X1-Xn:FLOAT	頂点X（N個）

Y1-Yn:FLOAT	頂点Y（N個）

- FHとTGが実寸（用紙寸ではない）ことに注意してください。

#### BALLOON

バルーン。

##### パラメータ

```
LC LS LW AT AS FC
MIN MAX
N1
TEXT
N2
FN
FH TS TC
N
X1 Y1
X2 Y2
・・・
Xn Yn
```

##### パラメータ詳細

LC:COLOR　線色

LS:INT　線種

LW:FLOAT(P)　線幅

AT:INT	矢印タイプ

AS:FLOAT(P)　矢印サイズ

FC:FACECOLOR　面色

MIN,MAX:FLOAT	円の半径の最小値、最大値。0の場合自動設定（文字列のサイズから決める）。

N1:INT	文字列行数（常に1）

TEXT:STRING	文字列

N2:INT	フォント名行数（常に1）

FN:STRING　フォント名

FH:FLOAT　フォントサイズ

TS:INT	テキストスタイル。以下のフラグのOR。

　1：イタリック　2：ボールド　4：下線　8：取り消し線

​	64：枠線　128：縦書き

TC:COLOR　文字色

N:頂点数

X1-Xn:FLOAT	頂点X（N個）

Y1-Yn:FLOAT	頂点Y（N個）

- FH,MIN,MAXが実寸（用紙寸ではない）ことに注意してください。

## 補足説明

1. セクションについて
   [TOOL]セクションと[TOOLS]は保存しなくても問題はありません。また、[PAPER]、[ORIGIN]そして[GRID]も保存しなくても読み込めます。
   例えばLilliCadでは[PAPER]セクションがない場合、新規作成時の用紙及び縮尺が使用されます。
   しかし、縮尺がわからないと図形の確認が困難になるのであまり意味がありません。
   [ORIGIN]と[GRID]はなくても問題無いかもしれません（未確認）。
2. SPLINEに面色がなかったのは不具合です。可能なら修正します。

## 変更履歴

- 2021/7/29
  - FAN（扇形）追加
  - MULTITEXT追加
  - 空行の説明見直し
  - SPLINEの面色（FC）について記述修正。
  - SPLINE曲線の種類を追記。
  
- 2021/7/26
  - 最初のリリース
