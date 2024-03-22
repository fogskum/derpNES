namespace DerpNES;

// https://masswerk.at/6502/6502_instruction_set.html#LSR
internal partial class Olc6502
{
    uint _opcode = 0x00;

    UInt8 Illegal()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Add with carry
    /// </summary>
    UInt8 ADC() => throw new NotImplementedException();

    /// <summary>
    /// AND Memory with Accumulator
    /// </summary>
    /// <returns></returns>
    UInt8 AND() => 0;

    UInt8 BRK() => 0;

    UInt8 ORA() => 0;
}
