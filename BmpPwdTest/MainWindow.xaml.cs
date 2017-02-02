﻿//Reference BmpPwd DLL
using mrousavy.Cryptography;

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BmpPwdTest {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            UnencryptedBox.Focus();
        }

        private void EncryptButton_OnClick(object sender, RoutedEventArgs e) {
            Encrypt();
        }

        private void UnencryptedBox_OnKeyDown(object sender, KeyEventArgs e) {
            if(e.Key == Key.Enter)
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
            if(string.IsNullOrWhiteSpace(UnencryptedBox.Text))
                return;

            Bitmap encryptedBitmap = BmpPwd.Encrypt("MyPassword", UnencryptedBox.Text);

            //Convert Bitmap to ImageSource
            string outputFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "file.png");
            using(MemoryStream memory = new MemoryStream()) {
                using(FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite)) {
                    encryptedBitmap.Save(memory, ImageFormat.Png);
                    byte[] bytes = memory.ToArray();

                    fs.Write(bytes, 0, bytes.Length);
                    EncryptedImage.Source = ByteToImage(bytes);
                }
            }

            Cipher c = new Cipher();
            string enc = c.Encrypt("MyPassword", UnencryptedBox.Text);
            string dec = c.Decrypt("MyPassword", enc);

            MessageBox.Show("Decrypted: " + BmpPwd.Decrypt("MyPassword", encryptedBitmap));
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e) {
            throw new NotImplementedException();
        }
    }
}
