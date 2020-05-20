using System.Drawing;

namespace BmpPwd
{
    /// <summary>
    ///     Helper class for System.Drawing functions
    /// </summary>
    internal static class Helper
    {
        /// <summary>
        ///     Creates Graphics and draws all the Text (ASCII Values/bytes) onto a Image with the correct
        ///     <see cref="BmpPwd.DrawingScheme" />
        /// </summary>
        /// <param name="encryptedImage">The <see cref="Image" /> to draw on</param>
        /// <param name="drawingScheme">The <see cref="BmpPwd.DrawingScheme" /> to use for the drawing Process</param>
        /// <param name="colorScheme">The <see cref="BmpPwd.ColorScheme" /> to apply on the Color picking</param>
        /// <param name="asciiValues">All the ASCII values of the Text to draw</param>
        internal static void DrawCorrectScheme(Image encryptedImage, BmpPwd.DrawingScheme drawingScheme,
            BmpPwd.ColorScheme colorScheme, byte[] asciiValues)
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
                            case BmpPwd.DrawingScheme.Circular:
                                //Circular has dynamic height -> y = height/2
                                gfx.FillEllipse(brush, position, position, diameter, diameter);
                                break;
                            case BmpPwd.DrawingScheme.Line:
                                //Line has only 1 Pixel Height -> y = 0
                                gfx.FillRectangle(brush, position, 0, 1, 1);
                                break;
                            case BmpPwd.DrawingScheme.Square:
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
        /// <param name="colorScheme">The <see cref="BmpPwd.ColorScheme" /> to apply on the Color picking</param>
        /// <param name="b">The byte defining the dynamic Color</param>
        /// <returns>The correct <see cref="Color" /></returns>
        internal static Color GetColor(BmpPwd.ColorScheme colorScheme, byte b)
        {
            //For Mixed Colors
            int rnd1 = BmpPwd.Random.Next(0, b);
            int rnd2 = BmpPwd.Random.Next(0, b);
            int rainbow1 = BmpPwd.Random.Next(0, 255);
            int rainbow2 = BmpPwd.Random.Next(0, 255);
            int value = b * 2;

            switch (colorScheme)
            {
                case BmpPwd.ColorScheme.Greyscale:
                    return Color.FromArgb(value, value, value);
                case BmpPwd.ColorScheme.RedOnly:
                    return Color.FromArgb(value, 0, 0);
                case BmpPwd.ColorScheme.GreenOnly:
                    return Color.FromArgb(0, value, 0);
                case BmpPwd.ColorScheme.BlueOnly:
                    return Color.FromArgb(0, 0, value);
                case BmpPwd.ColorScheme.RedMixed:
                    return Color.FromArgb(value, rnd1, rnd2);
                case BmpPwd.ColorScheme.GreenMixed:
                    return Color.FromArgb(rnd1, value, rnd2);
                case BmpPwd.ColorScheme.BlueMixed:
                    return Color.FromArgb(rnd1, rnd2, value);
                case BmpPwd.ColorScheme.Rainbow:
                    return Color.FromArgb(value, rainbow1, rainbow2);
                default:
                    return Color.FromArgb(value, value, value);
            }
        }

        /// <summary>
        ///     Gets the correct ASCII Value from the <see cref="Color" /> Input
        /// </summary>
        /// <param name="colorScheme">The <see cref="BmpPwd.ColorScheme" /> to apply on the Color picking</param>
        /// <param name="color">The <see cref="Color" /> where the ASCII Value should be picked</param>
        /// <returns>The correct <see cref="Color" /></returns>
        internal static byte GetAsciiValue(BmpPwd.ColorScheme colorScheme, Color color)
        {
            switch (colorScheme)
            {
                case BmpPwd.ColorScheme.Greyscale:
                    return color.R;
                case BmpPwd.ColorScheme.RedOnly:
                    return color.R;
                case BmpPwd.ColorScheme.GreenOnly:
                    return color.G;
                case BmpPwd.ColorScheme.BlueOnly:
                    return color.B;
                case BmpPwd.ColorScheme.RedMixed:
                    return color.R;
                case BmpPwd.ColorScheme.GreenMixed:
                    return color.G;
                case BmpPwd.ColorScheme.BlueMixed:
                    return color.B;
                case BmpPwd.ColorScheme.Rainbow:
                    return color.R;
                default:
                    return color.R;
            }
        }

