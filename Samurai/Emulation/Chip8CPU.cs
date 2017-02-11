using System;
using System.Collections.Generic;

namespace Samurai
{
    class Chip8CPU
    {
        // Other hardware
        Chip8MMU MMU;
        Chip8GPU GPU;

        // Handy flags
        public bool Crashed { get; private set; }

        // Handy Consts
        const int RegisterCount = 16;
        const int ProgramStart = 0x200;
        const int StackDepth = 16;
        const int KeyCount = 16;

        // Registers
        public byte[] Registers { get; private set; }
        public ushort PC { get; private set; }
        public ushort Indexer { get; private set; }
        public bool Flag { get; private set; }

        // Timers
        public byte Delay { get; private set; }
        public byte Sound { get; private set; }

        // Stack
        public Stack<ushort> Stack { get; private set; }
        public byte SP { get; private set; }

        // Keypad
        bool[] keypad;

        // Random
        Random rand;

        public Chip8CPU(Chip8MMU mmu, Chip8GPU gpu)
        {
            MMU = mmu;
            GPU = gpu;
            rand = new Random();
            Reset();
        }

        public void Reset()
        {
            // Handy Flags
            Crashed = false;

            // Registers
            Registers = new byte[RegisterCount];
            PC = ProgramStart;
            Indexer = 0;
            Flag = false;

            // Timers
            Delay = 0;
            Sound = 0;

            // Stack
            Stack = new Stack<ushort>(StackDepth);
            SP = (byte)Stack.Count;

            // Keypad
            keypad = new bool[KeyCount];
        }

        public void Step()
        {
            if (Crashed)
                return;

            if (Sound > 0)
                Sound--;
            if (Delay > 0)
            {
                Delay--;
                GPU.Dirty = true;
                return;
            }
            DecodeOpcode(MMU.ReadOpcode(PC));
        }

        #region OpCode Decode

