using System;
using System.Security.Cryptography;
using System.Text;

namespace IdentityServer.Admin.Core.Extensions
{
    /// <summary>Extension methods for hashing strings</summary>
    public static class HashExtensions
    {
        /// <summary>Creates a SHA256 hash of the specified input.</summary>
        /// <param name="input">The input.</param>
        /// <returns>A hash</returns>
        public static string Sha256(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            using SHA256 shA256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(shA256.ComputeHash(bytes));
        }

        /// <summary>Creates a SHA256 hash of the specified input.</summary>
        /// <param name="input">The input.</param>
        /// <returns>A hash.</returns>
        public static byte[] Sha256(this byte[] input)
        {
            if (input == null)
                return null;

            using SHA256 shA256 = SHA256.Create();
            return shA256.ComputeHash(input);
        }

        /// <summary>Creates a SHA512 hash of the specified input.</summary>
        /// <param name="input">The input.</param>
        /// <returns>A hash</returns>
        public static string Sha512(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            using SHA512 shA512 = SHA512.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(shA512.ComputeHash(bytes));
        }

        /// <summary>
        /// Create a data hash
        /// </summary>
        /// <param name="data">The data for calculating the hash</param>
        /// <param name="hashAlgorithm">Hash algorithm</param>
        /// <param name="trimByteCount">The number of bytes, which will be used in the hash algorithm; leave 0 to use all array</param>
        /// <returns>Data hash</returns>
        public static string CreateHash(byte[] data, string hashAlgorithm, int trimByteCount = 0)
        {
            if (string.IsNullOrEmpty(hashAlgorithm))
                throw new ArgumentNullException(nameof(hashAlgorithm));

            var algorithm = (HashAlgorithm)CryptoConfig.CreateFromName(hashAlgorithm);
            if (algorithm == null)
                throw new ArgumentException("Unrecognized hash name");

            if (trimByteCount > 0 && data.Length > trimByteCount)
            {
                var newData = new byte[trimByteCount];
                Array.Copy(data, newData, trimByteCount);

                return BitConverter.ToString(algorithm.ComputeHash(newData)).Replace("-", string.Empty);
            }

            return BitConverter.ToString(algorithm.ComputeHash(data)).Replace("-", string.Empty);
        }
    }
}
