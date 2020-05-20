using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace BmpPwd
{
    /// <summary>
    ///     Color Scheme/Style for Image Drawing
    /// </summary>
    public enum ColorScheme
    {
        Greyscale,
        RedOnly,
        GreenOnly,
        BlueOnly,
        RedMixed,
        GreenMixed,
        BlueMixed,
        Rainbow
    }

    /// <summary>
    ///     Drawing Scheme/Style for Image Drawing
    ///     (Use <see cref="DrawingScheme.Line" /> for faster encryption and minimal storage usage)
    /// </summary>
    public enum DrawingScheme
    {
        Line,
        Circular,
        Square
    }

    /// <summary>
    ///     Text and Image Cryptography
    /// </summary>
    public static class BmpPwd
    {

        internal static Random Random = new Random();

        //Text & Image Encryption

        #region Encrypt

        /// <summary>
        ///     Encrypt Text to an <see cref="Image"/> with default <see cref="Cipher"/> Encryption
        /// </summary>
        /// <param name="key">The key used for the Encryption</param>
        /// <param name="unencryptedText">The original unencrypted Text</param>
        /// <returns>The Encrypted Image</returns>
        public static Image Encrypt(string key, string unencryptedText)
        {
            return Encrypt(key, unencryptedText, new Cipher());
        }

        /// <summary>
        ///     Encrypt Text to an Image with a custom encryption implementing <see cref="ICrypt"/>
        /// </summary>
        /// <param name="key">The key used for the Encryption</param>
        /// <param name="unencryptedText">The original unencrypted Text</param>
        /// <param name="cryptScheme">The Scheme/Interface Used for Encryption</param>
        /// <param name="drawingScheme">The <see cref="DrawingScheme" /> to use for Drawing the Image</param>
        /// <param name="colorScheme">The <see cref="ColorScheme" /> to use for colorizing the Image</param>
        /// <returns>The Encrypted <see cref="Image"/></returns>
        public static Image Encrypt(
            string key,
            string unencryptedText,
            ICrypt cryptScheme,
            DrawingScheme drawingScheme = DrawingScheme.Line,
            ColorScheme colorScheme = ColorScheme.RedMixed)
        {
            if (cryptScheme == null)
                cryptScheme = new Cipher();

            //Get the encrypted Text
            string encryptedText = cryptScheme.Encrypt(key, unencryptedText);

            //Get all ASCII values
            var asciiValues = Encoding.Unicode.GetBytes(encryptedText);

            //Set correct Width and Height values
            Helper.SetWidthHeight(drawingScheme, out int height, out int width, asciiValues.Length);

            //Create Image with correct Sizes
            var encryptedImage = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            //Draw onto the Image
            Helper.DrawCorrectScheme(encryptedImage, drawingScheme, colorScheme, asciiValues);

            return encryptedImage;
        }

        #endregion

        //Text & Image Decryption

        #region Decrypt

        /// <summary>
        ///     Decrypt an encrypted <see cref="Image" /> to a <see cref="string" />
        /// </summary>
        /// <param name="key">The key used for the Encryption</param>
        /// <param name="encryptedImage">The <see cref="BmpPwd" /> Encrypted <see cref="Image" /></param>
        /// <returns>The decrypted Text from the Image</returns>
        public static string Decrypt(string key, Image encryptedImage)
        {
            return Decrypt(key, encryptedImage, new Cipher());
        }

        /// <summary>
        ///     Decrypt a encrypted <see cref="Image" /> to a <see cref="string" />
        /// </summary>
        /// <param name="key">The key used for the Encryption</param>
        /// <param name="encryptedImage">The <see cref="BmpPwd" /> Encrypted <see cref="Image" /></param>
        /// <param name="cryptScheme">The Scheme/Interface Used for Decryption</param>
        /// <param name="drawingScheme">The <see cref="DrawingScheme" /> to use for Drawing the Image</param>
        /// <param name="colorScheme">The <see cref="ColorScheme" /> to use for colorizing the Image</param>
        /// <returns>The decrypted Text from the Image</returns>
        public static string Decrypt(
            string key,
            Image encryptedImage,
            ICrypt cryptScheme,
            DrawingScheme drawingScheme = DrawingScheme.Line,
            ColorScheme colorScheme = ColorScheme.RedMixed)
        {
            if (cryptScheme == null)
                cryptScheme = new Cipher();

            //Set Width and Y for Image Reading
            Helper.SetWidthY(drawingScheme, out int y, out int width, encryptedImage.Width);

            //Get all Colors from the Image
            var colors = Helper.GetPixelsFromImage(width, y, drawingScheme, colorScheme, encryptedImage);

            //Fill ASCII Values with Color's R Value (R = G = B)
            var asciiValues = new byte[width];
            for (int i = 0; i < width; i++) asciiValues[i] = (byte)(Helper.GetAsciiValue(colorScheme, colors[i]) / 2);

            //Decrypt result
            string decrypted = Encoding.Unicode.GetString(asciiValues);
            decrypted = cryptScheme.Decrypt(key, decrypted);

            return decrypted;
        }

        #endregion
    }
}