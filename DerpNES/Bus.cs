namespace DerpNES;

public class Bus : IBus
{
    readonly uint _addressRange = 0xFFFF;

    uint[] ram = new uint[1024 * 64];

    public Bus()
    {
        
    }

    bool InAddressRange( uint address ) => address <= _addressRange;

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
