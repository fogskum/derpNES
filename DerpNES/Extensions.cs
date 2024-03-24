namespace DerpNES;

public static class Extensions
{
    public static string ToHex( this u16 value ) => $"0x{value.ToString("X4")}";
    
    public static string ToHex( this u8 value ) => $"0x{value.ToString("X2")}";

    public static string ToBinary( this u8 value ) => Convert.ToString( value, 2 );
    
    public static string ToBinary( this u16 value ) => Convert.ToString( value, 2 );
}