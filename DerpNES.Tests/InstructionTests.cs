using Microsoft.Extensions.Logging;

namespace DerpNES.Tests;

public class InstructionTests
{
    ILogger<Cpu6502> _logger;
    Cpu6502 _cpu;

    [SetUp]
    public void Setup()
    {
        _logger = new NullLogger<Cpu6502>();
        _cpu = new Cpu6502( _logger );
    }

    [Test]
    public void Test_Flag_Get_And_Set()
    {
        _cpu.SetFlag( Cpu6502.StatusFlag.Zero, true );
        _cpu.SetFlag( Cpu6502.StatusFlag.Negative, true );
        var flagStatus = _cpu.GetFlag( Cpu6502.StatusFlag.Zero );
        Assert.That( flagStatus, Is.EqualTo( true ) );

        _cpu.SetFlag( Cpu6502.StatusFlag.Zero, false );
        flagStatus = _cpu.GetFlag( Cpu6502.StatusFlag.Zero );
        Assert.That( flagStatus, Is.EqualTo( false ) );
    }

    [Test]
    public void Test_LDA_Immediate_LoadsValue()
    {
        // arrange
        u8 A = 42;
        int expectedCycles = 2;

        var Z = _cpu.GetFlag( Cpu6502.StatusFlag.Zero );
        var N = _cpu.GetFlag( Cpu6502.StatusFlag.Negative );

        _cpu.WriteByte( 0x0000, 0xA9 ); // LDA immediate instruction
        _cpu.WriteByte( 0x0001, A );

        // act
        var cycles = _cpu.ExecuteInstruction();

        // assert

        Assert.That( expectedCycles, Is.EqualTo( cycles ) );

        // registers
        Assert.That( A, Is.EqualTo( _cpu.A ) );
        Assert.That( Z, Is.EqualTo( _cpu.GetFlag( Cpu6502.StatusFlag.Zero ) ) );
        Assert.That( N, Is.EqualTo( _cpu.GetFlag( Cpu6502.StatusFlag.Negative ) ) );
    }

    [Test]
    public void Test_LDA_ZeroPage_Loads_Value()
    {
        // arrange
        u8 A = 42;
        int expectedCycles = 3;

        var Z = _cpu.GetFlag( Cpu6502.StatusFlag.Zero );
        var N = _cpu.GetFlag( Cpu6502.StatusFlag.Negative );

        _cpu.WriteByte( 0x0000, 0xA5 ); // LDA ZeroPage instruction
        _cpu.WriteByte( 0x0001, 0x0A ); // look for value at 0x0A
        _cpu.WriteByte( 0x000A, A );

        // act
        var cycles = _cpu.ExecuteInstruction();

        // assert
        Assert.That( expectedCycles, Is.EqualTo( cycles ) );

        // registers
        Assert.That( A, Is.EqualTo( _cpu.A ) );
        Assert.That( Z, Is.EqualTo( _cpu.GetFlag( Cpu6502.StatusFlag.Zero ) ) );
        Assert.That( N, Is.EqualTo( _cpu.GetFlag( Cpu6502.StatusFlag.Negative ) ) );
    }

    [Test]
    public void Test_LDA_ZeroPageX_Loads_Value()
    {
        // arrange
        u8 X = 5;
        _cpu.X = X;
        u8 A = 42;
        int expectedCycles = 4;

        var Z = _cpu.GetFlag( Cpu6502.StatusFlag.Zero );
        var N = _cpu.GetFlag( Cpu6502.StatusFlag.Negative );

        _cpu.WriteByte( 0x0000, 0xB5 ); // LDA ZeroPage, X instruction
        _cpu.WriteByte( 0x0001, 0x0A ); // look for value at 0x0A+X
        var valueAddress = 0x0A + X;
        _cpu.WriteByte( (u16)valueAddress, A );

        // act
        var cycles = _cpu.ExecuteInstruction();

        // assert
        Assert.That( expectedCycles, Is.EqualTo( cycles ) );

        // registers
        Assert.That( A, Is.EqualTo( _cpu.A ) );
        Assert.That( Z, Is.EqualTo( _cpu.GetFlag( Cpu6502.StatusFlag.Zero ) ) );
        Assert.That( N, Is.EqualTo( _cpu.GetFlag( Cpu6502.StatusFlag.Negative ) ) );
    }

    [Test]
    public void Test_LDA_ZeroPageX_Loads_Value_When_It_Wraps()
    {
        // arrange
        u8 A = 42;
        _cpu.X = 0xFF;
        int expectedCycles = 4;

        var Z = _cpu.GetFlag( Cpu6502.StatusFlag.Zero );
        var N = _cpu.GetFlag( Cpu6502.StatusFlag.Negative );

        _cpu.WriteByte( 0x0000, 0xB5 ); // LDA ZeroPage, X instruction
        _cpu.WriteByte( 0x0001, 0x80 ); // look for value at 0x0A+X
        _cpu.WriteByte( 0x007F, A );

        // act
        var cycles = _cpu.ExecuteInstruction();

        // assert
        Assert.That( expectedCycles, Is.EqualTo( cycles ) );

        // registers
        Assert.That( A, Is.EqualTo( _cpu.A ) );
        Assert.That( Z, Is.EqualTo( _cpu.GetFlag( Cpu6502.StatusFlag.Zero ) ) );
        Assert.That( N, Is.EqualTo( _cpu.GetFlag( Cpu6502.StatusFlag.Negative ) ) );
    }

