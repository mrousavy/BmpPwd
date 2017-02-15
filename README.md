# <img src="https://github.com/mrousavy/BmpPwd/blob/master/Images/Logo.png?raw=true" width="42"> BmpPwd
**BmpPwd** is a _Class Library_ for **en/decrypting** Text and visualizing it with a **System.Drawing.Bitmap**

Applications like [LaZagne](https://github.com/AlessandroZ/LaZagne) can easily find stored **(and encrypted)** Passwords from various Applications. To prevent this, **BmpPwd** will convert your Text with your En/Decryption Algorithm of choice _(Default: Cipher)_ to a _System.Drawing.Image_ which contains your Text.

To decrypt a **normally Encrypted Text**, a Program must know:
   * Location of the stored Text File
   * En/Decryption Algorithm
   * Pass-Phrase/Salt for En/Decryption Algorithm

To decrypt a **BmpPwd-Encrypted Image**, a Program must know:
   * Location of the stored Image
   * BmpPwd Encryption Shape/Drawing Scheme
   * BmpPwd Encryption Color Scheme
   * BmpPwd Decryption Method
   * En/Decryption Algorithm _(Default: Cipher)_
   * Pass-Phrase/Salt for En/Decryption Algorithm

[Download the Demo (.zip)](https://github.com/mrousavy/BmpPwd/releases/download/1.0.0.5/BmpPwdTest.zip)

# How to use

### 1. Add Binaries
   + NuGet
      * [BmpPwd is also available on NuGet!](https://www.nuget.org/packages/BmpPwd)   Install by typing `Install-Package BmpPwd` in NuGet Package Manager Console. (Or search for `BmpPwd` on NuGet)

   + Manually
      1. [Download the latest Library (.dll)](https://github.com/mrousavy/BmpPwd/releases/download/1.0.0.5/BmpPwd.dll)
      2. Add the .dll to your Project   (Right click `References` in the Project Tree View, click `Add References` and `Browse` to the `.dll` File)

### 2. Add the reference
   * C#:
   ```C#
   using mrousavy.Cryptography;
   ```
   
   * VB:
   ```VB
   Imports mrousavy.Cryptography
   ```

### 3. Encrypt a string
(Do not lossy-compress the Image!)
   * C#:
   ```C#
   //Needs Reference to System.Drawing dll
   System.Drawing.Bitmap encryptedBitmap = BmpPwd.Encrypt("MyPassword", "The string to be encrypted");
   ```
   
   * VB:
   ```VB
   //Needs Reference to System.Drawing dll
   Dim encryptedBitmap As System.Drawing.Bitmap = BmpPwd.Encrypt("MyPassword", "The string to be encrypted")
   ```
   
### 4. Decrypt an Image
   * C#:
   ```C#
   //Needs Reference to System.Drawing dll
   string decryptedText = BmpPwd.Decrypt("MyPassword", encryptedBitmap);
   ```
   
   * VB:
   ```VB
   //Needs Reference to System.Drawing dll
   Dim decryptedText As String = BmpPwd.Decrypt("MyPassword", encryptedBitmap)
   ```
   
### 5. Import your own Encryption
   * C#:
   ```C#
   public class MyCryptoClass : ICrypto {
        ...
   }
   ```
   
   * VB:
   ```VB
   Public Class MyCryptoClass
      Implements ICrypto
        ...
   End Class
   ```

### 6. Use custom Parameters:
   * C#:
   ```C#
   System.Drawing.Bitmap encryptedBitmap = BmpPwd.Encrypt("MyPassword", "The string to be encrypted", new MyCryptoClass(), BmpPwd.DrawingScheme.Square, BmpPwd.ColorScheme.BlueMixed);
   ```
   
   * VB:
   ```VB
   Dim encryptedBitmap As System.Drawing.Bitmap = BmpPwd.Encrypt("MyPassword", "The string to be encrypted", new MyCryptoClass(), BmpPwd.DrawingScheme.Square, BmpPwd.ColorScheme.BlueMixed)
   ```

# Screenshots
<img src="https://github.com/mrousavy/BmpPwd/blob/master/Images/Screenshots.gif?raw=true" alt="Screenshots in a gif">


<img src="https://github.com/mrousavy/BmpPwd/blob/master/Images/password.png?raw=true" alt="The Password 'password' in Rainbow/Square">

# Thanks for using BmpPwd!
