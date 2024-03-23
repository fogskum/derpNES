using Microsoft.Extensions.Logging;
using System.Collections.Immutable;
using System.Text;

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
    public enum StatusFlag : u8
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
    u8 FlagStatus;

    // registers

    public u8 A { get; private set; }
    public u8 X { get; private set; }
    public u8 Y { get; private set; }

    // Stack pointer
    // Points to an address somewhere in the memory (bus)
    // Incremented/decremented as we pull things from the stack
    public u16 SP { get; private set; }

    // Program counter
    // Stores the address of the next progran uint
    // Increases with each clock or can be directly set in a branch to jump to
    // Different parts of the program, like a if-statement
    public u16 PC { get; private set; } = 0x0000;

    public void Step()
    {
        // todo: future PPU stuff

        ExecuteInstruction();
        LogState();
    }

    Instruction GetCurrentInstruction() => _instructions[_currentInstructionAddress];
    Instruction GetInstruction(u8 address) => _instructions[address];

    void LogState()
    {
        var sb = new StringBuilder();

        var instruction = GetCurrentInstruction();
        sb.AppendLine( $"Instruction: {instruction.Opcode.ToHex()}" );

        sb.AppendLine( "Registers" );
        sb.Append( $"A: {A.ToHex()}, " );
        sb.Append( $"X: {X.ToHex()}, " );
        sb.Append( $"Y: {Y.ToHex()}, " );
        sb.Append( $"PC: {PC.ToHex()}, " );
        sb.Append( $"SP: {SP.ToHex()}" );
        sb.AppendLine();

        var Z = GetFlag( StatusFlag.Zero ) ? 1 : 0;
        var N = GetFlag( StatusFlag.Negative ) ? 1 : 0;
        var C = GetFlag( StatusFlag.Carry ) ? 1 : 0;
        var V = GetFlag( StatusFlag.Overflow ) ? 1 : 0;
        var I = GetFlag( StatusFlag.DisableInterrupts ) ? 1 : 0;
        var D = GetFlag( StatusFlag.DecimalMode ) ? 1 : 0;
        var B = GetFlag( StatusFlag.Break ) ? 1 : 0;
        sb.AppendLine( "Flags" );
        sb.Append( $"Z:{Z}, N:{N}, C:{C}, V:{V}, I:{I}, D:{D}, B:{B}" );
        logger.LogInformation( sb.ToString() );
    }

    // todo: this method should take an address from where instruction should be executed
    /// <summary>
    /// Executes one instruction.
    /// </summary>
    public int ExecuteInstruction()
    {
        if (_cycles == 0)
        {
            _currentInstructionAddress = NextByte();
            var instruction = GetCurrentInstruction();
            _cycles = (int)instruction.Cycles;
            var cycle1 = instruction.AddressMode();
            var cycle2 = instruction.Operate();
            // check if we need additional cycles
            _cycles += (int)(cycle1 & cycle2);
        }
        _cycles--;

        return _cycles;
    }

    // press reset on the NES
    public void Reset() 
    {
        PC = 0xFFFC;
        SP = 0x0100;
        FlagStatus = 0;
        A = X = Y = 0;

        // test
        memory.Write( 0xFFFC, 0xA9 );
        memory.Write( 0xFFFD, 42 );
    }
    
    void InterruptRequest() { }
    
    void NonMaskableInterrupt() { }

    u8 FetchData()
    {
        if (_instructions[_currentInstructionAddress].AddressMode != Implied)
        {
            _fetchedData = ReadByte( _addressAbsolute );
        }
        return _fetchedData;
    }

    u8 _fetchedData = 0x00;
    u16 _addressAbsolute = 0x0000;
    u16 _addressRelative = 0x0000;
    int _cycles = 0;

    Dictionary<u8, Instruction> _instructions;
    u8 _currentInstructionAddress;

    private readonly IBus memory = new Memory();
    private readonly ILogger<Cpu6502> logger;

    public Cpu6502(ILogger<Cpu6502> logger)
    {
        var instructions = ImmutableArray.Create(
         new Instruction( Name: nameof(BRK), Opcode: 0x00, Operate: BRK, AddressMode: Immediate, Cycles: 7 ),
         new Instruction( Name: nameof( ORA ), Opcode: 0x09, Operate: ORA, AddressMode: Immediate, Cycles: 2 ),
         new Instruction( Name: nameof( ORA ), Opcode: 0x05, Operate: ORA, AddressMode: ZeroPage, Cycles: 3 ),
         new Instruction( Name: nameof( ORA ), Opcode: 0x15, Operate: ORA, AddressMode: ZeroPageX, Cycles: 4 ),
         new Instruction( Name: nameof( ORA ), Opcode: 0x0D, Operate: ORA, AddressMode: Absolute, Cycles: 4 ),
         new Instruction( Name: nameof( ORA ), Opcode: 0x1D, Operate: ORA, AddressMode: AbsoluteX, Cycles: 4 ),
         new Instruction( Name: nameof( ORA ), Opcode: 0x19, Operate: ORA, AddressMode: AbsoluteY, Cycles: 4 ),
         new Instruction( Name: nameof( ORA ), Opcode: 0x01, Operate: ORA, AddressMode: IndirectX, Cycles: 6 ),
         new Instruction( Name: nameof( ORA ), Opcode: 0x11, Operate: ORA, AddressMode: IndirectY, Cycles: 5 ),
         new Instruction( Name: nameof( LDA ), Opcode: 0xA9, Operate: LDA, AddressMode: Immediate, Cycles: 2 ),
         new Instruction( Name: nameof( LDA ), Opcode: 0xA5, Operate: LDA, AddressMode: ZeroPage, Cycles: 3 ),
         new Instruction( Name: nameof( LDA ), Opcode: 0xB5, Operate: LDA, AddressMode: ZeroPageX, Cycles: 4 ),
         new Instruction( Name: nameof( LDA ), Opcode: 0xAD, Operate: LDA, AddressMode: Absolute, Cycles: 4 ),
         new Instruction( Name: nameof( LDA ), Opcode: 0xBD, Operate: LDA, AddressMode: AbsoluteX, Cycles: 4 ),
         new Instruction( Name: nameof( LDA ), Opcode: 0xB9, Operate: LDA, AddressMode: AbsoluteY, Cycles: 4 ),
         new Instruction( Name: nameof( LDA ), Opcode: 0xA1, Operate: LDA, AddressMode: IndirectX, Cycles: 6 ),
         new Instruction( Name: nameof( LDA ), Opcode: 0xB1, Operate: LDA, AddressMode: IndirectY, Cycles: 5 )
        );
        _instructions = instructions.ToDictionary( k => k.Opcode, v => v );
        this.logger = logger;
    }

    public u8 ReadByte( u16 address ) => memory.Read( address );
    public void WriteByte( u16 address, u8 data ) => memory.Write( address, data );
    
    u8 NextByte() => ReadByte( PC++ );

    // 6502 is little endian
    u16 NextWord() => (u16)(NextByte() | (NextByte() << 8));
    u16 ReadWord() => (u16)(ReadByte( PC ) | (ReadByte(PC) << 8));

    public bool GetFlag( StatusFlag flagBit )
        => (FlagStatus & (uint)flagBit) > 0 ? true : false;

    public void SetFlag( StatusFlag flagBit, bool set )
    {
        if (set)
        {
            FlagStatus |= (u8)flagBit;
        }
        else
        {
            // clear flag
            FlagStatus &= (u8)~flagBit;
        }
    }
}
