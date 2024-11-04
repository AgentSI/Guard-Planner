using System.Security.Cryptography;

namespace Domain
{
    public static class Crypto
    {
        private const int Pbkdf2IterCount = 1000; // default for Rfc2898DeriveBytes
        private const int Pbkdf2SubkeyLength = 256 / 8; // 256 bits
        private const int SaltSize = 128 / 8; // 128 bits

        internal static byte[] GenerateSaltInternal(int byteLength = SaltSize)
        {
            var buf = new byte[byteLength];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(buf);

            return buf;
        }

        public static string GenerateSalt(int byteLength = SaltSize)
        {
            return Convert.ToBase64String(GenerateSaltInternal(byteLength));
        }

        public static string HashPassword(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            // Produce a version 0 (see comment above) password hash.
            byte[] salt;
            byte[] subkey;
            using (var deriveBytes = new Rfc2898DeriveBytes(password, SaltSize, Pbkdf2IterCount))
            {
                salt = deriveBytes.Salt;
                subkey = deriveBytes.GetBytes(Pbkdf2SubkeyLength);
            }

            byte[] outputBytes = new byte[1 + SaltSize + Pbkdf2SubkeyLength];
            Buffer.BlockCopy(salt, 0, outputBytes, 1, SaltSize);
            Buffer.BlockCopy(subkey, 0, outputBytes, 1 + SaltSize, Pbkdf2SubkeyLength);

            return Convert.ToBase64String(outputBytes);
        }

        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            if (hashedPassword == null)
            {
                throw new ArgumentNullException(nameof(hashedPassword));
            }
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            var hashedPasswordBytes = Convert.FromBase64String(hashedPassword);

            // Verify a version 0 (see comment above) password hash.
            if (hashedPasswordBytes.Length != (1 + SaltSize + Pbkdf2SubkeyLength) || hashedPasswordBytes[0] != 0x00)
            {
                // Wrong length or version header.
                return false;
            }

            byte[] salt = new byte[SaltSize];
            Buffer.BlockCopy(hashedPasswordBytes, 1, salt, 0, SaltSize);
            byte[] storedSubkey = new byte[Pbkdf2SubkeyLength];
            Buffer.BlockCopy(hashedPasswordBytes, 1 + SaltSize, storedSubkey, 0, Pbkdf2SubkeyLength);

            byte[] generatedSubkey;
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, Pbkdf2IterCount))
            {
                generatedSubkey = deriveBytes.GetBytes(Pbkdf2SubkeyLength);
            }

            return ByteArraysEqual(storedSubkey, generatedSubkey);
        }

        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }

            var areSame = true;
            for (int i = 0; i < a.Length; i++)
            {
                areSame &= (a[i] == b[i]);
            }
            return areSame;
        }
    }
}
