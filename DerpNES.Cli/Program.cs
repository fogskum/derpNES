using DerpNES;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var services = new ServiceCollection();
services.AddLogging( builder => builder.AddConsole() );
services.AddDerpNES();

using var provider = services.BuildServiceProvider();
var emulator = provider.GetRequiredService<Emulator>();
emulator.Reset();

var isRunning = true;
while (isRunning)
{
    Console.WriteLine( "Awaiting input..." );
    var input = Console.ReadKey();
    switch (input.Key)
    {
        case ConsoleKey.Spacebar: emulator.Step(); break;
        case ConsoleKey.Escape: isRunning = false; break;
    }
}
