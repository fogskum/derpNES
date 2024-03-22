namespace DerpNES;

// https://masswerk.at/6502/6502_instruction_set.html#LSR
internal partial class Olc6502
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
    uint AND() => 0;

    uint BRK() => 0;

    uint ORA() => 0;
}
