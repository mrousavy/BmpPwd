using Microsoft.VisualStudio.TestTools.UnitTesting;
using mrousavy.Cryptography;
using System.Drawing;

namespace BmpPwdTests {
    /// <summary>
    /// Test to find out if BmpPwd works
    /// </summary>
    [TestClass]
    public class UnitTest1 {
        private const string Salt = "~MyPass-Phrase/Salt~";
        private const string Text = "I'm trying to encrypt this text with normal Cipher vs BmpPwd encryption. 123";


        [TestMethod]
        public void TestBmpPwdLine() {
            Bitmap encrypted = BmpPwd.Encrypt(Salt, Text, new Cipher(), BmpPwd.DrawingScheme.Line, BmpPwd.ColorScheme.Rainbow);
            string decrypted = BmpPwd.Decrypt(Salt, encrypted, new Cipher(), BmpPwd.DrawingScheme.Line, BmpPwd.ColorScheme.Rainbow);

            Assert.AreEqual(Text, decrypted);
        }

        [TestMethod]
        public void TestBmpPwdCircle() {
            Bitmap encrypted = BmpPwd.Encrypt(Salt, Text, new Cipher(), BmpPwd.DrawingScheme.Circular, BmpPwd.ColorScheme.Rainbow);
            string decrypted = BmpPwd.Decrypt(Salt, encrypted, new Cipher(), BmpPwd.DrawingScheme.Circular, BmpPwd.ColorScheme.Rainbow);

            Assert.AreEqual(Text, decrypted);
        }

        [TestMethod]
        public void TestBmpPwdSquare() {
            Bitmap encrypted = BmpPwd.Encrypt(Salt, Text, new Cipher(), BmpPwd.DrawingScheme.Square, BmpPwd.ColorScheme.Rainbow);
            string decrypted = BmpPwd.Decrypt(Salt, encrypted, new Cipher(), BmpPwd.DrawingScheme.Square, BmpPwd.ColorScheme.Rainbow);

            Assert.AreEqual(Text, decrypted);
        }


        [TestMethod]
        public void TestNormalEncryption() {
            Cipher cipher = new Cipher();
            string encrypted = cipher.Encrypt(Salt, Text);
            string decrypted = cipher.Decrypt(Salt, encrypted);

            Assert.AreEqual(Text, decrypted);
        }
    }
}