        /// <summary>
        ///     Sets width and y values depending on the <see cref="BmpPwd.DrawingScheme" />
        /// </summary>
        /// <param name="scheme">The <see cref="BmpPwd.DrawingScheme" /> to use</param>
        /// <param name="y">The y value to be set</param>
        /// <param name="width">The width value to be set</param>
        /// <param name="imageWidth">The <see cref="Image" />'s width</param>
        internal static void SetWidthY(BmpPwd.DrawingScheme scheme, out int y, out int width, int imageWidth)
        {
            //Set width and y values for different DrawingSchemes
            switch (scheme)
            {
                case BmpPwd.DrawingScheme.Circular:
                    //Circular has radius of textlength -> width = Image.Width / 2
                    width = imageWidth / 2;
                    y = imageWidth / 2;
                    break;
                case BmpPwd.DrawingScheme.Square:
                    //Square has dynamic height -> width = Image.Width / 2
                    width = imageWidth / 2;
                    y = imageWidth / 2 - 1;
                    break;
                case BmpPwd.DrawingScheme.Line:
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
        ///     Sets width and height values depending on the <see cref="BmpPwd.DrawingScheme" />
        /// </summary>
        /// <param name="scheme">The <see cref="BmpPwd.DrawingScheme" /> to use</param>
        /// <param name="height">The height value to be set</param>
        /// <param name="width">The width value to be set</param>
        /// <param name="textLength">The Encrypted Text's length</param>
        internal static void SetWidthHeight(BmpPwd.DrawingScheme scheme, out int height, out int width, int textLength)
        {
            //Set correct Width and Height values for different DrawingSchemes
            switch (scheme)
            {
                case BmpPwd.DrawingScheme.Circular:
                    //Circular has radius of bytes -> width & height = textlength * 2
                    width = textLength * 2;
                    height = textLength * 2;
                    break;
                case BmpPwd.DrawingScheme.Square:
                    //Square has dynamic height -> width & height = textlength * 2
                    width = textLength * 2;
                    height = textLength * 2;
                    break;
                case BmpPwd.DrawingScheme.Line:
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
        /// <param name="drawingScheme">The <see cref="BmpPwd.DrawingScheme" /> to read</param>
        /// <param name="encryptedImage">The <see cref="Image" /> to read from</param>
        /// <param name="colorScheme">The <see cref="BmpPwd.ColorScheme" /> to apply on the Color picking</param>
        /// <returns>The filled <see cref="Color" />[]</returns>
        internal static Color[] GetPixelsFromImage(int width, int y, BmpPwd.DrawingScheme drawingScheme,
            BmpPwd.ColorScheme colorScheme, Image encryptedImage)
        {
            //Get all Pixels from Image
            var colors = new Color[width];
            using (var bitmap = new Bitmap(encryptedImage))
            {
                for (int i = 0; i < width; i++)
                    switch (drawingScheme)
                    {
                        case BmpPwd.DrawingScheme.Circular:
                            //Circular has dynamic height -> y = height/2
                            colors[i] = bitmap.GetPixel(i, y);
                            break;
                        case BmpPwd.DrawingScheme.Square:
                            //Square has dynamic height -> y = height/2
                            colors[i] = bitmap.GetPixel(i, y);
                            break;
                        case BmpPwd.DrawingScheme.Line:
                            //Line has only 1 Pixel Height -> y = 0
                            colors[i] = bitmap.GetPixel(i, 0);
                            break;
                    }
            }

            return colors;
        }
    }
}