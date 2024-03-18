namespace DerpNES;

/// <summary>
/// Emulates the olc6502 CPU
/// </summary>
public class Olc6502 : ICpu
{
    IBus Bus { get; set; } = null!;

    public void ConnectBus( IBus bus )
    {
        this.Bus = bus;
    }
}
