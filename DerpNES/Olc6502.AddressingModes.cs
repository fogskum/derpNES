namespace DerpNES;

internal partial class Olc6502
{
    // addressing modes: https://blogs.oregonstate.edu/ericmorgan/2022/01/21/6502-addressing-modes/

    /// <summary>
    /// These are instructions that operate against the currently value in the accumulator.
    /// They require two cycles to complete.
    /// </summary>
    /// <returns></returns>
    UInt8 Accumuator() => throw new NotImplementedException();

    /// <summary>
    /// With this addressing mode, two additional bytes are read after the op code address that make up a word.
    /// This word defines a specific address against which to operate.
    /// These instructions can additionally be incremented by the value in the X or Y indexes.
    /// These instructions require 4 CPU cycles to complete. When indexing against the address, 
    /// if incrementing the absolute address causes the address to cross pages (changes the value of the high byte) 
    /// then an additional cycle is required.
    /// </summary>
    /// <returns></returns>
    UInt8 Absolute() => throw new NotImplementedException();
    UInt8 AbsoluteX() => throw new NotImplementedException();
    UInt8 AbsoluteY() => throw new NotImplementedException();

    /// <summary>
    /// With this addressing mode 1 additional byte is read following the instruction.
    /// This byte represents a specific value against which to operate.
    /// This addressing mode requires 2 cycles.
    /// </summary>
    /// <returns></returns>
    UInt8 Immediate()
    {
        _address_abs = NextByte();
        return 0;
    }

    /// <summary>
    /// The implied addressing mode basically means there is no addressing mode.
    /// These are instructions that operate against specific areas of memory.
    /// This are op codes like BRK and those that change processer flags directly.
    /// This addressing mode requires 2 cycles to complete.
    /// </summary>
    /// <returns></returns>
    UInt8 Implied()
    {
        _fetchedData = Accumulator;
        return 0;
    }

    /// <summary>
    /// These instructions return a value located at a specific address.
    /// When this addressing mode is run, a word is read following the instruction which represents a memory location.
    /// This addressing mode then reads the value at that location and returns this value. Indirect requires 5 cycles.
    /// </summary>
    /// <returns></returns>
    UInt8 Indirect() => throw new NotImplementedException();

    /// <summary>
    /// These are similar to the previous indirect, except a single byte is read following the instruction.
    /// This is then added to the zero page, and offset by the X or Y register.
    /// The word located at the calculated address is then returned.
    /// Indirect X increments without a carry operation, while Y is incremented and can carry.
    /// Indirect X uses 6 CPU cycles, while indirect Y uses 5. If indirect Y carries it uses 6 CPU cycles.
    /// </summary>
    /// <returns></returns>
    UInt8 IndirectX() => throw new NotImplementedException();
    UInt8 IndirectY() => throw new NotImplementedException();

    /// <summary>
    /// This addressing mode is used for branch operations.
    /// When used, a single byte following the instruction is read.
    /// This byte represents a signed integer that offsets from the current program counter.
    /// This instruction requires 2 cycles.
    /// If the offset causes the address to cross into a new page an additional cycle is required (for 3 total).
    /// </summary>
    /// <returns></returns>
    UInt8 Relative() => throw new NotImplementedException();

    /// <summary>
    /// This addressing mode targets values following a beginning of the memory (zeropage), and are very quick to run. 
    /// When run a byte is read following the instruction representing an offset from the zero page.
    /// Zeropage X and Y additionally add the X or Y index (as applicable) to this offset.
    /// If the additional of the index causes the instruction to carry,
    /// the carry is dropped and the returned address instead just wraps back around to start at zero.
    /// They are mainly used to access frequently referenced variables.
    /// Zeropage requires 3 CPU cycles, while Zeropage X and Zeropage Y require 4.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    UInt8 ZeroPage() => throw new NotImplementedException();
    UInt8 ZeroPageX() => throw new NotImplementedException();
    UInt8 ZeroPageY() => throw new NotImplementedException();
}
