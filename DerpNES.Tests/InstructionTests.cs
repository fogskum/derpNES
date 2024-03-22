namespace DerpNES.Tests;

public class InstructionTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestFlags()
    {
        var cpu = new Cpu6502();
        
        cpu.SetFlag( Cpu6502.StatusFlag.Zero, true );
        cpu.SetFlag( Cpu6502.StatusFlag.Negative, true );
        var flagStatus = cpu.GetFlag( Cpu6502.StatusFlag.Zero );
        Assert.That( flagStatus, Is.EqualTo( true ) );
        
        cpu.SetFlag( Cpu6502.StatusFlag.Zero, false );
        flagStatus = cpu.GetFlag( Cpu6502.StatusFlag.Zero );
        Assert.That( flagStatus, Is.EqualTo( false ) );
    }
}