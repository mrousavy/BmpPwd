using System.Drawing;

namespace mrousavy {

    namespace Cryptography {
        /// <summary>
        /// En/De-crypt Text to a Bitmap
        /// </summary>
        public static class BmpPwd {
            public static Bitmap Encrypt(string Passphrase, string UnencryptedText) {
                return BmpPwd.Encrypt(Passphrase, UnencryptedText, new Cipher());
            }

            public static Bitmap Encrypt(string Passphrase, string UnencryptedText, ICrypt CryptSchema) {
                Bitmap encrypted;



                return null;
            }


        }


        /// <summary>
        /// En/De-crypt Text with Passphrases
        /// </summary>
        public class Cipher : ICrypt {
            public string Encrypt(string Passphrase, string UnencryptedText) {

            }
            public string Decrypt(string Passphrase, string EncryptedText) {

            }
        }
    }
}
