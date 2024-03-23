namespace DerpNES;

public class Emulator
{
    public readonly Cpu6502 Cpu;

    public Emulator(Cpu6502 cpu)
    {
        Cpu = cpu;
    }

    public void Reset() => Cpu.Reset();

    public void Step() => Cpu.Step();
}
