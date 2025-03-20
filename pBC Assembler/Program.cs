using pVM.Shared;

Dictionary<string, string> argsParsed = new Dictionary<string, string>();
for(int i = 0; i < args.Length; i++)
{
    argsParsed.Add(args[i], args[++i]);
}

string input = argsParsed["-i"];
string output = argsParsed["-o"];

string[] lines = File.ReadAllLines(input);
List<byte> bytes = new List<byte>();
foreach(string line in lines)
{
    if (line.StartsWith(';'))
        continue;
    string[] tokens = line.Split(' ');
    List<byte> byteLine = new List<byte>();
    int i = 0;
    foreach(string token in tokens)
    {
        switch (token.ToUpper())
        {
            case "MOV":
                byteLine.Add((byte)Opcodes.Move);
                break;
            case "NOP":
                byteLine.Add((byte)Opcodes.NOP);
                break;
            case "ADD":
                byteLine.Add((byte)Opcodes.Add);
                break;
            case "PUSH":
                byteLine.Add((byte)Opcodes.Push);
                break;
            case "POP":
                byteLine.Add((byte)Opcodes.Pop);
                break;
            case "CALL":
                byteLine.Add((byte)Opcodes.CallGameFunc);
                break;
            case "PRINTTXT":
                byteLine.Add((byte)Opcodes.DebugPrint);
                break;
            case "PRINTNUM":
                byteLine.Add((byte)Opcodes.DebugPrintNum);
                break;
            case "HALT":
                byteLine.Add((byte)Opcodes.Halt);
                break;
            case "JMP":
                byteLine.Add((byte)Opcodes.Jump);
                break;
            case "JMC":
                byteLine.Add((byte)Opcodes.JumpIfCarry);
                break;
            case "JMZ":
                byteLine.Add((byte)Opcodes.JumpIfZero);
                break;
            case "JME":
                byteLine.Add((byte)Opcodes.JumpIfError);
                break;
            case "JMN":
                byteLine.Add((byte)Opcodes.JumpIfNegative);
                break;
            case "JMO":
                byteLine.Add((byte)Opcodes.JumpIfOverflow);
                break;
            case "STC":
                byteLine.Add((byte)Opcodes.SetCarry);
                break;
            case "STE":
                byteLine.Add((byte)Opcodes.SetError);
                break;
            case "STO":
                byteLine.Add((byte)Opcodes.SetOverflow);
                break;
            case "STN":
                byteLine.Add((byte)Opcodes.SetNegative);
                break;
            case "STZ":
                byteLine.Add((byte)Opcodes.SetZero);
                break;
            case "SMEM":
                byteLine.Add((byte)Opcodes.SetMemory);
                break;
            case "GMEM":
                byteLine.Add((byte)Opcodes.GetMemory);
                break;
            case "PAUSE":
                byteLine.Add((byte)Opcodes.Pause);
                break;
            case "INC":
                byteLine.Add((byte)Opcodes.Increment);
                break;
            case "DEC":
                byteLine.Add((byte)Opcodes.Decrease);
                break;
            case "SUB":
                byteLine.Add((byte)Opcodes.Subtract);
                break;
            case "MUL":
                byteLine.Add((byte)Opcodes.Multiply);
                break;
            case "DIV":
                byteLine.Add((byte)Opcodes.Divide);
                break;
            case "CMP":
                byteLine.Add((byte)Opcodes.Compare);
                break;
            case "PUSHR":
                byteLine.Add((byte)Opcodes.PushRegister);
                break;
            default:
                if (token.StartsWith('b') && byte.TryParse(token.Substring(1), out byte res))
                {
                    byteLine.Add(res);
                    break;
                }
                else if (token.StartsWith('h'))
                {
                    string substring = token.Substring(1);
                    if (substring.Length == 2)
                        byteLine.Add(byte.Parse(substring, System.Globalization.NumberStyles.HexNumber));
                    if (substring.Length == 4)
                        byteLine.AddRange(BitConverter.GetBytes(uint.Parse(substring, System.Globalization.NumberStyles.HexNumber)));
                }
                else if (uint.TryParse(token, out uint res2))
                {
                    byteLine.AddRange(BitConverter.GetBytes(res2));
                    break;
                }
                break;
        }
        i++;
    }
    bytes.AddRange(byteLine);
}


File.WriteAllBytes(output, bytes.ToArray());