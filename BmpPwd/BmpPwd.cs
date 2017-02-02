﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace mrousavy {

    namespace Cryptography {
        /// <summary>
        /// En/De-crypt Text to a Bitmap
        /// </summary>
        public static class BmpPwd {
            #region Enccrypt
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
                        if(b > 255)
                            throw new Exception("ASCII value is too high for Pixel!");

                        //Set Pixel to ASCII Values (change Color.FromArg() values for different colors)
                        using(SolidBrush brush = new SolidBrush(Color.FromArgb(b * 2, 0, 0))) {
                            gfx.FillRectangle(brush, position, 0, 1, 1);
                        }
                        position++;
                    }
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
                return BmpPwd.Decrypt(salt, encryptedBitmap, new Cipher());
            }

            /// <summary>
            /// Decrypt a encrypted <see cref="Bitmap"/> to a <see cref="string"/>
            /// </summary>
            /// <param name="salt">The salt used for the Encryption</param>
            /// <param name="encryptedBitmap">The <see cref="BmpPwd"/> Encrypted <see cref="Bitmap"/></param>
            /// <param name="cryptSchema">The Schema/Interface Used for Decryption</param>
            /// <returns>The decrypted Text from the Bitmap</returns>
            public static string Decrypt(string salt, Bitmap encryptedBitmap, ICrypt cryptSchema) {
                //Get all Pixels from Bitmap
                Color[] colors = new Color[encryptedBitmap.Width];
                for(int i = 0; i < encryptedBitmap.Width; i++) {
                    colors[i] = encryptedBitmap.GetPixel(i, 0);
                }

                //Fill ASCII Values with Color's R Value (R = G = B)
                byte[] asciiValues = new byte[encryptedBitmap.Width];
                for(int i = 0; i < encryptedBitmap.Width; i++) {
                    asciiValues[i] = (byte)(colors[i].R / 2);
                }

                //Fill Char[] with the ASCII Value
                char[] chars = new char[encryptedBitmap.Width];
                for(int i = 0; i < encryptedBitmap.Width; i++) {
                    chars[i] = (char)asciiValues[i];
                }

                //Decrypt result
                string decrypted = new string(chars);
                decrypted = cryptSchema.Decrypt(salt, decrypted);

                return decrypted;
            }
            #endregion
        }
    }
}
