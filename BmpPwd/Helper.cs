using System.Drawing;
using static mrousavy.Cryptography.BmpPwd;

namespace mrousavy
{
    /// <summary>
    ///     Helper class for System.Drawing functions
    /// </summary>
    internal static class Helper
    {
        /// <summary>
        ///     Creates Graphics and draws all the Text (ASCII Values/bytes) onto a Image with the correct
        ///     <see cref="DrawingScheme" />
        /// </summary>
        /// <param name="encryptedImage">The <see cref="Image" /> to draw on</param>
        /// <param name="drawingScheme">The <see cref="DrawingScheme" /> to use for the drawing Process</param>
        /// <param name="colorScheme">The <see cref="ColorScheme" /> to apply on the Color picking</param>
        /// <param name="asciiValues">All the ASCII values of the Text to draw</param>
        internal static void DrawCorrectScheme(Image encryptedImage, DrawingScheme drawingScheme,
            ColorScheme colorScheme, byte[] asciiValues)
        {
            //Initialize Graphics
            using (var gfx = Graphics.FromImage(encryptedImage))
            {
                //Position & Diameter of Image
                int position = 0;
                int diameter = encryptedImage.Width;

                #region Drawing

                //Loop through each Pixel
                foreach (byte b in asciiValues)
                {
                    //The correct color used for drawing (b * 2 because b's max value is 128)
                    var color = GetColor(colorScheme, b);

                    //Set Pixel to ASCII Values (change Color.FromArg() values for different colors)
                    using (var brush = new SolidBrush(color))
                    {
                        //Draw different Schemes
                        switch (drawingScheme)
                        {
                            case DrawingScheme.Circular:
                                //Circular has dynamic height -> y = height/2
                                gfx.FillEllipse(brush, position, position, diameter, diameter);
                                break;
                            case DrawingScheme.Line:
                                //Line has only 1 Pixel Height -> y = 0
                                gfx.FillRectangle(brush, position, 0, 1, 1);
                                break;
                            case DrawingScheme.Square:
                                //Square has dynamic height -> y = height/2
                                gfx.FillRectangle(brush, position, position, diameter, diameter);
                                break;
                        }
                    }
                    position++;
                    diameter -= 2;
                }

                #endregion
            }
        }

        /// <summary>
        ///     Gets the correct <see cref="Color" />
        /// </summary>
        /// <param name="colorScheme">The <see cref="ColorScheme" /> to apply on the Color picking</param>
        /// <param name="b">The byte defining the dynamic Color</param>
        /// <returns>The correct <see cref="Color" /></returns>
        internal static Color GetColor(ColorScheme colorScheme, byte b)
        {
            //For Mixed Colors
            int rnd1 = Random.Next(0, b);
            int rnd2 = Random.Next(0, b);
            int rainbow1 = Random.Next(0, 255);
            int rainbow2 = Random.Next(0, 255);

            switch (colorScheme)
            {
                case ColorScheme.Greyscale:
                    return Color.FromArgb(b, b, b);
                case ColorScheme.RedOnly:
                    return Color.FromArgb(b, 0, 0);
                case ColorScheme.GreenOnly:
                    return Color.FromArgb(0, b, 0);
                case ColorScheme.BlueOnly:
                    return Color.FromArgb(0, 0, b);
                case ColorScheme.RedMixed:
                    return Color.FromArgb(b, rnd1, rnd2);
                case ColorScheme.GreenMixed:
                    return Color.FromArgb(rnd1, b, rnd2);
                case ColorScheme.BlueMixed:
                    return Color.FromArgb(rnd1, rnd2, b);
                case ColorScheme.Rainbow:
                    return Color.FromArgb(b, rainbow1, rainbow2);
                default:
                    return Color.FromArgb(b, b, b);
            }
        }

