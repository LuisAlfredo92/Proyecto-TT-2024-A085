using System.Security.Cryptography;
using Konscious.Security.Cryptography;

namespace Hashes;

public class Argon2Id(int memory = 19000, int iterations = 2, int parallelism = 1, byte[]? salt = null, byte[]? secret = null, byte[]? associatedData = null)
{
    private readonly byte[] _salt = salt ?? RandomNumberGenerator.GetBytes(16);
    private Argon2id? _argon2Id;

    public byte[] Hash(byte[] data)
    {
        // Minimal settings by OWASP https://cheatsheetseries.owasp.org/cheatsheets/Password_Storage_Cheat_Sheet.html
        _argon2Id = new Argon2id(data)
        {
            Salt = _salt,
            KnownSecret = secret,
            AssociatedData = associatedData,
            Iterations = iterations,
            MemorySize = memory,
            DegreeOfParallelism = parallelism
        };

        return _argon2Id.GetBytes(32);
    }
}