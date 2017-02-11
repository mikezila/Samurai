using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Samurai
{
    public partial class MainForm : Form
    {
        const int scaleFactor = 8;

        Chip8System Chip8VM;
        Debugger debugger;
        Graphics g;

        Stopwatch watch;

        public MainForm()
        {
            InitializeComponent();
            watch = new Stopwatch();
            ClientSize = new Size(Chip8GPU.ScreenWidth * scaleFactor, (Chip8GPU.ScreenHeight * scaleFactor) + 24);
            Chip8VM = new Chip8System();
            debugger = new Debugger(Chip8VM);
            g = CreateGraphics();
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileBox.Multiselect = false;
            var result = openFileBox.ShowDialog();
            if (result == DialogResult.Cancel)
                return;
            Chip8VM.Reset();
            Chip8VM.LoadROM(openFileBox.FileName);
            Chip8VM.Run();
        }

        int maxFrames = 16;
        int frame = 0;
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            if (!Chip8VM.Running)
                return;
            if (!Chip8VM.Debugging)
                while (!Chip8VM.FrameBufferDirty && frame++ < maxFrames)
                {
                    Chip8VM.Step();
                }
            frame = 0;
            if (Chip8VM.FrameBufferDirty)
            {
                g.DrawImage(Chip8VM.FrameBuffer, 0, 24, Chip8GPU.ScreenWidth * scaleFactor, Chip8GPU.ScreenHeight * scaleFactor);
                Chip8VM.FrameBufferDirty = false;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About().ShowDialog();
        }

        private void debuggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!debugger.Visible)
            {
                debugger.Show();
                debugger.Location = new Point(this.Width + this.Location.X, this.Location.Y);
            }
            else
                debugger.Hide();
            Focus();
        }

        private void manualStepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manualStepToolStripMenuItem.Checked = !manualStepToolStripMenuItem.Checked;
            Chip8VM.Debugging = manualStepToolStripMenuItem.Checked;
            debugger.StepButtonControl(manualStepToolStripMenuItem.Checked);
        }
    }
}
