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
        u8 a = 0b0110_1110;
        u8 b = 0b0000_1000;
        u8 result = (u8)(a & b);
        Assert.That( result, Is.EqualTo( 0b0000_1000 ) );
    }

    [Test]
    public void Test_XOR()
    {
        u8 a = 0b0110_1110;
        u8 b = 0b0010_0000;
        u8 result = (u8)(a ^ b);
        //         0100_1110
        var test = result.ToBinary();
        Assert.That( result, Is.EqualTo( 0b0100_1110 ) );
    }

    [Test]
    public void Test_OR()
    {
        u8 a = 0b0110_1110;
        u8 b = 0b0010_0000;
        u8 result = (u8)(a | b);
        //         0110_1110
        var test = result.ToBinary();
        Assert.That( result, Is.EqualTo( 0b0110_1110 ) );
    }

    [Test]
    public void Test_NOT()
    {
        u8 a = 0b0110_1110;
        u8 result = (u8)(a & ~(1 << 7));
        var b = Convert.ToString( result, 2 );
        //         1001_0001
        Assert.That( result, Is.EqualTo( 0b0110_1110 ) );
    }
}