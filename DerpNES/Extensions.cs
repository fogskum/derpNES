namespace DerpNES;

public static class UIntExtensions
{
    public static string ToHex( this u16 value ) => $"0x{value.ToString("X4")}";
    
    public static string ToHex( this u8 value ) => $"0x{value.ToString("X2")}";

    public static string ToBinary( this uint value ) => Convert.ToString( value, 2 );
    
    public static string ToBinary( this u8 value ) => Convert.ToString( value, 2 );

    public static uint ShiftRight( this uint value, int pos )
        => value >> pos;

    public static uint ShiftLeft( this uint value, int pos )
        => value << pos;

    public static uint ClearBit( this uint value, int pos )
        => (uint)(value & ~(1 << pos));
}