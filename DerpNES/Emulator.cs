namespace DerpNES;

public class Emulator
{
    readonly Cpu6502 cpu = new Cpu6502();
    readonly IBus bus = new Bus();

    public Emulator()
    {
        cpu.ConnectBus(bus);
    }

    public void Run()
    {
        cpu.Clock();
    }
}
