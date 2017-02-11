using System;
using System.Windows.Forms;

namespace Samurai
{
    partial class Debugger : Form
    {
        Chip8System Chip8VM;

        public Debugger(Chip8System chip8vm)
        {
            InitializeComponent();
            Chip8VM = chip8vm;
        }

        // To stop issues with closing/spawning the debugger over and over.
        // It can still be hidden/shown using a button on the form.
        private void Debugger_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
                e.Cancel = true;
        }

        private void closeDebuggerButton_Click(object sender, EventArgs e)
        {
            Hide();
        }

        public void UpdateDebugger()
        {
            stackListBox.DataSource = Chip8VM.StackState;
            cpuStateLabel.Text = Chip8VM.CPUState;
            registersListBox.DataSource = Chip8VM.RegisterState;
        }

        public void StepButtonControl(bool state)
        {
            stepButton.Enabled = state;
            dumpMemoryButton.Enabled = state;
        }

        private void stepButton_Click(object sender, EventArgs e)
        {
            Chip8VM.Step();
            UpdateDebugger();
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            UpdateDebugger();
        }

        private void dumpMemoryButton_Click(object sender, EventArgs e)
        {
            //memoryDumpListbox.DataSource = CPU.MMUState;
            //memoryDumpListbox.SelectedIndex = CPU.PC;
            //opcodeListBox.DataSource = CPU.MMUStateOpcodes;
            //opcodeListBox.SelectedIndex = CPU.PC / 2;
        }
    }
}
