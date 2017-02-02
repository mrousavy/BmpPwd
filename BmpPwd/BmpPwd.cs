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
    }
}
