namespace DerpNES;

public class Memory : IBus
{
    readonly uint _addressRange = 0xFFFF;

    u8[] ram = new u8[1024 * 64];

    public Memory()
    {
        for(int i = 0; i < ram.Length; i++)
        {
            ram[i] = 0;
        }
    }

    bool InAddressRange( u16 address ) => address <= _addressRange;

    public void Write(u16 address, u8 data )
    {
        if (InAddressRange( address ))
        {
            ram[address] = data;
        }
    }

    public u8 Read( u16 address, bool readOnly = false )
    {
        if (InAddressRange( address ))
        {
            return ram[address];
        }
        return 0;
    }
}
