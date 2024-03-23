namespace DerpNES;

public interface IBus
{
    void Write( u16 address, u8 data );

    u8 Read( u16 address, bool readOnly = false );
}
