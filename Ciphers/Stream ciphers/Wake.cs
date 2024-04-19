using System.ComponentModel.DataAnnotations;

namespace Stream_ciphers;

public class Wake([Length(4, 4)] ReadOnlySpan<int> key)
{
    private readonly int[] _table = GenerateTable(key);
    private int[] _r; // end key

    public byte[] Encrypt(byte[] plainText)
    {
        return plainText;
    }

    public byte[] Decrypt(byte[] cipherText)
    {
        return cipherText;
    }

    private void Cypher(int[] v, int n, int[] k, ref int[] r)
    {
        int r1, r2, r3, r4, r5, r6, e, d, m = 0x00ffffff;
        r3 = k[0];
        r4 = k[1];
        r5 = k[2];
        r6 = k[3];

        if (n < 0)
            d = -1;
        else
            d = 1;
    }

    private static int[] GenerateTable(ReadOnlySpan<int> key)
    {
        int x, z, p;
        // Table
        Span<int> t = stackalloc int[257],
            tt = [0x726a8f3b,
                unchecked((int)0xe69a3b5c),
                unchecked((int)0xd3c71fe5),
                unchecked((int)0xab3c73d2),
                0x4d3a8eb3,
                0x0396d6e8,
                0x3d4c2f7a,
                unchecked((int)0x9ee27cf3)];

        // Copy K
        key.CopyTo(t);

        // Fill t
        for (p = 4; p < 256; p++)
        {
            x = t[p - 4] + t[p - 1];
            t[p] = x >> 3 ^ tt[x & 7];
        }

        // Mix first entries
        for (p = 0; p < 23; p++)
            t[p] += t[p + 89];
        x = t[33];
        z = t[59] | 0x01000001;
        z &= unchecked((int)0xff7fffff);

        // change top byte to a permutation etc
        for (p = 0; p < 256; p++)
        {
            x = (x & unchecked((int)0xff7fffff)) + z;
            t[p] = t[p] & 0x00ffffff ^ x;
        }

        t[256] = t[0];
        x &= 255;

        // further change perm. and other digits
        for (p = 0; p < 256; p++)
        {
            t[p] = t[x = (t[p ^ x] ^ x) & 255];
            t[x] = t[p + 1];
        }

        return t.ToArray();
    }
}