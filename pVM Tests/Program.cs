using pVM.Shared;
using pVM;

uint health = 32;
Console.WriteLine("Current Health: " + health);

VM vm = new("app.pbc");
vm.RegisterGameFunc(0, () =>
{
    vm.stack.Push(health);
});
vm.RegisterGameFunc(1, () =>
{
    health = vm.stack.Pop();
    if (health <= 0)
    {
        health = 0;
        vm.zero = true;
    }
    Console.WriteLine("Current Health: " + health);
});

vm.Run();

vm.PrintStats();