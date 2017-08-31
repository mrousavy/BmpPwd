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
            /// <param name="salt">Salt for encryption</param>
            /// <returns>Encrypted Text</returns>
            string Encrypt(string salt, string unencryptedText);

            /// <summary>
            ///     Decrypt Text
            /// </summary>
            /// <param name="encryptedText">The text to Decrypt</param>
            /// <param name="salt">Salt for decryption</param>
            /// <returns>Decrypted Text</returns>
            string Decrypt(string salt, string encryptedText);
        }
    }
}