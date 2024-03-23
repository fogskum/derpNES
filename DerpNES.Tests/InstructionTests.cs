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
        
        var Z = _cpu.GetFlag( Cpu6502.StatusFlag.Zero );
        var N = _cpu.GetFlag( Cpu6502.StatusFlag.Negative );

        _cpu.WriteByte( 0x0000, 0xA9 ); // LDA immediate instruction
        _cpu.WriteByte( 0x0001, A );

        // act
        _cpu.ExecuteInstruction();

        // assert
        
        // registers
        Assert.That( A, Is.EqualTo( _cpu.A ) );

        // todo: assert all flags
        Assert.That( Z, Is.EqualTo( _cpu.GetFlag(Cpu6502.StatusFlag.Zero) ) );
        Assert.That( N, Is.EqualTo( _cpu.GetFlag(Cpu6502.StatusFlag.Negative) ) );
    }
}