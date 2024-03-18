using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    /// Break / interrupt
    /// </summary>
    /// <returns></returns>
    UInt8 BRK()
    {

        return 0;
    }
}
