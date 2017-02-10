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
        bool[,] pixels;

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
            pixels = new bool[ScreenWidth, ScreenHeight];
        }

        public bool DrawSprites(int x, int y, byte[] sprites)
        {
            bool collision = false;

            for (int sprite = 0; sprite < sprites.Length; sprite++)
                for (int bit = 0; bit < 8; bit++)
                {
                    pixels[(x + bit) % ScreenWidth, (y + sprite) % ScreenHeight] = ((sprites[sprite] >> (7 - bit)) & 0x1) == 1;
                }

            UpdateFrameBuffer();
            return collision;
        }

        private void UpdateFrameBuffer()
        {
            for (int x = 0; x < ScreenWidth; x++)
                for (int y = 0; y < ScreenHeight; y++)
                    FrameBuffer.SetPixel(x, y, pixels[x, y] ? onColor : offColor);
        }

        internal void Clear()
        {
            pixels = new bool[ScreenWidth, ScreenHeight];
        }
    }
}
