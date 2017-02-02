namespace mrousavy {
    namespace Cryptography {
        public interface ICrypt {
            string Encrypt(string Passphrase, string UnencryptedText);
            string Decrypt(string Passphrase, string EncryptedText);
        }
    }
}
