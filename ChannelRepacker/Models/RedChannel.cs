using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullSoftware.Models
{
    public class RedChannel : TextureChannel
    {
        public RedChannel(Texture texture) : base(texture)
        {
        }

        public override byte GetPixel(int x, int y)
        {
            return Texture.GetRed(x, y);
        }
    }
}
