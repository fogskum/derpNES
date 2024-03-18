namespace DerpNES;

internal class Bus : IBus
{
    (UInt16 Start, UInt16 End) _addressRange = new(0x00, 0xFFFF);

    UInt8[] ram = new UInt8[1024 * 64];

    public Bus()
    {
        
    }

    bool InRange( UInt16 address ) => address >= _addressRange.Start && address <= _addressRange.End;

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
