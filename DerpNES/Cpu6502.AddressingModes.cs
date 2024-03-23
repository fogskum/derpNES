namespace DerpNES;

public partial class Cpu6502
{
    // addressing modes: https://blogs.oregonstate.edu/ericmorgan/2022/01/21/6502-addressing-modes/

    /// <summary>
    /// These are instructions that operate against the currently value in the accumulator.
    /// They require two cycles to complete.
    /// </summary>
    /// <returns></returns>
    u8 Accumuator() => throw new NotImplementedException();

    /// <summary>
    /// With this addressing mode, two additional uints are read after the op code address that make up a word.
    /// This word defines a specific address against which to operate.
    /// These instructions can additionally be incremented by the value in the X or Y indexes.
    /// These instructions require 4 CPU cycles to complete. When indexing against the address, 
    /// if incrementing the absolute address causes the address to cross pages (changes the value of the high uint) 
    /// then an additional cycle is required.
    /// </summary>
    /// <returns></returns>
    u8 Absolute() => throw new NotImplementedException();
    u8 AbsoluteX() => throw new NotImplementedException();
    u8 AbsoluteY() => throw new NotImplementedException();

    /// <summary>
    /// With this addressing mode 1 additional uint is read following the instruction.
    /// This uint represents a specific value against which to operate.
    /// This addressing mode requires 2 cycles.
    /// </summary>
    /// <returns></returns>
    u8 Immediate()
    {
        _addressAbsolute = PC;
        return 0;
    }

    /// <summary>
    /// The implied addressing mode basically means there is no addressing mode.
    /// These are instructions that operate against specific areas of memory.
    /// These are op codes like BRK and those that change processer flags directly.
    /// This addressing mode requires 2 cycles to complete.
    /// </summary>
    /// <returns></returns>
    u8 Implied()
    {
        _fetchedData = A;
        return 0;
    }

    // Address Mode: Indirect
    // The supplied 16-bit address is read to get the actual 16-bit address. This is
    // instruction is unusual in that it has a bug in the hardware! To emulate its
    // function accurately, we also need to emulate this bug. If the low byte of the
    // supplied address is 0xFF, then to read the high byte of the actual address
    // we need to cross a page boundary. This doesnt actually work on the chip as 
    // designed, instead it wraps back around in the same page, yielding an 
    // invalid actual address
    u8 Indirect()
    {
        var low = NextByte();
        var hight = NextByte();
        u16 ptr = (u16)((hight << 8) | low);
        if (ptr == 0x00FF)
        {
            // Simulate page boundary hardware bug
            _addressAbsolute = (u16)((ReadByte( (u16)(ptr & 0xFF00) ) << 8) | ReadByte( (u16)(ptr + 0) ));
        }
        else
        {
            // Behave normally
            _addressAbsolute = (u16)((ReadByte( (u16)(ptr + 1) ) << 8) | ReadByte( (u16)(ptr + 0) ));
        }
        return 0;
    }

    /// <summary>
    /// These are similar to the previous indirect, except a single uint is read following the instruction.
    /// This is then added to the zero page, and offset by the X or Y register.
    /// The word located at the calculated address is then returned.
    /// Indirect X increments without a carry operation, while Y is incremented and can carry.
    /// Indirect X uses 6 CPU cycles, while indirect Y uses 5. If indirect Y carries it uses 6 CPU cycles.
    /// </summary>
    /// <returns></returns>
    u8 IndirectX() => throw new NotImplementedException();
    u8 IndirectY() => throw new NotImplementedException();

    /// <summary>
    /// This addressing mode is used for branch operations.
    /// When used, a single uint following the instruction is read.
    /// This uint represents a signed integer that offsets from the current program counter.
    /// This instruction requires 2 cycles.
    /// If the offset causes the address to cross into a new page an additional cycle is required (for 3 total).
    /// </summary>
    /// <returns></returns>
    u8 Relative() => throw new NotImplementedException();

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
    u8 ZeroPage()
    {
        _addressAbsolute = (u8)(NextByte() & 0x00FF);

        return 0;
    }

    u8 ZeroPageX() => throw new NotImplementedException();
    u8 ZeroPageY() => throw new NotImplementedException();
}
