namespace DerpNES;

public class RAM : IBus
{
    const uint size = 2048;
    readonly uint[] ram = new uint[size];

    public uint Read( uint address, bool readOnly = false )
    {
        address %= 2048;
        return ram[address];
    }

    public void Write( uint address, uint data )
    {
        address %= size;
        ram[address] = data;
    }
}
