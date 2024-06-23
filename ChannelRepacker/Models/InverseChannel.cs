using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullSoftware.Models
{
    public class InverseChannel : TextureChannel
    {
        private TextureChannel _baseChannel;

        public InverseChannel(TextureChannel baseChannel) : base(baseChannel.Texture)
        {
            _baseChannel = baseChannel;
        }

        public override byte GetPixel(int x, int y)
        {
            return (byte)(byte.MaxValue - _baseChannel.GetPixel(x, y));
        }
    }
}
