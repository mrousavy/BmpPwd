using Microsoft.VisualStudio.TestTools.UnitTesting;
using BmpPwd;

namespace BmpPwdTests
{
    /// <summary>
    ///     Test to find out if BmpPwd works
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        private const string Key = "~MyPass-Phrase/Key~2017:)--";
        private const string Text = "I'm trying to encrypt this text with normal Cipher vs BmpPwd encryption. 123";


        [TestMethod]
        public void TestBmpPwdLine()
        {
            var encrypted = BmpPwd.Encrypt(Key, Text, new Cipher(), BmpPwd.DrawingScheme.Line,
                BmpPwd.ColorScheme.Rainbow);
            string decrypted = BmpPwd.Decrypt(Key, encrypted, new Cipher(), BmpPwd.DrawingScheme.Line,
                BmpPwd.ColorScheme.Rainbow);

            Assert.AreEqual(Text, decrypted);
        }

        [TestMethod]
        public void TestBmpPwdCircle()
        {
            var encrypted = BmpPwd.Encrypt(Key, Text, new Cipher(), BmpPwd.DrawingScheme.Circular,
                BmpPwd.ColorScheme.Rainbow);
            string decrypted = BmpPwd.Decrypt(Key, encrypted, new Cipher(), BmpPwd.DrawingScheme.Circular,
                BmpPwd.ColorScheme.Rainbow);

            Assert.AreEqual(Text, decrypted);
        }

        [TestMethod]
        public void TestBmpPwdSquare()
        {
            var encrypted = BmpPwd.Encrypt(Key, Text, new Cipher(), BmpPwd.DrawingScheme.Square,
                BmpPwd.ColorScheme.Rainbow);
            string decrypted = BmpPwd.Decrypt(Key, encrypted, new Cipher(), BmpPwd.DrawingScheme.Square,
                BmpPwd.ColorScheme.Rainbow);

            Assert.AreEqual(Text, decrypted);
        }


        [TestMethod]
        public void TestNormalEncryption()
        {
            var cipher = new Cipher();
            string encrypted = cipher.Encrypt(Key, Text);
            string decrypted = cipher.Decrypt(Key, encrypted);

            Assert.AreEqual(Text, decrypted);
        }
    }
}