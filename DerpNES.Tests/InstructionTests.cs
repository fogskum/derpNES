using Microsoft.Extensions.Logging;

namespace DerpNES.Tests;

public class InstructionTests
{
    ILogger<Cpu6502> _logger;

    [SetUp]
    public void Setup()
    {
        _logger = new NullLogger<Cpu6502>();
    }

    [Test]
    public void TestFlags()
    {
        var cpu = new Cpu6502(_logger);

        cpu.SetFlag( Cpu6502.StatusFlag.Zero, true );
        cpu.SetFlag( Cpu6502.StatusFlag.Negative, true );
        var flagStatus = cpu.GetFlag( Cpu6502.StatusFlag.Zero );
        Assert.That( flagStatus, Is.EqualTo( true ) );
        
        cpu.SetFlag( Cpu6502.StatusFlag.Zero, false );
        flagStatus = cpu.GetFlag( Cpu6502.StatusFlag.Zero );
        Assert.That( flagStatus, Is.EqualTo( false ) );
    }

    [Test]
    public void Test_AND()
    {
        var cpu = new Cpu6502(_logger);
        var cycles = cpu.AND();

        Assert.Pass();
    }
}