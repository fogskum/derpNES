namespace DerpNES;

// https://masswerk.at/6502/6502_instruction_set.html#LSR
public partial class Cpu6502
{
    void SetFlagZN()
    {
        SetFlag( StatusFlag.Zero, A == 0 );
        SetFlag( StatusFlag.Negative, (A & (u8)StatusFlag.Negative) > 0 );
    }

    u8 Illegal()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Loads a byte of memory into the accumulator
    /// </summary>
    /// <returns></returns>
    u8 LDA()
    {
        A = ReadByte( _operandAddress );
        SetFlagZN();

        return 0;
    }

    /// <summary>
    /// Add with carry
    /// </summary>
    u8 ADC() => throw new NotImplementedException();

    /// <summary>
    /// AND Memory with Accumulator
    /// </summary>
    /// <returns></returns>
    u8 AND()
    {
        A &= FetchData();
        SetFlag( StatusFlag.Zero, A == 0 );
        // 1000 0000
        SetFlag( StatusFlag.Negative, (A & (u8)StatusFlag.Negative) > 0 );

        return 1; // additional clock cycle
    }

    u8 BRK() => 0;

    u8 ORA() => 0;
}
