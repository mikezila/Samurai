using System;
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

        public MainForm()
        {
            InitializeComponent();
            ClientSize = new Size(Chip8GPU.ScreenWidth * scaleFactor, (Chip8GPU.ScreenHeight * scaleFactor) + 24);
            Chip8VM = new Chip8System();
            g = CreateGraphics();
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            debugger = new Debugger(Chip8VM);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileBox.Multiselect = false;
            openFileBox.ShowDialog();
            CPU.LoadROM(openFileBox.FileName);
            debugger.UpdateDebugger();
            gameTimer.Enabled = !manualStepToolStripMenuItem.Checked;
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            if (CPU.Crashed)
            {
                gameTimer.Enabled = false;
                return;
            }
            g.DrawImage(CPU.FrameBuffer, 0, 24, Chip8GPU.ScreenWidth * scaleFactor, Chip8GPU.ScreenHeight * scaleFactor);
        }

        public void SystemStep()
        {
            CPU.Step();
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
        }
    }
}
