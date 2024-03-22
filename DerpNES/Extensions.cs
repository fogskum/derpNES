namespace DerpNES;

public static class UIntExtensions
{
    public static string ToBinary( this uint value ) => Convert.ToString( value, 2 );

    public static uint ShiftRight( this uint value, int pos )
        => value >> pos;

    public static uint ShiftLeft( this uint value, int pos )
        => value << pos;

    public static uint ClearBit( this uint value, int pos )
        => (uint)(value & ~(1 << pos));
}