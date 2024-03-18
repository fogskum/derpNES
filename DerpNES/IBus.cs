namespace DerpNES;

internal interface IBus
{
    void Write( UInt16 address, UInt8 data );

    UInt8 Read( UInt16 address, bool readOnly = false );
}
