namespace DerpNES;

// todo: add name
internal record struct Instruction( uint Opcode, Func<uint> Operate, Func<uint> AddressMode, uint Cycles );
