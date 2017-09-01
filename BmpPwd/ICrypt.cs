namespace mrousavy {
    namespace Cryptography {
        /// <summary>
        ///     Cryptography interface
        /// </summary>
        public interface ICrypt {
            /// <summary>
            ///     Encrypt Text
            /// </summary>
            /// <param name="unencryptedText">The text to Encrypt</param>
            /// <param name="key">Encryption key</param>
            /// <returns>Encrypted Text</returns>
            string Encrypt(string key, string unencryptedText);

            /// <summary>
            ///     Decrypt Text
            /// </summary>
            /// <param name="encryptedText">The text to Decrypt</param>
            /// <param name="key">Encryption key</param>
            /// <returns>Decrypted Text</returns>
            string Decrypt(string key, string encryptedText);
        }
    }
}