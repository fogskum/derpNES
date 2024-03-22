namespace DerpNES;

public class Emulator
{
    readonly Cpu6502 cpu = null!;
    readonly IBus bus = new Bus();

    public Emulator()
    {
        bus = new Bus();
        cpu = new Cpu6502(bus);
    }

    public void Run()
    {
        cpu.Clock();
    }
}
