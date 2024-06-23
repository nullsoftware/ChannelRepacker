using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NullSoftware.Models
{
    internal static class Palattes
    {
        private static BitmapPalette? _blackWhitePalatte;

        public static BitmapPalette BlackWhitePalatte
        {
            get
            {
                if (_blackWhitePalatte == null)
                {
                    Color[] colors = new Color[256];
                    for (int i = 0; i < colors.Length; i++)
                    {
                        colors[i] = Color.FromRgb((byte)i, (byte)i, (byte)i);
                    }

                    _blackWhitePalatte = new BitmapPalette(colors);
                }

                return _blackWhitePalatte;
            }
        }
    }
}
