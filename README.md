# <img src="https://github.com/mrousavy/BmpPwd/blob/master/Images/Logo.png?raw=true" width="42"> BmpPwd
**BmpPwd** is a _Class Library_ for **en/decrypting** Text and visualizing it with a **System.Drawing.Bitmap**

[![NuGet](https://img.shields.io/nuget/dt/BmpPwd.svg)](https://www.nuget.org/packages/BmpPwd/)

Applications like [LaZagne](https://github.com/AlessandroZ/LaZagne) can easily find stored **(and encrypted)** Passwords from various Applications. To prevent this, **BmpPwd** will convert your Text with your En/Decryption Algorithm of choice _(Default: Cipher)_ to a _System.Drawing.Image_ which contains your Text.

To decrypt a **BmpPwd-Encrypted Image**, a Program must know:
   * Location of the stored Image
   * BmpPwd Encryption Shape/Drawing Scheme
   * BmpPwd Encryption Color Scheme
   * BmpPwd Decryption Method
   * En/Decryption Algorithm _(Default: Cipher)_
   * Pass-Phrase/Key for En/Decryption Algorithm
   * Random Salt for En/Decryption Algorithm

[Download the Demo (.zip)](https://github.com/mrousavy/BmpPwd/releases/download/1.0.0.5/BmpPwdTest.zip)

# How to use

### 1. Add Binaries
   + NuGet
      * [BmpPwd is also available on NuGet!](https://www.nuget.org/packages/BmpPwd)   Install via NuGet Package Manager Console:
      ```nuget
      PM> Install-Package BmpPwd -Version 1.0.0.7-fix
      ```
      or by browsing NuGet Marketplace for `BmpPwd`

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
   ' Needs Reference to System.Drawing dll
   Dim encryptedBitmap As System.Drawing.Bitmap
   encryptedBitmap = BmpPwd.Encrypt("MyPassword", "The string to be encrypted")
   ```

### 4. Decrypt an Image
   * C#:
   ```C#
   //Needs Reference to System.Drawing dll
   string decryptedText = BmpPwd.Decrypt("MyPassword", encryptedBitmap);
   ```

   * VB:
   ```VB
   ' Needs Reference to System.Drawing dll
   Dim decryptedText As String
   decryptedText = BmpPwd.Decrypt("MyPassword", encryptedBitmap)
   ```

### 5. Import your own Encryption
   * C#:
   ```C#
   public class MyCryptoClass : ICrypto {
       // Implement Encrypt(..) and Decrypt(..) here
   }
   ```

   * VB:
   ```VB
   Public Class MyCryptoClass
       Implements ICrypto
           ' Implement Encrypt(..) and Decrypt(..) here
   End Class
   ```

### 6. Use custom Parameters:
   * C#:
   ```C#
   System.Drawing.Bitmap encryptedBitmap = BmpPwd.Encrypt("MyPassword",
                                                          "The string to be encrypted",
                                                          new MyCryptoClass(),
                                                          BmpPwd.DrawingScheme.Square,
                                                          BmpPwd.ColorScheme.BlueMixed);
   ```

   * VB:
   ```VB
   Dim encryptedBitmap As System.Drawing.Bitmap
   encryptedBitmap = BmpPwd.Encrypt(
       "MyPassword",
       "The string to be encrypted",
       new MyCryptoClass(),
       BmpPwd.DrawingScheme.Square,
       BmpPwd.ColorScheme.BlueMixed)
   ```

# Screenshots
<img src="https://github.com/mrousavy/BmpPwd/blob/master/Images/Screenshots.gif?raw=true" alt="Screenshots in a gif">


<img src="https://github.com/mrousavy/BmpPwd/blob/master/Images/password.png?raw=true" alt="The Password 'password' in Rainbow/Square">

# See also
## Performance Benchmark
<img src="https://github.com/mrousavy/BmpPwd/blob/master/Images/Benchmark.png?raw=true" alt="Benchmark (BmpPwd: 23ms | Normal Text: 16ms)">

**BmpPwd** does perform nearly as good as normal **Cipher string encryption**.

Use `Line` if you care about small performance benefits.

## Lossy compression
Do **not** lossy-compress the encrypted Bitmap or chances are the Bitmap can't get decrypted again. If you want to save the Image, do not use `.jpg`.


# Thanks for using BmpPwd!
> License: [MIT](https://github.com/mrousavy/BmpPwd/blob/master/LICENSE) | mrousavy 2017
