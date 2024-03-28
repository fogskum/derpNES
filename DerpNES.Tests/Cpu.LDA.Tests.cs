namespace DerpNES.Tests;

public partial class CpuTests
{
    [Test]
    public void Test_LDA_Immediate_LoadsValue()
    {
        // arrange
        u8 A = 42;
        int expectedCycles = 2;

        var Z = _cpu.GetFlag( Cpu6502.StatusFlag.Zero );
        var N = _cpu.GetFlag( Cpu6502.StatusFlag.Negative );

        _cpu.WriteByte( 0xFFFC, 0xA9 ); // LDA immediate instruction
        _cpu.WriteByte( 0xFFFD, A );

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

        _cpu.WriteByte( 0xFFFC, 0xA5 ); // LDA ZeroPage instruction
        _cpu.WriteByte( 0xFFFD, 0x0A ); // look for value at 0x0A
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

        _cpu.WriteByte( 0xFFFC, 0xB5 ); // LDA ZeroPage, X instruction
        _cpu.WriteByte( 0xFFFD, 0x0A ); // look for value at 0x0A+X
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

        _cpu.WriteByte( 0xFFFC, 0xB5 ); // LDA ZeroPage, X instruction
        _cpu.WriteByte( 0xFFFD, 0x80 ); // look for value at 0x0A+X
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

        _cpu.WriteByte( 0xFFFC, 0xAD ); // LDA, absolute instruction
        _cpu.WriteByte( 0xFFFD, 0xB0 );
        _cpu.WriteByte( 0xFFFE, 0xA0 );
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

        _cpu.WriteByte( 0xFFFC, 0xBD ); // LDA, absolute, X instruction
        _cpu.WriteByte( 0xFFFD, 0xB0 );
        _cpu.WriteByte( 0xFFFE, 0xA0 );
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

        _cpu.WriteByte( 0xFFFC, 0xB9 ); // LDA, absolute Y instruction
        _cpu.WriteByte( 0xFFFD, 0xB0 );
        _cpu.WriteByte( 0xFFFE, 0xA0 );
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
    public void Test_LDA_IndexedIndirect_No_Page_Change_LoadsValue()
    {
        // arrange
        _cpu.X = 4;
        u8 A = 42;
        int expectedCycles = 6;

        var Z = _cpu.GetFlag( Cpu6502.StatusFlag.Zero );
        var N = _cpu.GetFlag( Cpu6502.StatusFlag.Negative );

        _cpu.WriteByte( 0xFFFC, 0xA1 );
        _cpu.WriteByte( 0xFFFD, 0x02 );
        _cpu.WriteByte( 0x0006, 0x00 ); // 2 + 4
        _cpu.WriteByte( 0x0007, 0x80 );
        _cpu.WriteByte( 0x8000, A );

        // act
        var cycles = _cpu.ExecuteInstruction();

        // assert

        Assert.That( expectedCycles, Is.EqualTo( cycles ) );

        // registers
        Assert.That( A, Is.EqualTo( _cpu.A ) );
        Assert.That( Z, Is.EqualTo( _cpu.GetFlag( Cpu6502.StatusFlag.Zero ) ) );
        Assert.That( N, Is.EqualTo( _cpu.GetFlag( Cpu6502.StatusFlag.Negative ) ) );
    }
}