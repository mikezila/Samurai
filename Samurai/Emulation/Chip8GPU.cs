using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samurai
{
    class Chip8GPU
    {
        public static int ScreenWidth { get; } = 64;
        public static int ScreenHeight { get; } = 32;

        public Chip8GPU()
        {
            Reset();
        }

        public void Reset()
        {

        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool DrawSprites(int x, int y, byte[] sprites)
        {
            return false;
        }
    }
}
