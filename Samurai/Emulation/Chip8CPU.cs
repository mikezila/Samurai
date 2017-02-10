using System;
using System.Collections.Generic;
using System.Text;

namespace Samurai
{
    class Chip8CPU
    {
        // Other hardware
        Chip8MMU MMU;
        Chip8GPU GPU;

        // Handy flags
        bool gameLoaded;
        bool crashed;

        // Registers
        const int RegisterCount = 16;
        byte[] registers;
        const int ProgramStart = 0x200;
        ushort pc;
        ushort indexer;
        bool flag;

        // Timers
        byte delay;
        byte sound;

        // Stack
        const int StackDepth = 16;
        Stack<ushort> stack;
        byte sp;

        // Keypad
        const int KeyCount = 16;
        bool[] keypad;

        // Random
        Random rand;

        public bool GameLoaded
        {
            get
            {
                return gameLoaded;
            }
        }

        // Current CPU State
        public string CPUState
        {
            get
            {
                StringBuilder state = new StringBuilder();

                state.AppendLine("PC: " + pc.ToString("X4") + "  I: " + indexer.ToString("X4"));
                state.AppendLine(" F: " + (flag ? 1 : 0) + "    SP: " + sp.ToString("X2"));
                if (crashed)
                    state.AppendLine("!! CRASHED !!");

                return state.ToString();
            }
        }

        // Current Register State
        public string[] RegisterState
        {
            get
            {
                string[] registerStrings = new string[RegisterCount];
                for (int i = 0; i < RegisterCount; i++)
                {
                    registerStrings[i] = "V" + i.ToString("X") + " - " + registers[i].ToString("X2");
                }
                return registerStrings;
            }
        }

        // Program counter as simple int, for debugger
        public int PC
        {
            get
            {
                return pc;
            }
        }

        // Addresses on the stack, for debugger
        public string[] StackState
        {
            get
            {
                string[] stackStrings = new string[stack.Count];
                for (int i = 0; i < stack.Count; i++)
                {
                    stackStrings[i] = i.ToString("X1") + " " + stack.ToArray()[i].ToString("X4");
                }
                return stackStrings;
            }
        }

        // Current MMU States
        public string[] MMUState
        {
            get
            {
                return MMU.State;
            }
        }

        public string[] MMUStateOpcodes
        {
            get
            {
                return MMU.StateAsOpcodes;
            }
        }


        public Chip8CPU(Chip8MMU mmu, Chip8GPU gpu)
        {
            MMU = mmu;
            GPU = gpu;
            rand = new Random();
            Reset();
        }

        public void Reset()
        {
            // Hardware
            MMU.Reset();
            GPU.Reset();

            // Handy Flags
            gameLoaded = false;
            crashed = false;

            // Registers
            registers = new byte[RegisterCount];
            pc = ProgramStart;
            indexer = 0;
            flag = false;

            // Timers
            delay = 0;
            sound = 0;

            // Stack
            stack = new Stack<ushort>(StackDepth);
            sp = (byte)stack.Count;

            // Keypad
            keypad = new bool[KeyCount];
        }

        public void LoadROM(string path)
        {
            Reset();
            MMU.LoadROM(path);
            gameLoaded = true;
        }

        public void Step(int steps)
        {
            for (int i = 0; i < steps; i++)
                Step();
        }

        public void Step()
        {
            DecodeOpcode(MMU.ReadOpcode(pc));
            if (!crashed)
                pc += 2;
        }

        #region OpCode Decode

        public void DecodeOpcode(ushort opcode)
        {
            // Invalid/blank opcode
            if (opcode == 0x000)
            {
                crashed = true;
                return;
            }

            // Clear Display
            if (opcode == 0x00E0)
            {
                GPU.Clear();
                return;
            }

            // Return from subroutine
            if (opcode == 0x00EE)
            {
                pc = stack.Pop();
                return;
            }

            // Call machine code routine
            if ((opcode & 0xF000) == 0x0000)
            {
                throw new NotImplementedException("Machine code routines are not supported.");
            }

            // Jump to location
            if ((opcode & 0xF000) == 0x1000)
            {
                pc = (ushort)(opcode & 0x0FFF);
                return;
            }

            // Call subroutine
            if ((opcode & 0xF000) == 0x2000)
            {
                CallSubroutine((ushort)(opcode & 0x0FFF));
                return;
            }

            // Skip next if register is the same as given byte
            if ((opcode & 0xF000) == 0x3000)
            {
                if (registers[opcode.RegisterX()] == (opcode & 0x00FF))
                    pc += 2;
                return;
            }

            // Skip next if register is not the same as given byte
            if ((opcode & 0xF000) == 0x4000)
            {
                if (registers[opcode.RegisterX()] != (opcode & 0x00FF))
                    pc += 2;
                return;
            }

            // Skip next if registers are equal
            if ((opcode & 0xF000) == 0x5000)
            {
                if (registers[opcode.RegisterX()] == registers[opcode.RegisterY()])
                    pc += 2;
                return;
            }

            // Skip next if registers are not equal
            if ((opcode & 0xF000) == 0x6000)
            {
                if (registers[opcode.RegisterX()] != registers[opcode.RegisterY()])
                    pc += 2;
                return;
            }

            // 7xkk - ADD Vx, byte
            // Set Vx = Vx + kk.
            if ((opcode & 0xF000) == 0x7000)
            {
                registers[opcode.RegisterX()] += (byte)(opcode & 0x00FF);
                return;
            }

            // 8xy0 - LD Vx, Vy
            // Set Vx = Vy.
            if ((opcode & 0xF00F) == 0x8000)
            {
                LoadRegisterFromRegister(opcode & 0x0F00, opcode & 0x00F0);
                return;
            }

            // 8xy1 - OR Vx, Vy
            // Set Vx = Vx OR Vy.
            if ((opcode & 0xF00F) == 0x8001)
            {
                registers[opcode.RegisterX()] = (byte)(registers[opcode.RegisterX()] | registers[opcode.RegisterY()]);
                return;
            }

            // 8xy2 - AND Vx, Vy
            // Set Vx = Vx AND Vy.
            if ((opcode & 0xF00F) == 0x8002)
            {
                registers[opcode.RegisterX()] = (byte)(registers[opcode.RegisterX()] & registers[opcode.RegisterY()]);
                return;
            }

            // 8xy3 - XOR Vx, Vy
            // Set Vx = Vx XOR Vy.
            if ((opcode & 0xF00F) == 0x8003)
            {
                registers[opcode.RegisterX()] = (byte)(registers[opcode.RegisterX()] ^ registers[opcode.RegisterY()]);
                return;
            }

            // 8xy4 - ADD Vx, Vy
            // Set Vx = Vx + Vy, set VF = carry.
            if ((opcode & 0xF00F) == 0x8004)
            {
                int result = registers[opcode.RegisterX()] + registers[opcode.RegisterY()];
            }

            // We've received an unknown opcode
            crashed = true;
        }

