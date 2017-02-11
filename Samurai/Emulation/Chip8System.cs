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
        public bool Crashed { get { return CPU.Crashed; } }

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
