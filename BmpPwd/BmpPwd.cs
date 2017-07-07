using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace mrousavy {

    namespace Cryptography {
        /// <summary>
        /// Text and Bitmap Cryptography
        /// </summary>
        public static class BmpPwd {
            public static Random random = new Random();

            /// <summary>
            /// Drawing Scheme/Style for Image Drawing
            /// (Use <see cref="DrawingScheme.Line"/> for faster encryption and minimal storage usage)
            /// </summary>
            public enum DrawingScheme { Line, Circular, Square }

            /// <summary>
            /// Color Scheme/Style for Image Drawing
            /// </summary>
            public enum ColorScheme { Greyscale, RedOnly, GreenOnly, BlueOnly, RedMixed, GreenMixed, BlueMixed, Rainbow }

            //Text & Bitmap Encryption
            #region Encrypt
            /// <summary>
            /// Encrypt Text to a Bitmap with default Cipher Encryption
            /// </summary>
            /// <param name="salt">The salt used for the Encryption</param>
            /// <param name="unencryptedText">The original unencrypted Text</param>
            /// <returns>The Encrypted Bitmap</returns>
            public static Bitmap Encrypt(string salt, string unencryptedText) {
                return Encrypt(salt, unencryptedText, new Cipher());
            }

            /// <summary>
            /// Encrypt Text to a Bitmap with default Cipher Encryption
            /// </summary>
            /// <param name="salt">The salt used for the Encryption</param>
            /// <param name="unencryptedText">The original unencrypted Text</param>
            /// <param name="cryptScheme">The Scheme/Interface Used for Encryption</param>
            /// <param name="drawingScheme">The <see cref="DrawingScheme"/> to use for Drawing the Image</param>
            /// <returns>The Encrypted Bitmap</returns>
            public static Bitmap Encrypt(
                string salt,
                string unencryptedText,
                ICrypt cryptScheme,
                DrawingScheme drawingScheme = DrawingScheme.Line,
                ColorScheme colorScheme = ColorScheme.RedMixed) {
                if (cryptScheme == null)
                    cryptScheme = new Cipher();

                //Get the encrypted Text
                string encryptedText = cryptScheme.Encrypt(salt, unencryptedText);

                //Get all ASCII values
                byte[] asciiValues = Encoding.Unicode.GetBytes(encryptedText);

                //Set correct Width and Height values
                int width = 0, height = 0;
                Helper.SetWidthHeight(drawingScheme, ref height, ref width, asciiValues.Length);

                //Create Bitmap with correct Sizes
                Bitmap encryptedBitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

                //Draw onto the Bitmap
                Helper.DrawCorrectScheme(encryptedBitmap, drawingScheme, colorScheme, asciiValues);

                return encryptedBitmap;
            }
            #endregion

            //Text & Bitmap Decryption
            #region Decrypt
            /// <summary>
            /// Decrypt a encrypted <see cref="Bitmap"/> to a <see cref="string"/>
            /// </summary>
            /// <param name="salt">The salt used for the Encryption</param>
            /// <param name="encryptedBitmap">The <see cref="BmpPwd"/> Encrypted <see cref="Bitmap"/></param>
            /// <returns>The decrypted Text from the Bitmap</returns>
            public static string Decrypt(string salt, Bitmap encryptedBitmap) {
                return Decrypt(salt, encryptedBitmap, new Cipher(), DrawingScheme.Line);
            }

            /// <summary>
            /// Decrypt a encrypted <see cref="Bitmap"/> to a <see cref="string"/>
            /// </summary>
            /// <param name="salt">The salt used for the Encryption</param>
            /// <param name="encryptedBitmap">The <see cref="BmpPwd"/> Encrypted <see cref="Bitmap"/></param>
            /// <param name="cryptScheme">The Scheme/Interface Used for Decryption</param>
            /// <param name="drawingScheme">The <see cref="DrawingScheme"/> to use for Drawing the Image</param>
            /// <returns>The decrypted Text from the Bitmap</returns>
            public static string Decrypt(
                string salt,
                Bitmap encryptedBitmap,
                ICrypt cryptScheme,
                DrawingScheme drawingScheme = DrawingScheme.Line,
                ColorScheme colorScheme = ColorScheme.RedMixed) {
                if (cryptScheme == null)
                    cryptScheme = new Cipher();

                //Set Width and Y for Image Reading
                int y = 0, width = 0;
                Helper.SetWidthY(drawingScheme, ref y, ref width, encryptedBitmap.Width);

                //Get all Colors from the Bitmap
                Color[] colors = Helper.GetPixelsFromBitmap(width, y, drawingScheme, colorScheme, encryptedBitmap);

                //Fill ASCII Values with Color's R Value (R = G = B)
                byte[] asciiValues = new byte[width];
                for (int i = 0; i < width; i++) {
                    asciiValues[i] = Helper.GetAsciiValue(colorScheme, colors[i]);
                }

                //Decrypt result
                string decrypted = Encoding.Unicode.GetString(asciiValues);
                decrypted = cryptScheme.Decrypt(salt, decrypted);

                return decrypted;
            }
            #endregion
        }
    }
}
