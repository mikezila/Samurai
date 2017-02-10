using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Samurai
{
    public partial class MainForm : Form
    {
        int scaleFactor = 8;

        Chip8CPU CPU;
        Debugger debugger;

        public MainForm()
        {
            InitializeComponent();
            ClientSize = new Size(Chip8GPU.ScreenWidth, Chip8GPU.ScreenHeight).Scale(scaleFactor);
            CPU = new Chip8CPU(new Chip8MMU(), new Chip8GPU());
            debugger = new Debugger(CPU);
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
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
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
    }
}
