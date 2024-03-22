namespace DerpNES;

internal interface IBus
{
    void Write( uint address, uint data );

    uint Read( uint address, bool readOnly = false );
}
