namespace mrousavy
{
    namespace Cryptography
    {
        /// <summary>
        ///     Cryptography interface, see <see cref="Cipher"/> for an example implementation
        /// </summary>
        public interface ICrypt
        {
            /// <summary>
            ///     Encrypt plain text using a password-key
            /// </summary>
            /// <param name="unencryptedText">The text to Encrypt</param>
            /// <param name="key">Encryption key</param>
            /// <returns>Encrypted Text</returns>
            string Encrypt(string key, string unencryptedText);

            /// <summary>
            ///     Decrypt encrypted text using a password-key
            /// </summary>
            /// <param name="encryptedText">The text to Decrypt</param>
            /// <param name="key">Encryption key</param>
            /// <returns>Decrypted Text</returns>
            string Decrypt(string key, string encryptedText);
        }
    }
}