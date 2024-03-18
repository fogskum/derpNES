namespace DerpNES;

public class Bus : IBus
{
    public ICpu Cpu { get; } = null!;

    UInt8[] ram = new UInt8[1024 * 64];

    bool InRange( UInt16 address ) => address >= 0 && address <= 0xFFFF;

    public void Write( UInt16 address, UInt8 data )
    {
        if (InRange( address ))
        {
            ram[address] = data;
        }
    }

    public UInt8 Read( UInt16 address, bool readOnly = false )
    {
        if (InRange( address ))
        {
            return ram[address];
        }
        return 0;
    }
}
