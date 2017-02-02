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
            /// Drawing Scheme/Style for Image Drawing
            /// (Use <see cref="DrawingScheme.Line"/> for faster encryption and minimal storage usage)
            /// </summary>
            public enum DrawingScheme { Line, Circular, Square }


            #region Enccrypt
            /// <summary>
            /// Encrypt Text to a Bitmap with default Cipher Encryption
            /// </summary>
            /// <param name="salt">The salt used for the Encryption</param>
            /// <param name="unencryptedText">The original unencrypted Text</param>
            /// <returns>The Encrypted Bitmap</returns>
            public static Bitmap Encrypt(string salt, string unencryptedText) {
                return BmpPwd.Encrypt(salt, unencryptedText, new Cipher(), DrawingScheme.Circular);
            }

            /// <summary>
            /// Encrypt Text to a Bitmap with default Cipher Encryption
            /// </summary>
            /// <param name="salt">The salt used for the Encryption</param>
            /// <param name="unencryptedText">The original unencrypted Text</param>
            /// <param name="cryptSchema">The Schema/Interface Used for Encryption</param>
            /// <param name="drawingScheme">The <see cref="DrawingScheme"/> to use for Drawing the Image</param>
            /// <returns>The Encrypted Bitmap</returns>
            public static Bitmap Encrypt(string salt, string unencryptedText, ICrypt cryptSchema, DrawingScheme drawingScheme) {
                //Get the encrypted Text
                string encryptedText = cryptSchema.Encrypt(salt, unencryptedText);

                int width = encryptedText.Length;
                int height = 1;

                switch(drawingScheme) {
                    case DrawingScheme.Circular:
                        //Circular has radius of bytes -> width & height = textlength * 2
                        width = encryptedText.Length * 2;
                        height = encryptedText.Length * 2;
                        break;
                    case DrawingScheme.Line:
                        //Line has only height of 1 (default values)
                        break;
                    case DrawingScheme.Square:
                        //Square has dynamic height -> width & height = textlength * 2
                        width = encryptedText.Length * 2;
                        height = encryptedText.Length * 2;
                        break;
                }

                //Create Bitmap with correct Sizes
                Bitmap encryptedBitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

                //Get all ASCII values
                byte[] asciiValues = Encoding.ASCII.GetBytes(encryptedText);

                //Initialize Graphics
                using(Graphics gfx = Graphics.FromImage(encryptedBitmap)) {
                    //Position in Bitmap
                    int position = 0;
                    int diameter = encryptedBitmap.Width;

                    #region Drawing
                    //Loop through each Pixel
                    foreach(byte b in asciiValues) {

                        //Set Pixel to ASCII Values (change Color.FromArg() values for different colors)
                        using(SolidBrush brush = new SolidBrush(Color.FromArgb(b * 2, 0, 0))) {
                            using(Pen pen = new Pen(brush)) {
                                switch(drawingScheme) {
                                    case DrawingScheme.Circular:
                                        //Circular has dynamic height -> y = height/2
                                        gfx.DrawEllipse(pen, position, position, diameter, diameter);
                                        break;
                                    case DrawingScheme.Line:
                                        //Line has only 1 Pixel Height -> y = 0
                                        gfx.FillRectangle(brush, position, 0, 1, 1);
                                        break;
                                    case DrawingScheme.Square:
                                        //Square has dynamic height -> y = height/2
                                        gfx.DrawRectangle(pen, position, position, diameter, diameter);
                                        break;
                                }
                            }
                        }
                        position++;
                        diameter -= 2;
                    }
                    #endregion
                }

                return encryptedBitmap;
            }
            #endregion

            #region Decrypt
            /// <summary>
            /// Decrypt a encrypted <see cref="Bitmap"/> to a <see cref="string"/>
            /// </summary>
            /// <param name="salt">The salt used for the Encryption</param>
            /// <param name="encryptedBitmap">The <see cref="BmpPwd"/> Encrypted <see cref="Bitmap"/></param>
            /// <returns>The decrypted Text from the Bitmap</returns>
            public static string Decrypt(string salt, Bitmap encryptedBitmap) {
                return BmpPwd.Decrypt(salt, encryptedBitmap, new Cipher(), DrawingScheme.Circular);
            }

            /// <summary>
            /// Decrypt a encrypted <see cref="Bitmap"/> to a <see cref="string"/>
            /// </summary>
            /// <param name="salt">The salt used for the Encryption</param>
            /// <param name="encryptedBitmap">The <see cref="BmpPwd"/> Encrypted <see cref="Bitmap"/></param>
            /// <param name="cryptScheme">The Scheme/Interface Used for Decryption</param>
            /// <param name="drawingScheme">The <see cref="DrawingScheme"/> to use for Drawing the Image</param>
            /// <returns>The decrypted Text from the Bitmap</returns>
            public static string Decrypt(string salt, Bitmap encryptedBitmap, ICrypt cryptScheme, DrawingScheme drawingScheme) {
                int y = (encryptedBitmap.Width / 2) - 1;
                int width = encryptedBitmap.Width / 2;

                //Get all Pixels from Bitmap
                Color[] colors = new Color[width];
                for(int i = 0; i < width; i++) {
                    switch(drawingScheme) {
                        case DrawingScheme.Circular:
                            //Circular has dynamic height -> y = height/2
                            colors[i] = encryptedBitmap.GetPixel(i, y);
                            break;
                        case DrawingScheme.Line:
                            //Line has only 1 Pixel Height -> y = 0
                            colors[i] = encryptedBitmap.GetPixel(i, 0);
                            break;
                        case DrawingScheme.Square:
                            //Square has dynamic height -> y = height/2
                            colors[i] = encryptedBitmap.GetPixel(i, y);
                            break;
                    }
                }

                //Fill ASCII Values with Color's R Value (R = G = B)
                byte[] asciiValues = new byte[width];
                for(int i = 0; i < width; i++) {
                    asciiValues[i] = (byte)(colors[i].R / 2);
                }

                //Fill Char[] with the ASCII Value
                char[] chars = new char[width];
                for(int i = 0; i < width; i++) {
                    chars[i] = (char)asciiValues[i];
                }

                //Decrypt result
                string decrypted = new string(chars);
                decrypted = cryptScheme.Decrypt(salt, decrypted);

                return decrypted;
            }
            #endregion



            #region Helpers

            private static void DrawCorrectScheme(ref Graphics gfx, byte value, Brush brush, DrawingScheme drawingScheme) {

            }
            #endregion
        }
    }
}
