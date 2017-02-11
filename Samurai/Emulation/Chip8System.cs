namespace Samurai
{
    class Chip8System
    {
        Chip8CPU CPU;
        Chip8GPU GPU;
        Chip8MMU MMU;

        public Chip8System()
        {
            MMU = new Chip8MMU();
            GPU = new Chip8GPU();
            CPU = new Chip8CPU(MMU, GPU);
        }

        public void Step()
        {
            CPU.Step();
        }
    }
}
