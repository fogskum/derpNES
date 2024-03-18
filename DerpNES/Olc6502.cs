using System.Collections.Immutable;

namespace DerpNES;

/// <summary>
/// Emulates the MOS Technology 6502
/// </summary>
internal sealed partial class Olc6502 : ICpu
{
    public enum InterruptType
    {
        NonMaskableInterrupt,
        InterruptRequest,
        Reset
    }

    // bits of the status register
    public enum StatusFlag : byte
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
    UInt8 Accumulator = 0x00;
    UInt8 X = 0x00;
    UInt8 Y = 0x00;
    UInt8 StackPointer = 0x00; // points to location on bus
    UInt16 ProgramCounter = 0x0000;
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
    uint _address_abs = 0x0000;
    UInt16 _address_rel = 0x0000;
    uint _cycles = 0;

    Dictionary<uint, Instruction> _instructions;
    uint _currentInstruction;

    IBus Bus { get; set; } = null!;

    public Olc6502()
    {
        var instructions = ImmutableArray.Create(
         new Instruction( Opcode: 0x00, Operate: BRK, AddressMode: Immediate, Cycles: 7 ) );

        _instructions = instructions.ToDictionary( k => k.Opcode, v => v );
    }

    public void ConnectBus( IBus bus )
    {
        this.Bus = bus;
    }

    uint NextByte() => ReadByte( ProgramCounter++ );

    UInt8 GetFlag( StatusFlag flag ) => throw new NotImplementedException();
    void SetFlag( StatusFlag flag, bool v ) => throw new NotImplementedException();

    public UInt8 ReadByte( UInt16 address ) => Bus.Read( address );
    public void WriteByte( UInt16 address, UInt8 data ) => Bus.Write( address, data );
}
