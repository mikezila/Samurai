using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samurai
{
    class Chip8GPU
    {
        // Screen size
        public static int ScreenWidth { get; } = 64;
        public static int ScreenHeight { get; } = 32;

        // Raw pixels
        byte[,] pixels;

        // Drawable framebuffer
        public Bitmap FrameBuffer { get; private set; }

        // Colors
        readonly Color offColor = Color.Black;
        readonly Color onColor = Color.White;

        public Chip8GPU()
        {
            Reset();
        }

        public void Reset()
        {
            FrameBuffer = new Bitmap(ScreenWidth, ScreenHeight);
            pixels = new byte[ScreenWidth, ScreenHeight];
        }

        public bool DrawSprites(int x, int y, byte[] sprites)
        {
            UpdateFrameBuffer();
            return false;
        }

        private void UpdateFrameBuffer()
        {
            for (int x = 0; x < ScreenWidth; x++)
                for (int y = 0; y < ScreenHeight; y++)
                    FrameBuffer.SetPixel(x, y, pixels[x, y] == 1 ? onColor : offColor);
        }

        internal void Clear()
        {
            pixels = new byte[ScreenWidth, ScreenHeight];
        }
    }
}
