using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace NullSoftware.Models
{
    public abstract class TextureChannel
    {
        public Texture Texture { get; }

        public TextureChannel(Texture texture)
        {
            Texture = texture;
        }

        public abstract byte GetPixel(int x, int y);

        public virtual bool ValidatePosition(int x, int y)
        {
            return Texture.ValidatePosition(x, y);
        }
    }

    
}