        #endregion

        #region OpCode Implementations
        private void CallMachineRoutine(ushort address)
        {
            throw new NotImplementedException("Tried to call machine routine at 0x" + address.ToString("X4") + ".  Machine code routines are not supported.");
        }

        private void ClearScreen()
        {
            GPU.Clear();
        }

        private void ReturnFromSubroutine()
        {
            pc = stack.Pop();
        }

        private void UnconditionalJump(ushort address)
        {
            pc = address;
        }

        private void UnconditionalJumpWithRegisterZero(ushort address)
        {
            pc = (ushort)(address + registers[0]);
        }

        private void CallSubroutine(ushort address)
        {
            stack.Push(pc);
            pc = address;
        }

        private void SkipNextIfComparedBytesSame(byte x, byte y)
        {
            if (x == y)
                pc += 2;
        }

        private void SkipNextIfComparedBytesDifferent(byte x, byte y)
        {
            if (x != y)
                pc += 2;
        }

        private void LoadRegister(int register, byte data)
        {
            registers[register] = data;
        }

        private void LoadRegisterFromRegister(int destination, int source)
        {
            registers[destination] += registers[source];
        }

        private void AddByteToRegister(int register, byte data)
        {
            registers[register] += data;
        }

        private void AddByteToRegisterWithCarry(int register, byte data)
        {
            int result = registers[register] + data;
            flag = result > 255;
            registers[register] = (byte)(result & 0xFF);
        }

        private void SubtractByteFromRegisterWithBorrow(int register, byte data)
        {
            flag = registers[register] > data;
            registers[register] -= data;
        }

        private void SubtractByteFromRegisterWithBorrowReverse(int register, byte data)
        {
            flag = registers[register] < data;
            registers[register] = (byte)(data - registers[register]);
        }

        private void ShiftRegisterRight(int register)
        {
            flag = (registers[register] & 0x01) == 1;
            registers[register] = (byte)(registers[register] >> 1);
        }

        private void ShiftRegisterLeft(int register)
        {
            flag = (registers[register] & 0x80) == 1;
            registers[register] = (byte)(registers[register] << 1);
        }

        private void LoadIndexer(ushort value)
        {
            indexer = value;
        }

        private void SetRegisterToRandomByteWithMask(int register, byte mask)
        {
            registers[register] = (byte)(rand.Next(0, 256) & mask);
        }

        private void DrawSprite(int x, int y, int count)
        {
            byte[] sprites = new byte[count];

            for (int i = 0; i < count; i++)
                sprites[i] = MMU.ReadByte((ushort)(indexer + i));

            flag = GPU.DrawSprites(x, y, sprites);
        }

        private void SkipIfKeyHeld(int register)
        {
            if (keypad[registers[register]])
                pc += 2;
        }

        private void SkipIfKeyNotHeld(int register)
        {
            if (!keypad[registers[register]])
                pc += 2;
        }

        private void SetDelayTimer(int register)
        {
            delay = registers[register];
        }

        private void SetSoundTimer(int register)
        {
            sound = registers[register];
        }

        private void LoadRegisterFromDelayTimer(int register)
        {
            registers[register] = delay;
        }

        // TODO: Implement this properly
        private void WaitForKeyPress(int register)
        {
            throw new NotImplementedException("Keypad not emulated.");
            //registers[register] = 0x01;
        }

        private void AddRegisterToIndexer(int register)
        {
            indexer += registers[register];
        }

        // TODO: Implement this proplery.
        private void StoreBCDValue(int regsiter)
        {
            throw new NotImplementedException("BCD storage not emulated.");
        }

        private void LoadIndexerWithFontAddress(int register)
        {
            throw new NotImplementedException("Font set is not yet emulated.");
        }

        private void StoreRegistersAtIndexer(int count)
        {
            for (int i = 0; i < count; i++)
                MMU.WriteByte((ushort)(indexer + i), registers[i]);
        }

        private void LoadRegistersFromMemory(int count)
        {
            for (int i = 0; i < count; i++)
                registers[i] = MMU.ReadByte((ushort)(indexer + i));
        }
        #endregion
    }
}
