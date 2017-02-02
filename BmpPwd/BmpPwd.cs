﻿using System;
using System.Drawing;
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

                //Bitmap should be a Square -> Width&Height = Sqrt from total Pixels
                int length = (int)Math.Sqrt(encryptedText.Length);

                //Create Bitmap with correct Sizes
                Bitmap encryptedBitmap = new Bitmap(length, length);

                //Get all ASCII values
                byte[] asciiValues = Encoding.ASCII.GetBytes(encryptedText);

                using(Graphics gfx = Graphics.FromImage(encryptedBitmap)) {

                    int position = 0;

                    foreach(byte b in asciiValues) {
                        if(position > length)
                            position = 0;

                        using(SolidBrush brush = new SolidBrush(Color.FromArgb(b))) {
                            gfx.FillRectangle(brush, 0, 0, 1, 1);
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
