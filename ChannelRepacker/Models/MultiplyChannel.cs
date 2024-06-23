using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullSoftware.Models
{
    internal class MultiplyChannel : TextureChannel
    {
        private TextureChannel _channelA;
        private TextureChannel _channelB;

        public MultiplyChannel(TextureChannel a, TextureChannel b) : base(null)
        {
            _channelA = a;
            _channelB = b;
        }

        public override byte GetPixel(int x, int y)
        {
            byte a = _channelA.GetPixel(x, y);
            byte b = _channelB.GetPixel(x, y);

            return (byte)((a * b + 0xFF) >> 8);
        }
    }
}
