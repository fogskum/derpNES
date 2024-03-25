namespace DerpNES;

public partial class Cpu6502
{
    u8 Accumuator()
    {
        _byteToOperateOn = A;
        return 0;
    }

    // In absolute addressing mode, the next two bytes after the instruction
    // represent the 16-bit absolute address to operate on
    // Example: lowByte = 1011 0000, highByte = 1010 0000
    // highByte << 8 = 1010 0000 0000 0000
    // lowByte | highByte = 1010 0000 1011 0000 = 0xA0B0
    u8 Absolute()
    {
        u8 lowByte = NextByte();
        u8 highByte = NextByte();
        _operandAddress = (u16)(lowByte | highByte << 8);
        return 0;
    }

    // if the resulting address changes the page, an additional clock cycle is required
    u8 AbsoluteX()
    {
        u8 lowByte = NextByte();
        u8 highByte = NextByte();
        _operandAddress = (u16)(lowByte | highByte << 8);
        _operandAddress += X;

        if ((u16)(_operandAddress & 0xFF00) != (u16)(highByte << 8))
        {
            return 1;
        }
        return 0;
    }

    // if the resulting address changes the page, an additional clock cycle is required
    u8 AbsoluteY()
    {
        u8 lowByte = NextByte();
        u8 highByte = NextByte();
        _operandAddress = (u16)(lowByte | highByte << 8);
        _operandAddress += Y;

        if ((u16)(_operandAddress & 0xFF00) != (u16)(highByte << 8))
        {
            return 1;
        }
        return 0;
    }

    u8 Immediate()
    {
        _operandAddress = PC;
        return 0;
    }

    u8 Implied()
    {
        _byteToOperateOn = A;
        return 0;
    }

    u8 Indirect()
    {
        var lowByte = NextByte();
        var highByte = NextByte();
        u16 ptr = (u16)((highByte << 8) | lowByte);
        if (ptr == 0x00FF)
        {
            // simulate page boundary hardware bug
            _operandAddress = (u16)((ReadByte( (u16)(ptr & 0xFF00) ) << 8) | ReadByte( (u16)(ptr + 0) ));
        }
        else
        {
            // behave normally
            _operandAddress = (u16)((ReadByte( (u16)(ptr + 1) ) << 8) | ReadByte( (u16)(ptr + 0) ));
        }
        return 0;
    }

    u8 IndirectX() => throw new NotImplementedException();
    u8 IndirectY() => throw new NotImplementedException();

    u8 Relative() => throw new NotImplementedException();

    // The zero page is the first 256 bytes of the CPU's address space, from addresses 0x0000 to 0x00FF
    // The most significant byte of a zero page address will always be 0x00.
    u8 ZeroPage()
    {
        _operandAddress = (u16)(NextByte() & 0x00FF);

        return 0;
    }

    // Indexed variation of the ZeroPage with X as offset.
    // The address of the byte to operate on is found by
    // adding the current value in the X or Y register to
    // the address specified in the instruction.
    // Useful when you want to loop through some part of the address space.
    u8 ZeroPageX()
    {
        // mask with 0x00FF to handle wrap
        _operandAddress = (u16)((NextByte() + X) & 0x00FF);

        return 0;
    }

    // Indexed variation of the ZeroPage with Y as offset.
    // The address of the byte to operate on is found by
    // adding the current value in the X or Y register to
    // the address specified in the instruction.
    // Useful when you want to loop through some part of the address space.
    u8 ZeroPageY()
    {
        _operandAddress = (u16)((NextByte() + Y) & 0x00FF);

        return 0;
    }
}
