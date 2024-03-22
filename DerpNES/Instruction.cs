namespace DerpNES;

internal record struct Instruction( uint Opcode, Func<UInt8> Operate, Func<UInt8> AddressMode, UInt8 Cycles );
