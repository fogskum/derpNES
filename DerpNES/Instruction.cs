namespace DerpNES;

// todo: add name
internal record struct Instruction( string Name, uint Opcode, Func<uint> Operate, Func<uint> AddressMode, uint Cycles );
