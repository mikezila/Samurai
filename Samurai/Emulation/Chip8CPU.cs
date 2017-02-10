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

            // 00E0 - CLS
            if (opcode == 0x00E0)
            {
                GPU.Clear();
                return;
            }

            // 00EE - RET
            if (opcode == 0x00EE)
            {
                pc = stack.Pop();
                return;
            }

            // 0nnn - SYS addr
            if ((opcode & 0xF000) == 0x0000)
            {
                crashed = true;
                throw new NotImplementedException("Machine code routines are not supported.");
            }

            // 1nnn - JP addr
            if ((opcode & 0xF000) == 0x1000)
            {
                pc = (ushort)(opcode & 0x0FFF);
                return;
            }

            // 2nnn - CALL addr
            if ((opcode & 0xF000) == 0x2000)
            {
                stack.Push(pc);
                pc = (ushort)(opcode & 0x0FFF);
                return;
            }

            // 3xkk - SE Vx, byte
            // Skip next instruction if Vx = kk.
            if ((opcode & 0xF000) == 0x3000)
            {
                if (registers[opcode.RegisterX()] == (opcode & 0x00FF))
                    pc += 2;
                return;
            }

            // 4xkk - SNE Vx, byte
            // Skip next instruction if Vx != kk.
            if ((opcode & 0xF000) == 0x4000)
            {
                if (registers[opcode.RegisterX()] != (opcode & 0x00FF))
                    pc += 2;
                return;
            }

            // 5xy0 - SE Vx, Vy
            // Skip next instruction if Vx = Vy.
            if ((opcode & 0xF000) == 0x5000)
            {
                if (registers[opcode.RegisterX()] == registers[opcode.RegisterY()])
                    pc += 2;
                return;
            }

            // 6xkk - LD Vx, byte
            // Set Vx = kk.
            if ((opcode & 0xF000) == 0x6000)
            {
                registers[opcode.RegisterX()] = (byte)(opcode & 0x00FF);
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
                registers[opcode.RegisterX()] = registers[opcode.RegisterY()];
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
                flag = result > 255;
                registers[opcode.RegisterX()] = (byte)result;
                return;
            }

            // 8xy5 - SUB Vx, Vy
            // Set Vx = Vx - Vy, set VF = NOT borrow.
            if ((opcode & 0xF00F) == 0x8005)
            {
                flag = registers[opcode.RegisterX()] > registers[opcode.RegisterY()];
                registers[opcode.RegisterX()] = (byte)(registers[opcode.RegisterX()] - registers[opcode.RegisterY()]);
                return;
            }

            // 8xy6 - SHR Vx {, Vy}
            // Set Vx = Vx SHR 1.
            if ((opcode & 0xF00F) == 0x8006)
            {
                flag = (registers[opcode.RegisterX()] & 0x1) == 1;
                registers[opcode.RegisterX()] = (byte)(registers[opcode.RegisterX()] >> 1);
                return;
            }

            // 8xy7 - SUBN Vx, Vy
            // Set Vx = Vy - Vx, set VF = NOT borrow.
            if ((opcode & 0xF00F) == 0x8007)
            {
                flag = registers[opcode.RegisterX()] < registers[opcode.RegisterY()];
                registers[opcode.RegisterX()] = (byte)(registers[opcode.RegisterY()] - registers[opcode.RegisterX()]);
                return;
            }

            // 8xyE - SHL Vx {, Vy}
            // Set Vx = Vx SHL 1.
            if ((opcode & 0xF00F) == 0x800E)
            {
                flag = (registers[opcode.RegisterX()] & 0x80) > 0;
                registers[opcode.RegisterX()] = (byte)(registers[opcode.RegisterX()] >> 1);
                return;
            }

            // 9xy0 - SNE Vx, Vy
            // Skip next instruction if Vx != Vy.
            if ((opcode & 0xF00F) == 0x9000)
            {
                if (registers[opcode.RegisterX()] != registers[opcode.RegisterY()])
                    pc += 2;
                return;
            }

            // Annn - LD I, addr
            // Set I = nnn.
            if ((opcode & 0xF000) == 0xA000)
            {
                indexer = (ushort)(opcode & 0x0FFF);
                return;
            }

            // Bnnn - JP V0, addr
            // Jump to location nnn +V0
            if ((opcode & 0xF000) == 0xB000)
            {
                pc = (ushort)(registers[0] + (opcode & 0x0FFF));
                return;
            }

            // Cxkk - RND Vx, byte
            // Set Vx = random byte AND k
            if ((opcode & 0xF000) == 0xC000)
            {
                registers[opcode.RegisterX()] = (byte)(rand.Next(0, 256) & (opcode & 0x00FF));
                return;
            }

            // Dxyn - DRW Vx, Vy, nibble
            // Display n-byte sprite starting at memory location I at(Vx, Vy)
            // set VF = collision.
            if ((opcode & 0xF000) == 0xD000)
            {
                byte[] sprites = new byte[opcode & 0xF];
                for (int i = 0; i < (opcode & 0xF); i++)
                    sprites[i] = MMU.ReadByte((ushort)(indexer + i));
                GPU.DrawSprites(registers[opcode.RegisterX()], registers[opcode.RegisterY()], sprites);
                return;
            }

            // Ex9E - SKP Vx
            // Skip next instruction if key with the value of Vx is pressed.
            if ((opcode & 0xF0FF) == 0xE09E)
            {
                if (keypad[registers[opcode.RegisterX()]])
                    pc += 2;
                return;
            }

            // ExA1 - SKNP Vx
            // Skip next instruction if key with the value of Vx is not pressed.
            if ((opcode & 0xF0FF) == 0xE0A1)
            {
                if (!keypad[registers[opcode.RegisterX()]])
                    pc += 2;
                return;
            }

            // Fx07 - LD Vx, DT
            // Set Vx = delay timer value.
            if ((opcode & 0xF0FF) == 0xF007)
            {
                registers[opcode.RegisterX()] = delay;
            }

            // Fx0A - LD Vx, K
            // Wait for a key press, store the value of the key in Vx.
            if ((opcode & 0xF0FF) == 0xF00A)
            {
                throw new NotImplementedException("Keypad isn't working yet.");
            }

            // Fx15 - LD DT, Vx
            // Set delay timer = Vx.
            if ((opcode & 0xF0FF) == 0xF015)
            {
                delay = registers[opcode.RegisterX()];
                return;
            }

            // Fx18 - LD ST, Vx
            // Set sound timer = Vx.
            if ((opcode & 0xF0FF) == 0xF018)
            {
                sound = registers[opcode.RegisterX()];
                return;
            }

            // Fx1E - ADD I, Vx
            // Set I = I + Vx.
            if ((opcode & 0xF0FF) == 0xF01E)
            {
                indexer += registers[opcode.RegisterX()];
                return;
            }

            // Fx29 - LD F, Vx
            // Set I = location of sprite for digit Vx.
            if ((opcode & 0xF0FF) == 0xF029)
            {
                throw new NotImplementedException("Digit sprites not in just yet.");
            }

            // Fx33 - LD B, Vx
            // Store BCD representation of Vx in memory locations I, I + 1, and I+2.
            if ((opcode & 0xF0FF) == 0xF033)
            {
                throw new NotImplementedException("BCD not implemented.");
            }

            // Fx55 - LD [I], Vx
            // Store registers V0 through Vx in memory starting at location I.
            if ((opcode & 0xF0FF) == 0xF055)
            {
                for (int i = 0; i < (opcode.RegisterX()); i++)
                    MMU.WriteByte(indexer, registers[i]);
            }

            // Fx65 - LD Vx, [I]
            // Read registers V0 through Vx from memory starting at location I.
            if ((opcode & 0xF0FF) == 0xF065)
            {
                for (int i = 0; i < (opcode.RegisterX()); i++)
                    registers[i] = MMU.ReadByte(indexer);
            }

            // We've received an unknown opcode
            crashed = true;
        }
        #endregion
    }
}
