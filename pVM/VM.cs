using pVM.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace pVM
{
    public class VM
    {
        public byte[] memory;

        public Stack<uint> stack;
        private Stream loadedProgram;
        private BinaryReader reader;

        public bool carry = false;
        public bool zero = false;
        public bool overflow = false;
        public bool negative = false;
        public bool error = false;

        public bool executing = false;


        public Dictionary<byte, uint> Registers = new Dictionary<byte, uint>()
        {
            { 1, 0 }, // AA 
            { 2, 0 }, // AB
            { 3, 0 }, // AC
            { 4, 0 }, // AD
        };

        public Dictionary<uint, Action> Functions = new Dictionary<uint, Action>();


        public VM(byte[] bytecode, int memorySize = 4194304, int stackSize = 1024) // 4 MB
        {
            loadedProgram = new MemoryStream(bytecode);
            p_Init(memorySize, stackSize);
        }
        public VM(string path, int memorySize = 4194304, int stackSize = 1024) // 4MB
        {
            loadedProgram = new FileStream(path, FileMode.Open, FileAccess.Read);
            p_Init(memorySize, stackSize);
        }
        private void p_Init(int memSize, int stackSize)
        {
            reader = new BinaryReader(loadedProgram);
            memory = new byte[memSize];
            stack = new Stack<uint>(stackSize);
        }

        public void RegisterGameFunc(uint address, Action action)
        {
            Functions.Add(address, action);
        }


        public void Run()
        {
            executing = true;

            while (true)
            {
                if (!executing)
                    continue;

                byte opcode = reader.ReadByte();

                switch (opcode)
                {
                    case (byte)Opcodes.Jump:
                        Jump();
                        break;
                    case (byte)Opcodes.SetCarry:
                        carry = !carry;
                        break;
                    case (byte)Opcodes.SetZero:
                        zero = !zero;
                        break;
                    case (byte)Opcodes.SetError:
                        error = !error;
                        break;
                    case (byte)Opcodes.SetNegative:
                        negative = !negative;
                        break;
                    case (byte)Opcodes.SetOverflow:
                        overflow = !overflow;
                        break;
                    case (byte)Opcodes.JumpIfCarry:
                        if (carry)
                            Jump();
                        else
                            loadedProgram.Seek(4, SeekOrigin.Current);
                        break;
                    case (byte)Opcodes.JumpIfZero:
                        if (zero)
                            Jump();
                        else
                            loadedProgram.Seek(4, SeekOrigin.Current);
                        break;
                    case (byte)Opcodes.JumpIfError:
                        if (error)
                            Jump();
                        else
                            loadedProgram.Seek(4, SeekOrigin.Current);
                        break;
                    case (byte)Opcodes.JumpIfNegative:
                        if (negative)
                            Jump();
                        else
                            loadedProgram.Seek(4, SeekOrigin.Current);
                        break;
                    case (byte)Opcodes.JumpIfOverflow:
                        if (overflow)
                            Jump();
                        else
                            loadedProgram.Seek(4, SeekOrigin.Current);
                        break;
                    case (byte)Opcodes.Pause:
                        executing = false;
                        break;
                    case (byte)Opcodes.SetMemory:
                        SetMemory();
                        break;
                    case (byte)Opcodes.GetMemory:
                        SetMemory();
                        break;
                    case (byte)Opcodes.Pop:
                        Pop();
                        break;
                    case (byte)Opcodes.CallGameFunc:
                        Call();
                        break;
                    case (byte)Opcodes.Move:
                        Move();
                        break;
                    case (byte)Opcodes.Push:
                        Push();
                        break;
                    case (byte)Opcodes.Add:
                        Add();
                        break;
                    case (byte)Opcodes.Subtract:
                        Subtract();
                        break;
                    case (byte)Opcodes.Multiply:
                        Multiply();
                        break;
                    case (byte)Opcodes.Divide:
                        Divide();
                        break;
                    case (byte)Opcodes.Increment:
                        Increase();
                        break;
                    case (byte)Opcodes.Decrease:
                        Decrease();
                        break;
                    case (byte)Opcodes.Compare:
                        Compare();
                        break;
                    case (byte)Opcodes.DebugPrint:
                        Print(true);
                        break;
                    case (byte)Opcodes.DebugPrintNum:
                        Print(false);
                        break;
                    case (byte)Opcodes.Halt:
                        return;
                    case (byte)Opcodes.PushRegister:
                        PushRegister();
                        break;
                    case (byte)Opcodes.NOP:
                        break;
                    default:
                        Console.WriteLine("Unknown opcode");
                        return;
                }
            }
        }

        public void Jump()
        {
            uint pos = reader.ReadUInt32();
            loadedProgram.Seek(pos, SeekOrigin.Begin);
        }
        public void Pop()
        {
            byte reg = reader.ReadByte();
            if (reg == 0)
                throw new Exception("instruction Pop: Register 0 is stack");

            Registers[reg] = stack.Pop();
        }
        public void Push()
        {
            uint val = reader.ReadUInt32();
            stack.Push(val);
        }
        public void Move()
        {
            byte reg = reader.ReadByte();
            uint val = reader.ReadUInt32();
            if (reg == 0)
            {
                stack.Push(val);
                return;
            }
            Registers[reg] = val;
        }
        public void PushRegister()
        {
            byte reg = reader.ReadByte();
            stack.Push(Registers[reg]);
        }
        public void SetMemory()
        {
            uint address = reader.ReadUInt32();
            byte[] bytes = BitConverter.GetBytes(stack.Pop());
            memory[address] = bytes[0];
            memory[address + 1] = bytes[1];
            memory[address + 2] = bytes[2];
            memory[address + 3] = bytes[3];
        }

        public void GetMemory()
        {
            uint address = reader.ReadUInt32();
            byte[] bytes =
            {
                memory[address],
                memory[address + 1],
                memory[address + 2],
                memory[address + 3]
            };
            stack.Push(BitConverter.ToUInt32(bytes));
        }

        public void Call()
        {
            uint address = reader.ReadUInt32();
            Functions[address]();
        }

        public void Add()
        {
            uint val1 = stack.Pop();
            uint val2 = stack.Pop();
            stack.Push(val1 + val2);
        }
        public void Subtract()
        {
            uint val1 = stack.Pop();
            uint val2 = stack.Pop();
            stack.Push(val2 - val1);
        }
        public void Multiply()
        {
            uint val1 = stack.Pop();
            uint val2 = stack.Pop();
            stack.Push(val1 * val2);
        }
        public void Divide()
        {
            uint val1 = stack.Pop();
            uint val2 = stack.Pop();
            stack.Push(val2 / val1);
        }

        public void Increase()
        {
            uint val = stack.Pop();
            stack.Push(val++);
        }
        public void Decrease()
        {
            uint val = stack.Pop();
            stack.Push(val--);
        }
        public void Compare()
        {
            uint val1 = stack.Pop();
            uint val2 = stack.Pop();
            if (val1 == val2)
                Jump();
        }
        public void Print(bool text)
        {
            uint length = text ? reader.ReadUInt32() : 4;
            byte[] chars = new byte[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = (byte)stack.Pop();
            }
            Array.Reverse(chars);
            if (text)
                Console.WriteLine(Encoding.UTF8.GetString(chars));
            else
                Console.WriteLine(BitConverter.ToInt32(chars));

        }

        public void PrintStats()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=== pVM Debug Stats ===");
            Console.ResetColor();
            Console.WriteLine("Stack Length: " + stack.Count);
            Console.WriteLine("Bytecode Position: " + loadedProgram.Position);
            Console.WriteLine("Bytecode Length: " + loadedProgram.Length);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Press enter to exit.");
            Console.ResetColor();
            Console.ReadLine();
        }

    }
}
