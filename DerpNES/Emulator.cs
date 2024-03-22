namespace DerpNES;

public class Emulator
{
    readonly Cpu6502 cpu;

    public Emulator()
    {
        this.cpu = new Cpu6502();
    }

    public void Run()
    {
        cpu.Reset();
        cpu.Execute();
    }
}
