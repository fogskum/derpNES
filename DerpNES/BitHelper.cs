namespace DerpNES;

public static class BitHelper
{
    /// <summary>
    // value = 0000 1001 = 9
    // bitPos = 4
    // temp = 0000 0001 << 3 = 0000 1000 = 8
    // 0000 1001
    //&0000 1000
    // ---------
    // 0000 1000 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="bitPos"></param>
    /// <returns></returns>
    public static bool IsBitSet(this u16 value, u8 bitPos)
    {
        var temp = 1 << (bitPos-1);
        var result = value & temp;
        return temp == result;
    }

    /// <summary>
    // value = 0000 1001 = 9
    // bitPos = 4
    // temp = 0000 0001 << 3 = 0000 1000 = 8
    // 0000 1001
    //&0000 1000
    // ---------
    // 0000 1000 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="bitPos"></param>
    /// <returns></returns>
    public static bool IsBitSet( this u8 value, u8 bitPos )
    {
        var temp = 1 << (bitPos - 1);
        var result = value & temp;
        return temp == result;
    }
}