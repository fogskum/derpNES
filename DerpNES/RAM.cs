namespace DerpNES;

public class RAM : IBus
{
    const u16 size = 2048;
    readonly u8[] ram = new u8[size];

    public u8 Read( u16 address, bool readOnly = false )
    {
        address %= size;
        return ram[address];
    }

    public void Write( u16 address, u8 data )
    {
        address %= size;
        ram[address] = data;
    }
}
