using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace mrousavy {

    namespace Cryptography {
        /// <summary>
        /// En/De-crypt Text to a Bitmap
        /// </summary>
        public static class BmpPwd {
            /// <summary>
            /// Encrypt Text to a Bitmap with default Cipher Encryption
            /// </summary>
            /// <param name="salt">The salt used for the Encryption</param>
            /// <param name="unencryptedText">The original unencrypted Text</param>
            /// <returns>The Encrypted Bitmap</returns>
            public static Bitmap Encrypt(string salt, string unencryptedText) {
                return BmpPwd.Encrypt(salt, unencryptedText, new Cipher());
            }

            /// <summary>
            /// Encrypt Text to a Bitmap with default Cipher Encryption
            /// </summary>
            /// <param name="salt">The salt used for the Encryption</param>
            /// <param name="unencryptedText">The original unencrypted Text</param>
            /// <param name="cryptSchema">The Schema/Interface Used for Encryption</param>
            /// <returns>The Encrypted Bitmap</returns>
            public static Bitmap Encrypt(string salt, string unencryptedText, ICrypt cryptSchema) {
                //Get the encrypted Text
                string encryptedText = cryptSchema.Encrypt(salt, unencryptedText);

                //Create Bitmap with correct Sizes
                Bitmap encryptedBitmap = new Bitmap(encryptedText.Length, 1, PixelFormat.Format32bppArgb);

                //Get all ASCII values
                byte[] asciiValues = Encoding.ASCII.GetBytes(encryptedText);

                //Initialize Graphics
                using(Graphics gfx = Graphics.FromImage(encryptedBitmap)) {
                    //Position in Bitmap
                    int position = 0;

                    //Loop through each Pixel
                    foreach(byte b in asciiValues) {
                        //Set Pixel to ASCII Values
                        using(SolidBrush brush = new SolidBrush(Color.FromArgb(b, b, b))) {
                            gfx.FillRectangle(brush, position, 0, 1, 1);
                        }
                        position++;
                    }
                }

                return encryptedBitmap;
            }



            //TODO: Decrypt
        }
    }
}
