using System.Collections.Immutable;

namespace DerpNES;

/// <summary>
/// Emulates the MOS Technology 6502
/// </summary>
internal sealed partial class Olc6502
{
    internal enum InterruptType
    {
        NonMaskableInterrupt,
        InterruptRequest,
        Reset
    }

    // bits of the status register
    internal enum StatusFlag : UInt8
    {
        Carry = (1 << 0),
        Zero = (1 << 1),
        DisableInterrupts = (1 << 2),
        DecimalMode = (1 << 3),
        Break = (1 << 4),
        Unused = (1 << 5),
        Overflow = (1 << 6),
        Negative = (1 << 7),
    }

    // registers

    UInt8 A = 0x00;
    UInt8 X = 0x00;
    UInt8 Y = 0x00;

    // Points to an address somewhere in the memory (bus)
    // Incremented/decremented as we pull things from the stack
    UInt8 StackPointer = 0x00;
    
    // Stores the address of the next progran byte
    // Increases with each clock or can be directly set in a branch to jump to
    // Different parts of the program, like a if-statement
    UInt16 ProgramCounter = 0x0000;
    
    // We can interregate the CPU state - was the last result zero? Has there been a carry operation?
    // We can also enable/disble interrupts.
    // bit, but we just a Uint8
    UInt8 Status = 0x00;

    /// <summary>
    /// Perform one clock cycle
    /// </summary>
    public void Clock()
    {
        if (_cycles == 0)
        {
            _opcode = NextByte();
            _cycles = _instructions[_opcode].Cycles;
            uint cycle1 = _instructions[_opcode].AddressMode();
            uint cycle2 = _instructions[_opcode].Operate();
            _cycles += (cycle1 & cycle2);
        }
        _cycles--;
    }

    // these are interrupt signals
    // runs async but will finish the current instruction before interrupt
    void Reset() { }
    void InterruptRequest() { }
    void NonMaskableInterrupt() { }

    UInt8 FetchData() => throw new NotImplementedException();
    UInt8 _fetchedData = 0x00;
    UInt16 _address_abs = 0x0000;
    UInt16 _address_rel = 0x0000;
    uint _cycles = 0;

    Dictionary<uint, Instruction> _instructions;
    uint _currentInstruction;

    IBus Bus { get; set; } = null!;

    public Olc6502()
    {
        var instructions = ImmutableArray.Create( 
         new Instruction( Opcode: 0x00, Operate: BRK, AddressMode: Immediate, Cycles: 7 ),
         new Instruction( Opcode: 0x09, Operate: ORA, AddressMode: Immediate, Cycles: 2 ),
         new Instruction( Opcode: 0x05, Operate: ORA, AddressMode: ZeroPage, Cycles: 3 ),
         new Instruction( Opcode: 0x15, Operate: ORA, AddressMode: ZeroPageX, Cycles: 4 ),
         new Instruction( Opcode: 0x0D, Operate: ORA, AddressMode: Absolute, Cycles: 4 ),
         new Instruction( Opcode: 0x1D, Operate: ORA, AddressMode: AbsoluteX, Cycles: 4 ),
         new Instruction( Opcode: 0x19, Operate: ORA, AddressMode: AbsoluteY, Cycles: 4 ),
         new Instruction( Opcode: 0x01, Operate: ORA, AddressMode: IndirectX, Cycles: 6 ),
         new Instruction( Opcode: 0x11, Operate: ORA, AddressMode: IndirectY, Cycles: 5 )
        );
        _instructions = instructions.ToDictionary( k => k.Opcode, v => v );
    }

    public void ConnectBus( IBus bus )
    {
        this.Bus = bus;
    }

    UInt8 NextByte() => ReadByte( ProgramCounter++ );

    UInt8 GetFlag( StatusFlag flag ) => throw new NotImplementedException();
    void SetFlag( StatusFlag flag, bool v ) => throw new NotImplementedException();

    public UInt8 ReadByte( UInt16 address ) => Bus.Read( address );
    public void WriteByte( UInt16 address, UInt8 data ) => Bus.Write( address, data );
}
