using System.Text;

namespace Samurai
{
    class Chip8System
    {
        Chip8CPU CPU;
        Chip8GPU GPU;
        Chip8MMU MMU;

        public bool Running { get; private set; }
        public bool Debugging { get; set; }

        public System.Drawing.Bitmap FrameBuffer { get { return GPU.FrameBuffer; } }
        public bool FrameBufferDirty
        {
            get { return GPU.Dirty; }
            set { GPU.Dirty = value; }
        }

        #region Debugging States
        public string CPUState
        {
            get
            {
                StringBuilder state = new StringBuilder();
                state.AppendLine("PC: 0x" + CPU.PC.ToString("X4") + " I: 0x" + CPU.Indexer.ToString("X4"));
                state.AppendLine(" F: " + (CPU.Flag ? "1" : "0"));
                if (CPU.Crashed)
                    state.AppendLine("!!! Crashed !!!");
                return state.ToString();
            }
        }

        public int PC { get { return CPU.PC; } }

        public bool Crashed { get { return CPU.Crashed; } }

        public string[] RegisterState
        {
            get
            {
                string[] stateStrings = new string[CPU.Registers.Length];
                for (int i = 0; i < CPU.Registers.Length; i++)
                    stateStrings[i] = "V" + i.ToString("X1") + " " + CPU.Registers[i].ToString("X4");
                return stateStrings;
            }
        }

        public string[] StackState
        {
            get
            {
                string[] stateStrings = new string[CPU.Stack.Count];
                for (int i = 0; i < CPU.Stack.Count; i++)
                    stateStrings[i] = CPU.Stack.ToArray()[i].ToString("X4");
                return stateStrings;
            }
        }

        public string[] MMUState { get { return MMU.State; } }
        public string[] MMUStateOpcodes { get { return MMU.StateAsOpcodes; } }
        #endregion

        public Chip8System()
        {
            MMU = new Chip8MMU();
            GPU = new Chip8GPU();
            CPU = new Chip8CPU(MMU, GPU);
        }

        public void LoadROM(string fileName)
        {
            MMU.LoadROM(fileName);
        }

        public void Run()
        {
            Running = true;
        }

        public void Halt()
        {
            Running = false;
        }

        public void Reset()
        {
            GPU.Reset();
            MMU.Reset();
            CPU.Reset();
        }

        public void Step()
        {
            if (Running)
                CPU.Step();
        }
    }
}
