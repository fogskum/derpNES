namespace DerpNES.Tests;

public partial class CpuTests
{
    [Test]
    public void Test_BNE_Relative_Is_True_And_Paged_Crossed()
    {
        // arrange
        int expectedCycles = 4;
        u16 PC = 2;

        var Z = _cpu.GetFlag( Cpu6502.StatusFlag.Zero );
        var N = _cpu.GetFlag( Cpu6502.StatusFlag.Negative );

        _cpu.WriteByte( 0xFFFC, 0xD0 ); // BNE, relative instruction
        _cpu.WriteByte( 0xFFFD, 0xB0 ); // relative address
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