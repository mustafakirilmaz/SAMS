using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace SAMS.Helpers
{
    public class PasswordHelper
    {
        private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();
        private static int _iterationCount = 10000;

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (a == null && b == null)
            {
                return true;
            }
            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }
            var areSame = true;
            for (var i = 0; i < a.Length; i++)
            {
                areSame &= (a[i] == b[i]);
            }
            return areSame;
        }

        public static string HashPassword(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            return Convert.ToBase64String(HashPasswordV3(password, _rng));
        }

        private static byte[] HashPasswordV3(string password, RandomNumberGenerator rng)
        {
            return HashPasswordV3(password, rng,
                KeyDerivationPrf.HMACSHA256,
                _iterationCount,
                saltSize: 128 / 8,
                numBytesRequested: 256 / 8);
        }

        private static byte[] HashPasswordV3(string password, RandomNumberGenerator rng, KeyDerivationPrf prf, int iterCount, int saltSize, int numBytesRequested)
        {
            byte[] salt = new byte[saltSize];
            rng.GetBytes(salt);
            byte[] subkey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, numBytesRequested);
            var outputBytes = new byte[12 + salt.Length + subkey.Length];
            WriteNetworkByteOrder(outputBytes, 0, (uint)prf);
            WriteNetworkByteOrder(outputBytes, 4, (uint)iterCount);
            WriteNetworkByteOrder(outputBytes, 8, (uint)saltSize);
            Buffer.BlockCopy(salt, 0, outputBytes, 12, salt.Length);
            Buffer.BlockCopy(subkey, 0, outputBytes, 12 + saltSize, subkey.Length);
            return outputBytes;
        }

        private static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
        {
            buffer[offset + 0] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)(value >> 0);
        }

        public static bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            if (hashedPassword == null)
                throw new ArgumentNullException(nameof(hashedPassword));
            if (providedPassword == null)
                throw new ArgumentNullException(nameof(providedPassword));

            byte[] decodedHashedPassword = Convert.FromBase64String(hashedPassword);
            if (decodedHashedPassword.Length == 0)
                return false;
            int embeddedIterCount;
            if (VerifyHashedPasswordV3(decodedHashedPassword, providedPassword, out embeddedIterCount))
            {
                return embeddedIterCount == _iterationCount;
            }
            else
            {
                return false;
            }
        }

        private static bool VerifyHashedPasswordV3(byte[] hashedPassword, string password, out int iterCount)
        {
            iterCount = default(int);
            try
            {
                KeyDerivationPrf prf = (KeyDerivationPrf)ReadNetworkByteOrder(hashedPassword, 0);
                iterCount = (int)ReadNetworkByteOrder(hashedPassword, 4);
                int saltLength = (int)ReadNetworkByteOrder(hashedPassword, 8);
                if (saltLength < 128 / 8)
                {
                    return false;
                }
                byte[] salt = new byte[saltLength];
                Buffer.BlockCopy(hashedPassword, 12, salt, 0, salt.Length);
                int subkeyLength = hashedPassword.Length - 12 - salt.Length;
                if (subkeyLength < 128 / 8)
                {
                    return false;
                }
                byte[] expectedSubkey = new byte[subkeyLength];
                Buffer.BlockCopy(hashedPassword, 12 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);
                byte[] actualSubkey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, subkeyLength);
                return ByteArraysEqual(actualSubkey, expectedSubkey);
            }
            catch
            {
                return false;
            }
        }

        private static uint ReadNetworkByteOrder(byte[] buffer, int offset)
        {
            return ((uint)(buffer[offset + 0]) << 24)
                | ((uint)(buffer[offset + 1]) << 16)
                | ((uint)(buffer[offset + 2]) << 8)
                | ((uint)(buffer[offset + 3]));
        }

        private static string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }

            return res.ToString();
        }
    }
}
