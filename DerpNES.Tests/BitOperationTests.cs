namespace DerpNES.Tests;

public class BitOperationTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test_AND()
    {
        uint a = 0b0110_1110;
        uint b = 0b0000_1000;
        uint result = a & b;
        Assert.That( result, Is.EqualTo( 0b0000_1000 ) );
    }

    [Test]
    public void Test_XOR()
    {
        uint a = 0b0110_1110;
        uint b = 0b0010_0000;
        uint result = a ^ b;
        //         0100_1110
        var test = result.ToBinary();
        Assert.That( result, Is.EqualTo( 0b0100_1110 ) );
    }

    [Test]
    public void Test_OR()
    {
        uint a = 0b0110_1110;
        uint b = 0b0010_0000;
        uint result = a | b;
        //         0110_1110
        var test = result.ToBinary();
        Assert.That( result, Is.EqualTo( 0b0110_1110 ) );
    }

    [Test]
    public void Test_NOT()
    {
        uint a = 0b0110_1110;
        uint result = (uint)(a & ~(1 << 7));
        var b = Convert.ToString( result, 2 );
        //         1001_0001
        Assert.That( result, Is.EqualTo( 0b0110_1110 ) );
    }
}