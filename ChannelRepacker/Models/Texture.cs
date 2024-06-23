using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NullSoftware.Models
{
    public class Texture
    {
        public string Name { get; }

        public BitmapImage Source { get; }

        public int Height { get; }

        public int Width { get; }

        public byte[] Buffer { get; }

        public int Stride => Width * 4;

        public Texture(BitmapImage source, string name)
        {
            Name = name;
            Source = source;
            Height = source.PixelHeight;
            Width = source.PixelWidth;

            BitmapSource readable;
            if (Source.Format != PixelFormats.Bgra32)
            {
                readable = new FormatConvertedBitmap(Source, PixelFormats.Bgra32, null, 0);
            }
            else
            {
                readable = source;
            }

            Buffer = new byte[source.PixelHeight * source.PixelWidth * 4];
            readable.CopyPixels(Buffer, source.PixelWidth * 4, 0);
        }

        private int GetPixelCoord(int x, int y)
        {
            return y * Width + x; 
        }

        public byte GetBlue(int x, int y)
        {
            return Buffer[GetPixelCoord(x, y) * 4 + 0];
        }

        public byte GetGreen(int x, int y)
        {
            return Buffer[GetPixelCoord(x, y) * 4 + 1];
        }

        public byte GetRed(int x, int y)
        {
            return Buffer[GetPixelCoord(x, y) * 4 + 2];
        }

        public byte GetAlpha(int x, int y)
        {
            return Buffer[GetPixelCoord(x, y) * 4 + 3];
        }

        public bool ValidatePosition(int x, int y)
        {
            return (x > 0 && x < Width) && (y > 0 && y < Height);
        }
    }
}
