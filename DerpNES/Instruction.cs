namespace DerpNES;

public record struct Instruction( string Name, u8 Opcode, Func<u8> Operate, Func<u8> AddressMode, u8 Cycles );
