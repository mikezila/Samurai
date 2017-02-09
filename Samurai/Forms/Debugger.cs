using System;
using System.Windows.Forms;

namespace Samurai
{
    partial class Debugger : Form
    {
        Chip8CPU CPU;

        public Debugger(Chip8CPU cpu)
        {
            InitializeComponent();
            CPU = cpu;
        }

        private void Debugger_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
                e.Cancel = true;
        }

        private void closeDebuggerButton_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void UpdateDebugger()
        {
            memoryDumpListbox.DataSource = CPU.MMUState;
            memoryDumpListbox.SelectedIndex = CPU.PC;
            stackListBox.DataSource = CPU.StackState;
            cpuStateLabel.Text = CPU.CPUState;
            registersListBox.DataSource = CPU.RegisterState;
            opcodeListBox.DataSource = CPU.MMUStateOpcodes;
            opcodeListBox.SelectedIndex = CPU.PC / 2;
        }

        private void stepButton_Click(object sender, EventArgs e)
        {
            CPU.Step();
            UpdateDebugger();
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            UpdateDebugger();
        }
    }
}
