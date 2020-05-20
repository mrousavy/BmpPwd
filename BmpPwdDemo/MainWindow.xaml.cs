//Reference BmpPwd DLL

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BmpPwd;
using Microsoft.Win32;

namespace BmpPwdDemo
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private ColorScheme _colorScheme = ColorScheme.Greyscale;
        private DrawingScheme _scheme = DrawingScheme.Circular;


        public MainWindow()
        {
            InitializeComponent();

            UnencryptedBox.Focus();
        }

        private void EncryptButton_OnClick(object sender, RoutedEventArgs e)
        {
            Encrypt();
        }

        private void UnencryptedBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Encrypt();
            }
        }

        public static ImageSource ByteToImage(byte[] imageData)
        {
            var biImg = new BitmapImage();
            var ms = new MemoryStream(imageData);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();

            ImageSource imgSrc = biImg;

            return imgSrc;
        }


        private void Encrypt()
        {
            if (string.IsNullOrWhiteSpace(UnencryptedBox.Text))
            {
                return;
            }

            var encryptedBitmap =
                BmpPwd.BmpPwd.Encrypt("MyPassword", UnencryptedBox.Text, new Cipher(), _scheme, _colorScheme);

            //Convert Bitmap to ImageSource
            using (var memory = new MemoryStream())
            {
                encryptedBitmap.Save(memory, ImageFormat.Png);
                var bytes = memory.ToArray();
                EncryptedImage.Source = ByteToImage(bytes);
            }
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var sfd = new SaveFileDialog
                {
                    Filter = "Image files (*.png)|*.png",
                    FilterIndex = 2,
                    RestoreDirectory = true
                };
                if (sfd.ShowDialog() == true)
                {
                    string path = sfd.FileName;

                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create((BitmapSource) EncryptedImage.Source));
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        encoder.Save(stream);
                    }
                }
            } catch (Exception ex)
            {
                MessageBox.Show($"Could not save Image!\n{ex.Message}", "Error saving Image");
            }
        }

        private void OpenButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var sfd = new OpenFileDialog
                {
                    Filter = "Image files (*.png)|*.png",
                    FilterIndex = 2,
                    RestoreDirectory = true
                };
                if (sfd.ShowDialog() == true)
                {
                    string path = sfd.FileName;

                    var bytes = File.ReadAllBytes(path);

                    var biImg = new BitmapImage();
                    using (var ms = new MemoryStream(bytes))
                    {
                        biImg.BeginInit();
                        biImg.StreamSource = ms;
                        biImg.EndInit();

                        EncryptedImage.Source = biImg;

                        using (var outStream = new MemoryStream())
                        {
                            BitmapEncoder enc = new BmpBitmapEncoder();
                            enc.Frames.Add(BitmapFrame.Create(biImg));
                            enc.Save(outStream);
                            var bitmap = new Bitmap(outStream);

                            DecryptedBox.Text = BmpPwd.BmpPwd.Decrypt("MyPassword", new Bitmap(bitmap), new Cipher(),
                                _scheme,
                                _colorScheme);

                            MessageBox.Show("Decrypted: " + DecryptedBox.Text, "Successfully decrypted!");
                        }
                    }
                }
            } catch (Exception ex)
            {
                MessageBox.Show($"Could not open Image!\n{ex.Message}", "Error opening Image");
            }
        }


        private void FormBox_Changed(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var cbox = sender as ComboBox;
                _scheme = (DrawingScheme) Enum.Parse(typeof(DrawingScheme),
                    (cbox.SelectedItem as ComboBoxItem).Content as string);
            } catch
            {
                // ignored
            }
        }

        private void ColorBox_Changed(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var cbox = sender as ComboBox;
                _colorScheme = (ColorScheme) Enum.Parse(typeof(ColorScheme),
                    ((cbox.SelectedItem as ComboBoxItem).Content as string).Replace(" ", ""));
            } catch
            {
                // ignored
            }
        }
    }
}