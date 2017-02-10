using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samurai
{
    static class Util
    {
        public static System.Drawing.Size Scale(this System.Drawing.Size size, int scale)
        {
            return new System.Drawing.Size(size.Width * scale, size.Height * scale);
        }

        public static int RegisterX(this ushort value)
        {
            return (value >> 8) & 0xF;
        }

        public static int RegisterY(this ushort value)
        {
            return (value >> 4) & 0xF;
        }
    }
}
