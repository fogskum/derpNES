using Microsoft.Extensions.Logging;

namespace DerpNES.Tests;

public partial class CpuTests
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
    public void Test_Reset()
    {
        _cpu.Reset();

        Assert.That( _cpu.PC, Is.EqualTo( 0xFFFC ) );
        Assert.That( _cpu.SP, Is.EqualTo( 0 ) );
        Assert.That( _cpu.A, Is.EqualTo( 0 ) );
        Assert.That( _cpu.X, Is.EqualTo( 0 ) );
        Assert.That( _cpu.Y, Is.EqualTo( 0 ) );
        
        Assert.That( _cpu.GetFlag(Cpu6502.StatusFlag.Break), Is.EqualTo( false ) );
        Assert.That( _cpu.GetFlag(Cpu6502.StatusFlag.Negative), Is.EqualTo( false ) );
        Assert.That( _cpu.GetFlag(Cpu6502.StatusFlag.Overflow), Is.EqualTo( false ) );
        Assert.That( _cpu.GetFlag(Cpu6502.StatusFlag.Unused), Is.EqualTo( false ) );
        Assert.That( _cpu.GetFlag(Cpu6502.StatusFlag.Carry), Is.EqualTo( false ) );
        Assert.That( _cpu.GetFlag(Cpu6502.StatusFlag.Zero), Is.EqualTo( false ) );
        Assert.That( _cpu.GetFlag(Cpu6502.StatusFlag.DisableInterrupts), Is.EqualTo( true ) );
        Assert.That( _cpu.GetFlag(Cpu6502.StatusFlag.DecimalMode), Is.EqualTo( false ) );
    }
}