    [Test]
    public void Test_LDA_Absolute_LoadsValue()
    {
        // arrange
        u8 A = 42;
        int expectedCycles = 4;

        var Z = _cpu.GetFlag( Cpu6502.StatusFlag.Zero );
        var N = _cpu.GetFlag( Cpu6502.StatusFlag.Negative );

        _cpu.WriteByte( 0x0000, 0xAD ); // LDA, absolute instruction
        _cpu.WriteByte( 0x0001, 0xB0 );
        _cpu.WriteByte( 0x0002, 0xA0 );
        _cpu.WriteByte( 0xA0B0, A );

        // act
        var cycles = _cpu.ExecuteInstruction();

        // assert

        Assert.That( expectedCycles, Is.EqualTo( cycles ) );

        // registers
        Assert.That( A, Is.EqualTo( _cpu.A ) );
        Assert.That( Z, Is.EqualTo( _cpu.GetFlag( Cpu6502.StatusFlag.Zero ) ) );
        Assert.That( N, Is.EqualTo( _cpu.GetFlag( Cpu6502.StatusFlag.Negative ) ) );
    }

    [Test]
    public void Test_LDA_AbsoluteX_No_Page_Change_LoadsValue()
    {
        // arrange
        u8 X = 5;
        _cpu.X = X;
        u8 A = 42;
        int expectedCycles = 4;

        var Z = _cpu.GetFlag( Cpu6502.StatusFlag.Zero );
        var N = _cpu.GetFlag( Cpu6502.StatusFlag.Negative );

        _cpu.WriteByte( 0x0000, 0xBD ); // LDA, absolute, X instruction
        _cpu.WriteByte( 0x0001, 0xB0 );
        _cpu.WriteByte( 0x0002, 0xA0 );
        var valueAddress = 0xA0B0 + X;
        _cpu.WriteByte( (u16)valueAddress, A );

        // act
        var cycles = _cpu.ExecuteInstruction();

        // assert

        Assert.That( expectedCycles, Is.EqualTo( cycles ) );

        // registers
        Assert.That( A, Is.EqualTo( _cpu.A ) );
        Assert.That( Z, Is.EqualTo( _cpu.GetFlag( Cpu6502.StatusFlag.Zero ) ) );
        Assert.That( N, Is.EqualTo( _cpu.GetFlag( Cpu6502.StatusFlag.Negative ) ) );
    }

    [Test]
    public void Test_LDA_AbsoluteY_No_Page_Change_LoadsValue()
    {
        // arrange
        u8 Y = 5;
        _cpu.Y = Y;
        u8 A = 42;
        int expectedCycles = 4;

        var Z = _cpu.GetFlag( Cpu6502.StatusFlag.Zero );
        var N = _cpu.GetFlag( Cpu6502.StatusFlag.Negative );

        _cpu.WriteByte( 0x0000, 0xB9 ); // LDA, absolute Y instruction
        _cpu.WriteByte( 0x0001, 0xB0 );
        _cpu.WriteByte( 0x0002, 0xA0 );
        var valueAddress = 0xA0B0 + Y;
        _cpu.WriteByte( (u16)valueAddress, A );

        // act
        var cycles = _cpu.ExecuteInstruction();

        // assert

        Assert.That( expectedCycles, Is.EqualTo( cycles ) );

        // registers
        Assert.That( A, Is.EqualTo( _cpu.A ) );
        Assert.That( Z, Is.EqualTo( _cpu.GetFlag( Cpu6502.StatusFlag.Zero ) ) );
        Assert.That( N, Is.EqualTo( _cpu.GetFlag( Cpu6502.StatusFlag.Negative ) ) );
    }

    [Test]
    public void Test_BNE_Relative_Is_True_And_Paged_Crossed()
    {
        // arrange
        int expectedCycles = 4;
        u16 PC = 2;

        var Z = _cpu.GetFlag( Cpu6502.StatusFlag.Zero );
        var N = _cpu.GetFlag( Cpu6502.StatusFlag.Negative );

        _cpu.WriteByte( 0x0000, 0xD0 ); // BNE, relative instruction
        _cpu.WriteByte( 0x0001, 0xB0 ); // relative address
        var valueAddress = 0x0001 + 0xB0;
        _cpu.WriteByte( (u16)valueAddress, (u8)PC );

        // act
        var cycles = _cpu.ExecuteInstruction();

        // assert

        Assert.That( expectedCycles, Is.EqualTo( cycles ) );

        // registers
        Assert.That( PC, Is.EqualTo( _cpu.A ) );
        Assert.That( Z, Is.EqualTo( _cpu.GetFlag( Cpu6502.StatusFlag.Zero ) ) );
        Assert.That( N, Is.EqualTo( _cpu.GetFlag( Cpu6502.StatusFlag.Negative ) ) );
    }
}