using DerpNES;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddDerpNES();

using var provider = services.BuildServiceProvider();
var emulator = provider.GetRequiredService<Emulator>();
emulator.Run();