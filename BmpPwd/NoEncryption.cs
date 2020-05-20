namespace BmpPwd
{
    /// <summary>
    ///     ICrypt implementation without any en/ee-cryption
    /// </summary>
    public class NoEncryption : ICrypt
    {
        public string Decrypt(string key, string encryptedText)
        {
            return encryptedText;
        }

        public string Encrypt(string key, string unencryptedText)
        {
            return unencryptedText;
        }
    }
}