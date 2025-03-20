
namespace pVM.Shared
{
    public enum Opcodes : byte
    {
        NOP = 0x00,
        Push = 0x01,
        Pop = 0x02,
        Move = 0x03,
        Add = 0x04, // Pop 2 values and add them then push them
        DebugPrint = 0x05, // Pop {length: 4} values, merge them into an uint, convert them to ASCII string and print them 
        Halt = 0x06,
        DebugPrintNum = 0x07, // Pop {length: 4} values, merge them into an uint and print them
        CallGameFunc = 0x08,
        Jump = 0x09,
        SetCarry = 0x0A,
        JumpIfCarry = 0x0B,
        SetMemory = 0x0C,
        GetMemory = 0x0D,
        Multiply = 0x0E,
        Divide = 0x0F,
        Increment = 0x10,
        Decrease = 0x11,
        JumpIfZero = 0x12,
        JumpIfNegative = 0x13,
        JumpIfOverflow = 0x14,
        JumpIfError = 0x15,
        Pause = 0x16,
        SetOverflow = 0x17,
        SetError = 0x18,
        SetNegative = 0x19,
        SetZero = 0x1A,
        Subtract = 0x1B,
        Compare = 0x1C,
        PushRegister = 0x1D,
    }
}