        public void DecodeOpcode(ushort opcode)
        {
            // Invalid/blank opcode
            if (opcode == 0x000)
            {
                Crashed = true;
                return;
            }

            // 00E0 - CLS
            if (opcode == 0x00E0)
            {
                GPU.Clear();
                PC += 2;
                return;
            }

            // 00EE - RET
            if (opcode == 0x00EE)
            {
                PC = Stack.Pop();
                return;
            }

            // 0nnn - SYS addr
            if ((opcode & 0xF000) == 0x0000)
            {
                Crashed = true;
                throw new NotImplementedException("Machine code routines are not supported.");
            }

            // 1nnn - JP addr
            if ((opcode & 0xF000) == 0x1000)
            {
                PC = (ushort)(opcode & 0x0FFF);
                return;
            }

            // 2nnn - CALL addr
            if ((opcode & 0xF000) == 0x2000)
            {
                PC += 2;
                Stack.Push(PC);
                PC = (ushort)(opcode & 0x0FFF);
                return;
            }

            // 3xkk - SE Vx, byte
            // Skip next instruction if Vx = kk.
            if ((opcode & 0xF000) == 0x3000)
            {
                if (Registers[opcode.RegisterX()] == (opcode & 0x00FF))
                    PC += 2;
                PC += 2;
                return;
            }

            // 4xkk - SNE Vx, byte
            // Skip next instruction if Vx != kk.
            if ((opcode & 0xF000) == 0x4000)
            {
                if (Registers[opcode.RegisterX()] != (opcode & 0x00FF))
                    PC += 2;
                PC += 2;
                return;
            }

            // 5xy0 - SE Vx, Vy
            // Skip next instruction if Vx = Vy.
            if ((opcode & 0xF000) == 0x5000)
            {
                if (Registers[opcode.RegisterX()] == Registers[opcode.RegisterY()])
                    PC += 2;
                PC += 2;
                return;
            }

            // 6xkk - LD Vx, byte
            // Set Vx = kk.
            if ((opcode & 0xF000) == 0x6000)
            {
                Registers[opcode.RegisterX()] = (byte)(opcode & 0x00FF);
                PC += 2;
                return;
            }

            // 7xkk - ADD Vx, byte
            // Set Vx = Vx + kk.
            if ((opcode & 0xF000) == 0x7000)
            {
                Registers[opcode.RegisterX()] += (byte)(opcode & 0x00FF);
                PC += 2;
                return;
            }

            // 8xy0 - LD Vx, Vy
            // Set Vx = Vy.
            if ((opcode & 0xF00F) == 0x8000)
            {
                Registers[opcode.RegisterX()] = Registers[opcode.RegisterY()];
                PC += 2;
                return;
            }

            // 8xy1 - OR Vx, Vy
            // Set Vx = Vx OR Vy.
            if ((opcode & 0xF00F) == 0x8001)
            {
                Registers[opcode.RegisterX()] = (byte)(Registers[opcode.RegisterX()] | Registers[opcode.RegisterY()]);
                PC += 2;
                return;
            }

            // 8xy2 - AND Vx, Vy
            // Set Vx = Vx AND Vy.
            if ((opcode & 0xF00F) == 0x8002)
            {
                Registers[opcode.RegisterX()] = (byte)(Registers[opcode.RegisterX()] & Registers[opcode.RegisterY()]);
                PC += 2;
                return;
            }

            // 8xy3 - XOR Vx, Vy
            // Set Vx = Vx XOR Vy.
            if ((opcode & 0xF00F) == 0x8003)
            {
                Registers[opcode.RegisterX()] = (byte)(Registers[opcode.RegisterX()] ^ Registers[opcode.RegisterY()]);
                PC += 2;
                return;
            }

            // 8xy4 - ADD Vx, Vy
            // Set Vx = Vx + Vy, set VF = carry.
            if ((opcode & 0xF00F) == 0x8004)
            {
                int result = Registers[opcode.RegisterX()] + Registers[opcode.RegisterY()];
                Flag = result > 255;
                Registers[opcode.RegisterX()] = (byte)(result & 0xFF);
                PC += 2;
                return;
            }

            // 8xy5 - SUB Vx, Vy
            // Set Vx = Vx - Vy, set VF = NOT borrow.
            if ((opcode & 0xF00F) == 0x8005)
            {
                Flag = Registers[opcode.RegisterX()] >= Registers[opcode.RegisterY()];
                Registers[opcode.RegisterX()] = (byte)(Registers[opcode.RegisterX()] - Registers[opcode.RegisterY()]);
                PC += 2;
                return;
            }

            // 8xy6 - SHR Vx {, Vy}
            // Set Vx = Vx SHR 1.
            if ((opcode & 0xF00F) == 0x8006)
            {
                Flag = (Registers[opcode.RegisterX()] & 0x1) == 1;
                Registers[opcode.RegisterX()] = (byte)(Registers[opcode.RegisterX()] >> 1);
                PC += 2;
                return;
            }

            // 8xy7 - SUBN Vx, Vy
            // Set Vx = Vy - Vx, set VF = NOT borrow.
            if ((opcode & 0xF00F) == 0x8007)
            {
                Flag = Registers[opcode.RegisterY()] >= Registers[opcode.RegisterX()];
                Registers[opcode.RegisterX()] = (byte)(Registers[opcode.RegisterY()] - Registers[opcode.RegisterX()]);
                PC += 2;
                return;
            }

            // 8xyE - SHL Vx {, Vy}
            // Set Vx = Vx SHL 1.
            if ((opcode & 0xF00F) == 0x800E)
            {
                int result = Registers[opcode.RegisterX()] << 1;
                Flag = result > 255;
                Registers[opcode.RegisterX()] = (byte)(result);
                PC += 2;
                return;
            }

            // 9xy0 - SNE Vx, Vy
            // Skip next instruction if Vx != Vy.
            if ((opcode & 0xF00F) == 0x9000)
            {
                if (Registers[opcode.RegisterX()] != Registers[opcode.RegisterY()])
                    PC += 2;
                PC += 2;
                return;
            }

            // Annn - LD I, addr
            // Set I = nnn.
            if ((opcode & 0xF000) == 0xA000)
            {
                Indexer = (ushort)(opcode & 0x0FFF);
                PC += 2;
                return;
            }

            // Bnnn - JP V0, addr
            // Jump to location nnn +V0
            if ((opcode & 0xF000) == 0xB000)
            {
                PC = (ushort)(Registers[0] + (opcode & 0x0FFF));
                return;
            }

            // Cxkk - RND Vx, byte
            // Set Vx = random byte AND k
            if ((opcode & 0xF000) == 0xC000)
            {
                Registers[opcode.RegisterX()] = (byte)(rand.Next(0, 256) & (opcode & 0x00FF));
                PC += 2;
                return;
            }

            // Dxyn - DRW Vx, Vy, nibble
            // Display n-byte sprite starting at memory location I at(Vx, Vy)
            // set VF = collision.
            if ((opcode & 0xF000) == 0xD000)
            {
                byte[] sprites = new byte[opcode & 0xF];
                for (int i = 0; i < (opcode & 0xF); i++)
                    sprites[i] = MMU.ReadByte((ushort)(Indexer + i));
                GPU.DrawSprites(Registers[opcode.RegisterX()], Registers[opcode.RegisterY()], sprites);
                PC += 2;
                return;
            }

            // Ex9E - SKP Vx
            // Skip next instruction if key with the value of Vx is pressed.
            if ((opcode & 0xF0FF) == 0xE09E)
            {
                if (keypad[Registers[opcode.RegisterX()]])
                    PC += 2;
                PC += 2;
                return;
            }

            // ExA1 - SKNP Vx
            // Skip next instruction if key with the value of Vx is not pressed.
            if ((opcode & 0xF0FF) == 0xE0A1)
            {
                if (!keypad[Registers[opcode.RegisterX()]])
                    PC += 2;
                PC += 2;
                return;
            }

            // Fx07 - LD Vx, DT
            // Set Vx = delay timer value.
            if ((opcode & 0xF0FF) == 0xF007)
            {
                Registers[opcode.RegisterX()] = Delay;
                PC += 2;
                return;
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
                Delay = Registers[opcode.RegisterX()];
                PC += 2;
                return;
            }

            // Fx18 - LD ST, Vx
            // Set sound timer = Vx.
            if ((opcode & 0xF0FF) == 0xF018)
            {
                Sound = Registers[opcode.RegisterX()];
                PC += 2;
                return;
            }

            // Fx1E - ADD I, Vx
            // Set I = I + Vx.
            if ((opcode & 0xF0FF) == 0xF01E)
            {
                Indexer += Registers[opcode.RegisterX()];
                PC += 2;
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
                for (int i = 0; i <= (opcode.RegisterX()); i++)
                    MMU.WriteByte((ushort)(Indexer + i), Registers[i]);
                PC += 2;
                return;
            }

            // Fx65 - LD Vx, [I]
            // Read registers V0 through Vx from memory starting at location I.
            if ((opcode & 0xF0FF) == 0xF065)
            {
                for (int i = 0; i <= (opcode.RegisterX()); i++)
                    Registers[i] = MMU.ReadByte((ushort)(Indexer + i));
                PC += 2;
                return;
            }

            // We've received an unknown opcode
            Crashed = true;
        }
        #endregion
    }
}
