using pVM.Shared;
using pVM;

uint health = 32;
Console.WriteLine("Current Health: " + health);
/*VM vm = new VM(new byte[]
{
    (byte)Opcodes.Push,          0x01, 0, 0, 0,
    (byte)Opcodes.Push,          0x05, 0, 0, 0,
    (byte)Opcodes.Add,
    (byte)Opcodes.DebugPrintNum, 0x01, 0, 0, 0,
    (byte)Opcodes.Push,          0x48, 0, 0, 0,
    (byte)Opcodes.Push,          0x65, 0, 0, 0,
    (byte)Opcodes.Push,          0x6C, 0, 0, 0,
    (byte)Opcodes.Push,          0x6C, 0, 0, 0,
    (byte)Opcodes.Push,          0x6F, 0, 0, 0,
    (byte)Opcodes.DebugPrint,    0x05, 0, 0, 0,
    (byte)Opcodes.Move,          0x01, 0x0F, 0, 0, 0,
    (byte)Opcodes.CallGameFunc,  0x01, 0, 0, 0,
    (byte)Opcodes.CallGameFunc,  0x00, 0, 0, 0,
    (byte)Opcodes.Halt,
});*/

VM vm = new("app.pbc");
vm.RegisterGameFunc(0, () =>
{
    vm.stack.Push(health);
});
vm.RegisterGameFunc(1, () =>
{
    health = vm.stack.Pop();
    if(health <= 0)
    {
        health = 0;
        vm.zero = true;
    }
    Console.WriteLine("Current Health: " + health);
}); 

vm.Run();

vm.PrintStats();