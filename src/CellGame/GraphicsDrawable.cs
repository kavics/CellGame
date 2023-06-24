using System.Globalization;

namespace CellGame
{
    public class GraphicsDrawable : IDrawable
    {
        private string[] _darkColors = new[]
        {
            // used tool: https://coolors.co/gradient-palette/ffffff-ffff00?number=30

            "000000",
            "FFFFFF", "FFFFE3", "FFFFC6", "FFFFAA", "FFFF8E", "FFFF71", "FFFF55", "FFFF39", "FFFF1C", "FFFF00",

            "FFF600", "FFF200", "FFEE00", "FFEB00", "FFE700", "FFE300", "FFDF00", "FFDB00", "FFD800", "FFD400",
            "FFD000", "FFCC00", "FFC800", "FFC500", "FFC100", "FFBD00", "FFB900", "FFB600", "FFB200", "FFAE00",
            "FFAA00", "FFA600", "FFA300", "FF9F00", "FF9B00", "FF9700", "FF9300", "FF9000", "FF8C00", "FF8800",

            "FF8300", "FF7E00", "FF7A00", "FF7500", "FF7100", "FF6C00", "FF6800", "FF6300", "FF5F00", "FF5A00",
            "FF5600", "FF5100", "FF4D00", "FF4800", "FF4400", "FF3F00", "FF3B00", "FF3600", "FF3200", "FF2D00",
            "FF2900", "FF2400", "FF2000", "FF1B00", "FF1700", "FF1200", "FF0E00", "FF0900", "FF0500", "FF0000",

            "FF0000", "FB0000", "F70000", "F30000", "EF0000", "EA0000", "E60000", "E20000", "DE0000", "DA0000",
            "D60000", "D20000", "CE0000", "CA0000", "C60000", "C10000", "BD0000", "B90000", "B50000", "B10000",
            "AD0000", "A90000", "A50000", "A10000", "9D0000", "980000", "940000", "900000", "8C0000", "880000",

            "870404", "860707", "850B0B", "840E0E", "821212", "811515", "801818", "7F1C1C", "7E1F1F", "7D2222",
            "7C2626", "7A2929", "792D2D", "783030", "773333", "763737", "753A3A", "743D3D", "734141", "714444",
            "704848", "6F4B4B", "6E4E4E", "6D5252", "6C5555", "6B5858", "695C5C", "685F5F", "676363", "666666",

            "646464", "626262", "5F5F5F", "5D5D5D", "5B5B5B", "595959", "565656", "545454", "525252", "505050",
            "4D4D4D", "4B4B4B", "494949", "464646", "444444", "424242", "404040", "3D3D3D", "3B3B3B", "393939",
            "363636", "343434", "323232", "303030", "2D2D2D", "2B2B2B", "292929", "272727", "242424", "222222",

            "212121", "202020", "1F1F1F", "1E1E1E", "1C1C1C", "1B1B1B", "1A1A1A", "191919", "181818", "171717",
            "161616", "141414", "131313", "121212", "111111", "101010", "0F0F0F", "0E0E0E", "0D0D0D", "0B0B0B",
            "0A0A0A", "090909", "080808", "070707", "060606", "050505", "030303", "020202", "010101", "000000",

            "000000", "000000", "000000", "000000", "000000", "000000", "000000", "000000", "000000", "000000",
            "000000", "000000", "000000", "000000", "000000", "000000", "000000", "000000", "000000", "000000",
            "000000", "000000", "000000", "000000", "000000", "000000", "000000", "000000", "000000", "000000",
            "000000", "000000", "000000", "000000", "000000", "000000", "000000", "000000", "000000", "000000",
            "000000", "000000", "000000", "000000", "000000", "000000", "000000", "000000", "000000", "000000",
            "000000", "000000", "000000", "000000", "000000", "000000", "000000", "000000", "000000", "000000",
            "000000", "000000", "000000", "000000", "000000"
        };

        
        private Color[] _darkPalette;
        private Color[] _lightPalette;

        public World World { get; set; }
        public bool DarkMode { get; set; }
        public bool Glowing { get; set; }

        public GraphicsDrawable()
        {
            _lightPalette = new Color[256];
            _lightPalette[0] = new Color(255, 255, 255);
            for (int i = 1; i < _lightPalette.Length; i++)
                _lightPalette[i] = new Color(i-1, i-1, i-1);

            //_darkPalette = new Color[256];
            //_darkPalette[0] = new Color(0, 0, 0);
            //for (int i = 1; i < _darkPalette.Length; i++)
            //    _darkPalette[i] = new Color(256 - i, 256 - i, 256 - i);

            _darkPalette = new Color[256];
            _darkPalette[0] = new Color(0, 0, 0);
            for (int i = 1; i < _darkPalette.Length; i++)
                _darkPalette[i] = ColorFromString(_darkColors[i]);
        }
        private Color ColorFromString(string color)
        {
            if (color.Length != 6)
                throw new ArgumentException("Color string must be 6 characters long");

            int r = int.Parse(color.Substring(0, 2), NumberStyles.HexNumber);
            int g = int.Parse(color.Substring(2, 2), NumberStyles.HexNumber);
            int b = int.Parse(color.Substring(4, 2), NumberStyles.HexNumber);

            return new Color(r, g, b);
        }


        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            //canvas.Scale(0.5f, 0.5f);
            //canvas.Scale(2.0f, 2.0f);

            var palette = DarkMode ? _darkPalette : _lightPalette;

            canvas.StrokeSize = 8;

            for (var wy = 0; wy < World.Height; wy++)
            {
                var y = wy * 10f;
                for (var wx = 0; wx < World.Width; wx++)
                {
                    var x = wx * 10f;
                    var c = Convert.ToInt32(World.Generation + wy + wx) % (256 / 8);
                    //canvas.StrokeColor = _palette[c * 8];

                    if(Glowing)
                        canvas.StrokeColor = palette[World.Cells[wx, wy]];
                    else
                        canvas.StrokeColor = palette[World.Cells[wx, wy] == 1 ? 1 : 0];

                    canvas.DrawLine(x, y + 5.0f, x + 8.0f, y + 5.0f);
                }
            }
        }
    }
}
