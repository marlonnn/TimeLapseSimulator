using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Summer.System.Util
{
    /// <summary>
    /// 字体工具类
    /// </summary>
    public class Fonts
    {
        public static Font NORMAL(float size = 12)
        {
            return new Font(Font.HELVETICA, size, Font.NORMAL);
        }

        public static Font NORMAL(Color color, float size = 12)
        {
            return new Font(Font.HELVETICA, size, Font.NORMAL, color);
        }

        public static Font BOLD(float size = 12)
        {
            return new Font(Font.HELVETICA, size, Font.BOLD);
        }

        public static Font BOLD(Color color, float size = 12)
        {
            return new Font(Font.HELVETICA, size, Font.BOLD, color);
        }

        public static Font ITALIC(float size = 12)
        {
            return new Font(Font.HELVETICA, size, Font.ITALIC);
        }

        public static Font ITALIC(Color color, float size = 12)
        {
            return new Font(Font.HELVETICA, size, Font.ITALIC, color);
        }

        public static Font BOLDITALIC(float size = 12)
        {
            return new Font(Font.HELVETICA, size, Font.BOLDITALIC);
        }

        public static Font BOLDITALIC(Color color, float size = 12)
        {
            return new Font(Font.HELVETICA, size, Font.BOLDITALIC, color);
        }
    }
}
