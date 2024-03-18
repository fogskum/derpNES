namespace DerpNES;

public interface IBus
{
    void Write( UInt16 address, UInt8 data );
}
