namespace mrousavy {
    namespace Cryptography {
        /// <summary>
        ///     ICrypt implementation without any En/De-cryption
        /// </summary>
        public class NoEncryption : ICrypt {
            public string Decrypt(string key, string encryptedText) {
                return encryptedText;
            }

            public string Encrypt(string key, string unencryptedText) {
                return unencryptedText;
            }
        }
    }
}