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

        // Flag for if the screen has changed since last draw
        public bool Dirty { get; set; }

        public Chip8GPU()
        {
            FrameBuffer = new Bitmap(ScreenWidth, ScreenHeight);
            Reset();
        }

        public void Reset()
        {
            pixels = new bool[ScreenWidth, ScreenHeight];
            UpdateFrameBuffer();
        }

        // Dare you enter my magical realm?
        public bool DrawSprites(int x, int y, byte[] sprites)
        {
            Dirty = true;
            bool collision = false;
            bool stickyCollision = false;

            for (int sprite = 0; sprite < sprites.Length; sprite++)
                for (int bit = 0; bit < 8; bit++)
                {
                    // Holy shit.  It works, though.
                    collision = SetPixelXOR((x + bit) % ScreenWidth, (y + sprite) % ScreenHeight, (sprites[sprite] >> (7 - bit)) & 0x1);
                    if (collision)
                        stickyCollision = true;
                }

            UpdateFrameBuffer();
            return stickyCollision;
        }

        // This is a mess but it seems to work right.  Don't fucking breathe on it.
        private bool SetPixelXOR(int x, int y, int value)
        {
            if (value == 0)
                return false;

            bool flag = false;
            if (pixels[x, y] && (value == 1))
            {
                pixels[x, y] = false;
                flag = true;
            }
            else if (pixels[x, y] == false && (value == 1))
            {
                pixels[x, y] = true;
                flag = false;
            }

            return flag;
        }

        private void UpdateFrameBuffer()
        {
            for (int x = 0; x < ScreenWidth; x++)
                for (int y = 0; y < ScreenHeight; y++)
                    FrameBuffer.SetPixel(x, y, pixels[x, y] ? onColor : offColor);
        }

        public void Clear()
        {
            Dirty = true;
            pixels = new bool[ScreenWidth, ScreenHeight];
        }
    }
}
