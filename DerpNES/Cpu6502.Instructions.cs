namespace DerpNES;

// https://masswerk.at/6502/6502_instruction_set.html#LSR
public partial class Cpu6502
{
    uint Illegal()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Add with carry
    /// </summary>
    uint ADC() => throw new NotImplementedException();

    /// <summary>
    /// AND Memory with Accumulator
    /// </summary>
    /// <returns></returns>
    public uint AND()
    {
        A &= FetchData();
        SetFlag(StatusFlag.Zero, A == 0 );
        // 1000 0000
        SetFlag( StatusFlag.Negative, (A & (uint)StatusFlag.Negative) > 0 );
        
        return 1; // additional clock cycle
    }

    uint BRK() => 0;

    uint ORA() => 0;
}
