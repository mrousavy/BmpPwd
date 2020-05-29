using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BmpPwd
{
    /// <summary>
    ///     Example of <see cref="ICrypt" /> (Cipher En/De-crypt Text with password-keys)
    /// </summary>
    /// <seealso cref="https://stackoverflow.com/questions/10168240/encrypting-decrypting-a-string-in-c-sharp" />
    public class Cipher : ICrypt
    {
        private readonly int _keysize;
        private const int DerivationIterations = 1000;

        public Cipher(int keySize = 128)
        {
            _keysize = keySize;
        }

        /// <summary>
        ///     Encrypt a plain text using a string password-key
        /// </summary>
        /// <param name="unencryptedText">The plain text to encrypt</param>
        /// <param name="key">Encryption Key, or "password"</param>
        /// <returns>Encrypted Text</returns>
        public string Encrypt(string key, string unencryptedText)
        {
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting.  
            var saltStringBytes = GenerateBitsOfRandomEntropy(_keysize);
            var ivStringBytes = GenerateBitsOfRandomEntropy(_keysize);
            var plainTextBytes = Encoding.UTF8.GetBytes(unencryptedText);
            using (var password = new Rfc2898DeriveBytes(key, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(_keysize / 8);
                using (var symmetricKey = Rijndael.Create())
                {
                    // only .NET Core the block size has to be 128!
                    symmetricKey.BlockSize = _keysize;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Decrypt an encrypted text using a string password-key
        /// </summary>
        /// <param name="encryptedText">The encrypted text to decrypt</param>
        /// <param name="key">Encryption Key, or "password"</param>
        /// <returns>Decrypted Text</returns>
        public string Decrypt(string key, string encryptedText)
        {
            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(encryptedText);
            // Get the salt bytes by extracting the first 32 bytes from the supplied cipherText bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(_keysize / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(_keysize / 8).Take(_keysize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip(_keysize / 8 * 2)
                .Take(cipherTextBytesWithSaltAndIv.Length - _keysize / 8 * 2).ToArray();

            using (var password = new Rfc2898DeriveBytes(key, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(_keysize / 8);
                using (var symmetricKey = Rijndael.Create())
                {
                    // only .NET Core the block size has to be 128!
                    symmetricKey.BlockSize = _keysize;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        private static byte[] GenerateBitsOfRandomEntropy(int size = 256)
        {
            var randomBytes = new byte[size / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(randomBytes);
            }

            return randomBytes;
        }
    }
}