namespace DerpNES;

internal class Bus : IBus
{
    readonly (uint Start, uint End) _addressRange = new(0x0000, 0xFFFF);

    uint[] ram = new uint[1024 * 64];

    public Bus()
    {
        
    }

    bool InAddressRange( uint address ) => address >= _addressRange.Start && address <= _addressRange.End;

    public void Write( uint address, uint data )
    {
        if (InAddressRange( address ))
        {
            ram[address] = data;
        }
    }

    public uint Read( uint address, bool readOnly = false )
    {
        if (InAddressRange( address ))
        {
            return ram[address];
        }
        return 0;
    }
}