        /// <summary>
        ///     Gets the correct ASCII Value from the <see cref="Color" /> Input
        /// </summary>
        /// <param name="colorScheme">The <see cref="ColorScheme" /> to apply on the Color picking</param>
        /// <param name="color">The <see cref="Color" /> where the ASCII Value should be picked</param>
        /// <returns>The correct <see cref="Color" /></returns>
        internal static byte GetAsciiValue(ColorScheme colorScheme, Color color)
        {
            switch (colorScheme)
            {
                case ColorScheme.Greyscale:
                    return color.R;
                case ColorScheme.RedOnly:
                    return color.R;
                case ColorScheme.GreenOnly:
                    return color.G;
                case ColorScheme.BlueOnly:
                    return color.B;
                case ColorScheme.RedMixed:
                    return color.R;
                case ColorScheme.GreenMixed:
                    return color.G;
                case ColorScheme.BlueMixed:
                    return color.B;
                case ColorScheme.Rainbow:
                    return color.R;
                default:
                    return color.R;
            }
        }

        /// <summary>
        ///     Sets width and y values depending on the <see cref="DrawingScheme" />
        /// </summary>
        /// <param name="scheme">The <see cref="DrawingScheme" /> to use</param>
        /// <param name="y">The y value to be set</param>
        /// <param name="width">The width value to be set</param>
        /// <param name="imageWidth">The <see cref="Image" />'s width</param>
        internal static void SetWidthY(DrawingScheme scheme, out int y, out int width, int imageWidth)
        {
            //Set width and y values for different DrawingSchemes
            switch (scheme)
            {
                case DrawingScheme.Circular:
                    //Circular has radius of textlength -> width = Image.Width / 2
                    width = imageWidth / 2;
                    y = imageWidth / 2;
                    break;
                case DrawingScheme.Square:
                    //Square has dynamic height -> width = Image.Width / 2
                    width = imageWidth / 2;
                    y = imageWidth / 2 - 1;
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
        ///     Sets width and height values depending on the <see cref="DrawingScheme" />
        /// </summary>
        /// <param name="scheme">The <see cref="DrawingScheme" /> to use</param>
        /// <param name="height">The height value to be set</param>
        /// <param name="width">The width value to be set</param>
        /// <param name="textLength">The Encrypted Text's length</param>
        internal static void SetWidthHeight(DrawingScheme scheme, out int height, out int width, int textLength)
        {
            //Set correct Width and Height values for different DrawingSchemes
            switch (scheme)
            {
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
        ///     Get all Pixels from a <see cref="Image" /> into a <see cref="Color" />[]
        /// </summary>
        /// <param name="width">Width of the Image</param>
        /// <param name="y">The Y index of the Image to read from</param>
        /// <param name="drawingScheme">The <see cref="DrawingScheme" /> to read</param>
        /// <param name="encryptedImage">The <see cref="Image" /> to read from</param>
        /// <param name="colorScheme">The <see cref="ColorScheme" /> to apply on the Color picking</param>
        /// <returns>The filled <see cref="Color" />[]</returns>
        internal static Color[] GetPixelsFromImage(int width, int y, DrawingScheme drawingScheme,
            ColorScheme colorScheme, Image encryptedImage)
        {
            //Get all Pixels from Image
            Color[] colors = new Color[width];
            using (var bitmap = new Bitmap(encryptedImage))
            {
                for (int i = 0; i < width; i++)
                    switch (drawingScheme)
                    {
                        case DrawingScheme.Circular:
                            //Circular has dynamic height -> y = height/2
                            colors[i] = bitmap.GetPixel(i, y);
                            break;
                        case DrawingScheme.Square:
                            //Square has dynamic height -> y = height/2
                            colors[i] = bitmap.GetPixel(i, y);
                            break;
                        case DrawingScheme.Line:
                            //Line has only 1 Pixel Height -> y = 0
                            colors[i] = bitmap.GetPixel(i, 0);
                            break;
                    }
            }

            return colors;
        }
    }
}