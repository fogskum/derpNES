namespace DerpNES;

public class Emulator
{
    readonly Olc6502 cpu = new Olc6502();
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
