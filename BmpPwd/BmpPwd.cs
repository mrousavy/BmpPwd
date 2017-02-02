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
                return BmpPwd.Encrypt(salt, unencryptedText, new Cipher(), DrawingScheme.Square);
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

                //Set correct Width and Height values
                int width = 0, height = 0;
                SetWidthHeight(drawingScheme, ref height, ref width, encryptedText.Length);

                //Create Bitmap with correct Sizes
                Bitmap encryptedBitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

                //Get all ASCII values
                byte[] asciiValues = Encoding.ASCII.GetBytes(encryptedText);

                //Draw onto the Bitmap
                DrawCorrectScheme(encryptedBitmap, drawingScheme, asciiValues);

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
                return BmpPwd.Decrypt(salt, encryptedBitmap, new Cipher(), DrawingScheme.Square);
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
                //Set Width and Y for Image Reading
                int y = 0, width = 0;
                SetWidthY(drawingScheme, ref y, ref width, encryptedBitmap.Width);

                //Get all Colors from the Bitmap
                Color[] colors = GetPixelsFromBitmap(width, y, drawingScheme, encryptedBitmap);

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


            //Helpers for different Schemes or Configs
            #region Helpers
            /// <summary>
            /// Creates Graphics and draws all the Text (ASCII Values/bytes) onto a Bitmap with the correct <see cref="DrawingScheme"/>
            /// </summary>
            /// <param name="encryptedBitmap">The <see cref="Bitmap"/> to draw on</param>
            /// <param name="drawingScheme">The <see cref="DrawingScheme"/> to use for the drawing Process</param>
            /// <param name="asciiValues">All the ASCII values of the Text to draw</param>
            private static void DrawCorrectScheme(Bitmap encryptedBitmap, DrawingScheme drawingScheme, byte[] asciiValues) {
                //Initialize Graphics
                using(Graphics gfx = Graphics.FromImage(encryptedBitmap)) {
                    //Position & Diameter of Bitmap
                    int position = 0;
                    int diameter = encryptedBitmap.Width;

                    #region Drawing
                    //Loop through each Pixel
                    foreach(byte b in asciiValues) {

                        //Set Pixel to ASCII Values (change Color.FromArg() values for different colors)
                        using(SolidBrush brush = new SolidBrush(Color.FromArgb(b * 2, 0, 0))) {
                            using(Pen pen = new Pen(brush, 2)) {
                                //Draw different Schemes
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
            }

            /// <summary>
            /// Sets width and y values depending on the <see cref="DrawingScheme"/>
            /// </summary>
            /// <param name="scheme">The <see cref="DrawingScheme"/> to use</param>
            /// <param name="y">The y value to be set</param>
            /// <param name="width">The width value to be set</param>
            /// <param name="imageWidth">The <see cref="Bitmap"/>'s width</param>
            private static void SetWidthY(DrawingScheme scheme, ref int y, ref int width, int imageWidth) {
                //Set width and y values for different DrawingSchemes
                switch(scheme) {
                    case DrawingScheme.Circular:
                        //Circular has radius of textlength -> width = Bitmap.Width / 2
                        width = imageWidth / 2;
                        y = (imageWidth / 2) - 1;
                        break;
                    case DrawingScheme.Square:
                        //Square has dynamic height -> width = Bitmap.Width / 2
                        width = imageWidth / 2;
                        y = (imageWidth / 2) - 1;
                        break;
                    case DrawingScheme.Line:
                        //Line has only height of 1 (Index = 0)
                        width = imageWidth;
                        y = 0;
                        break;
                    default:
                        //Default is Line
                        width = imageWidth;
                        y = 0;
                        break;
                }

            }

            /// <summary>
            /// Sets width and height values depending on the <see cref="DrawingScheme"/>
            /// </summary>
            /// <param name="scheme">The <see cref="DrawingScheme"/> to use</param>
            /// <param name="height">The height value to be set</param>
            /// <param name="width">The width value to be set</param>
            /// <param name="textLength">The Encrypted Text's length</param>
            private static void SetWidthHeight(DrawingScheme scheme, ref int height, ref int width, int textLength) {
                //Set correct Width and Height values for different DrawingSchemes
                switch(scheme) {
                    case DrawingScheme.Circular:
                        //Circular has radius of bytes -> width & height = textlength * 2
                        width = textLength * 2;
                        height = textLength * 2;
                        break;
                    case DrawingScheme.Square:
                        //Square has dynamic height -> width & height = textlength * 2
                        width = textLength * 2;
                        height = textLength * 2;
                        break;
                    case DrawingScheme.Line:
                        //Line has only height of 1 (default values)
                        height = 1;
                        width = textLength;
                        break;
                    default:
                        //Default is Line
                        height = 1;
                        width = textLength;
                        break;
                }
            }

            /// <summary>
            /// Get all Pixels from a <see cref="Bitmap"/> into a <see cref="Color[]"/>
            /// </summary>
            /// <param name="width">Width of the Image</param>
            /// <param name="y">The Y index of the Image to read from</param>
            /// <param name="drawingScheme">The <see cref="DrawingScheme"/> to read</param>
            /// <param name="encryptedBitmap">The <see cref="Bitmap"/> to read from</param>
            /// <returns>The filled <see cref="Color[]"/></returns>
            private static Color[] GetPixelsFromBitmap(int width, int y, DrawingScheme drawingScheme, Bitmap encryptedBitmap) {
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

                return colors;
            }
            #endregion
        }
    }
}
