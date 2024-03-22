namespace DerpNES;

public class Emulator
{
    readonly Cpu6502 cpu;

    public Emulator(string? path)
    {
        this.cpu = new Cpu6502(this);
    }

    public void Run()
    {
        cpu.Clock();
    }
}
