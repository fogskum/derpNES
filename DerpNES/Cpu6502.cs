using System.Collections.Immutable;

namespace DerpNES;

// & = extract bit (and)
// | = set bit (or)
// << >> shift to correct location
// ~& = clear bit (not)
// ^ = toggle bit (xor)

/// <summary>
/// Emulates the MOS Technology 6502
/// </summary>
public sealed partial class Cpu6502
{
    internal enum InterruptType
    {
        NonMaskableInterrupt,
        InterruptRequest,
        Reset
    }

    // bits of the status register
    public enum StatusFlag : uint
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

    // We can interregate the CPU state - was the last result zero? Has there been a carry operation?
    // We can also enable/disble interrupts.
    // bit, but we just a Uint8
    uint FlagStatus = 0x00;

    // registers

    uint A = 0x00;
    uint X = 0x00;
    uint Y = 0x00;

    // Points to an address somewhere in the memory (bus)
    // Incremented/decremented as we pull things from the stack
    uint StackPointer = 0x00;

    // Stores the address of the next progran uint
    // Increases with each clock or can be directly set in a branch to jump to
    // Different parts of the program, like a if-statement
    uint ProgramCounter = 0x0000;


    /// <summary>
    /// Perform one clock cycle
    /// </summary>
    public void Clock()
    {
        if (_cycles == 0)
        {
            // get opcode for current location => will increase PC
            _currentInstruction = NextByte();
            _cycles = _instructions[_currentInstruction].Cycles;
            var cycle1 = _instructions[_currentInstruction].AddressMode();
            var cycle2 = _instructions[_currentInstruction].Operate();
            // check if we need additional cycles
            _cycles += (cycle1 & cycle2);
        }
        _cycles--;
    }

    // these are interrupt signals
    // runs async but will finish the current instruction before interrupt
    void Reset() { }
    void InterruptRequest() { }
    void NonMaskableInterrupt() { }

    uint FetchData()
    {
        if (_instructions[_currentInstruction].AddressMode != Implied)
        {
            _fetchedData = ReadByte( _addressAbsolute );
        }
        return _fetchedData;
    }

    uint _fetchedData = 0x00;
    uint _addressAbsolute = 0x0000;
    uint _address_rel = 0x0000;
    uint _cycles = 0;

    Dictionary<uint, Instruction> _instructions;
    uint _currentInstruction;

    private readonly IBus bus;
    private readonly Emulator emulator;

    public Cpu6502( Emulator emulator )
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

        this.emulator = emulator;
        this.bus = new Bus();
    }

    uint NextByte() => ReadByte( ProgramCounter++ );
    uint NextWord() => NextByte() | (NextByte() << 8);

    public bool GetFlag( StatusFlag flagBit )
        => (FlagStatus & (uint)flagBit) > 0 ? true : false;

    public void SetFlag( StatusFlag flagBit, bool set )
    {
        if (set)
        {
            FlagStatus |= (uint)flagBit;
        }
        else
        {
            // clear flag
            FlagStatus &= ~(uint)flagBit;
        }
    }

    public uint ReadByte( uint address ) => bus.Read( address );
    public void WriteByte( uint address, uint data ) => bus.Write( address, data );
}
