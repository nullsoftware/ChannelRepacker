using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullSoftware.Models
{
    public class FakeChannel : TextureChannel
    {
        public static readonly FakeChannel MinValue = new FakeChannel(byte.MinValue);
        public static readonly FakeChannel MaxValue = new FakeChannel(byte.MaxValue);

        public byte Result { get; }

        public FakeChannel(byte result) : base(null)
        {
            Result = result;
        }

        public override byte GetPixel(int x, int y)
        {
            return Result;
        }

        public override bool ValidatePosition(int x, int y)
        {
            return true;
        }
    }
}
