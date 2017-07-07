//Reference BmpPwd DLL
using Microsoft.Win32;
using mrousavy.Cryptography;

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BmpPwdDemo {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        BmpPwd.DrawingScheme scheme = BmpPwd.DrawingScheme.Circular;
        BmpPwd.ColorScheme colorScheme = BmpPwd.ColorScheme.Greyscale;


        public MainWindow() {
            InitializeComponent();

            UnencryptedBox.Focus();
        }

        private void EncryptButton_OnClick(object sender, RoutedEventArgs e) {
            Encrypt();
        }

        private void UnencryptedBox_OnKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter)
                Encrypt();
        }

        public static ImageSource ByteToImage(byte[] imageData) {
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();

            ImageSource imgSrc = biImg;

            return imgSrc;
        }


        private void Encrypt() {
            if (string.IsNullOrWhiteSpace(UnencryptedBox.Text))
                return;

            Bitmap encryptedBitmap = BmpPwd.Encrypt("MyPassword", UnencryptedBox.Text, new Cipher(), scheme, colorScheme);

            //Convert Bitmap to ImageSource
            using (MemoryStream memory = new MemoryStream()) {
                encryptedBitmap.Save(memory, ImageFormat.Png);
                byte[] bytes = memory.ToArray();
                EncryptedImage.Source = ByteToImage(bytes);
            }
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e) {
            try {
                string path;

                SaveFileDialog sfd = new SaveFileDialog() {
                    Filter = "Image files (*.png)|*.png",
                    FilterIndex = 2,
                    RestoreDirectory = true
                };
                if (sfd.ShowDialog() == true) {
                    path = sfd.FileName;

                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create((BitmapSource)EncryptedImage.Source));
                    using (FileStream stream = new FileStream(path, FileMode.Create))
                        encoder.Save(stream);
                }
            } catch (Exception ex) {
                MessageBox.Show($"Could not save Image!\n{ex.Message}", "Error saving Image");
            }
        }
        private void OpenButton_OnClick(object sender, RoutedEventArgs e) {
            try {
                string path;

                OpenFileDialog sfd = new OpenFileDialog() {
                    Filter = "Image files (*.png)|*.png",
                    FilterIndex = 2,
                    RestoreDirectory = true
                };
                if (sfd.ShowDialog() == true) {
                    path = sfd.FileName;

                    byte[] bytes = File.ReadAllBytes(path);

                    BitmapImage biImg = new BitmapImage();
                    using (MemoryStream ms = new MemoryStream(bytes)) {
                        biImg.BeginInit();
                        biImg.StreamSource = ms;
                        biImg.EndInit();

                        EncryptedImage.Source = biImg;

                        using (MemoryStream outStream = new MemoryStream()) {
                            BitmapEncoder enc = new BmpBitmapEncoder();
                            enc.Frames.Add(BitmapFrame.Create(biImg));
                            enc.Save(outStream);
                            Bitmap bitmap = new Bitmap(outStream);

                            DecryptedBox.Text = BmpPwd.Decrypt("MyPassword", new Bitmap(bitmap), new Cipher(), scheme, colorScheme);

                            MessageBox.Show("Decrypted: " + DecryptedBox.Text, "Successfully decrypted!");
                        }
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show($"Could not open Image!\n{ex.Message}", "Error opening Image");
            }
        }


        private void FormBox_Changed(object sender, SelectionChangedEventArgs e) {
            try {
                ComboBox cbox = sender as ComboBox;
                scheme = (BmpPwd.DrawingScheme)Enum.Parse(typeof(BmpPwd.DrawingScheme), (cbox.SelectedItem as ComboBoxItem).Content as string);
            } catch { }
        }

        private void ColorBox_Changed(object sender, SelectionChangedEventArgs e) {
            try {
                ComboBox cbox = sender as ComboBox;
                colorScheme = (BmpPwd.ColorScheme)Enum.Parse(typeof(BmpPwd.ColorScheme), ((cbox.SelectedItem as ComboBoxItem).Content as string).Replace(" ", ""));
            } catch { }
        }
    }
}
