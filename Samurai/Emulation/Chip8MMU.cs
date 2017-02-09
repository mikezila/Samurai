using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Samurai
{
    class Chip8MMU
    {
        byte[] Memory { get; set; }

        const int MemorySize = 0xFFF;
        const int MemoryRomStart = 0x200;

        public string[] State
        {
            get
            {
                string[] dumpedMemory = new string[MemorySize];
                for (int i = 0; i < MemorySize; i++)
                {
                    dumpedMemory[i] = "0x" + i.ToString("X3") + " " + ReadByte((ushort)i).ToString("X2");
                }
                return dumpedMemory;
            }
        }

        public string[] StateAsOpcodes
        {
            get
            {
                string[] dumpedMemory = new string[MemorySize / 2];
                for (int i = 0; i < dumpedMemory.Length; i++)
                {
                    dumpedMemory[i] = "0x" + (i * 2).ToString("X3") + " " + ReadOpcode((ushort)(i * 2)).ToString("X4");
                }
                return dumpedMemory;
            }
        }

        public Chip8MMU()
        {
            Reset();
        }

        public void Reset()
        {
            Memory = new byte[MemorySize];
        }

        public void LoadROM(string path)
        {
            Reset();
            File.ReadAllBytes(path).CopyTo(Memory, MemoryRomStart);
        }

        public byte ReadByte(ushort address)
        {
            return Memory[address];
        }

        public ushort ReadOpcode(ushort address)
        {
            return (ushort)(ReadByte(address) << 8 | ReadByte(++address));
        }

        public void WriteByte(ushort address, byte value)
        {
            if (address < MemoryRomStart)
                throw new ArgumentOutOfRangeException("Tried to write to reserved memory!");
            else
                Memory[address] = value;
        }
    }
}
