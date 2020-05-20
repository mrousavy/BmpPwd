# <img src="https://github.com/mrousavy/BmpPwd/blob/master/Images/Logo.png?raw=true" width="42"> BmpPwd
**BmpPwd** is a .NET Standard 2.0 class library for encrypting and decrypting plain text to an Image (see: [`System.Drawing.Common`](https://www.nuget.org/packages/System.Drawing.Common/))

[![NuGet](https://img.shields.io/nuget/dt/BmpPwd.svg)](https://www.nuget.org/packages/BmpPwd/)

<a href='https://ko-fi.com/F1F8CLXG' target='_blank'><img height='36' style='border:0px;height:36px;' src='https://az743702.vo.msecnd.net/cdn/kofi2.png?v=0' border='0' alt='Buy Me a Coffee at ko-fi.com' /></a>

Applications like [LaZagne](https://github.com/AlessandroZ/LaZagne) can easily find stored **(and encrypted)** Passwords from various Applications. To prevent this, **BmpPwd** will convert your Text with your En/Decryption Algorithm of choice _(Default: Cipher)_ to a _System.Drawing.Image_ which contains your Text.

To decrypt a **BmpPwd-Encrypted Image**, a Program must know:
   * Location of the stored Image
   * BmpPwd Encryption Shape/Drawing Scheme
   * BmpPwd Encryption Color Scheme
   * BmpPwd Decryption Method
   * En/Decryption Algorithm _(Default: Cipher)_
   * Pass-Phrase/Key for En/Decryption Algorithm
   * Random Salt for En/Decryption Algorithm

[**Download the Demo Application** (.zip)](https://github.com/mrousavy/BmpPwd/releases/download/2.0.0/BmpPwdDemo.zip)

### Install
   + NuGet
      * [BmpPwd is also available on NuGet!](https://www.nuget.org/packages/BmpPwd)   Install via NuGet Package Manager Console:
      ```nuget
      PM> Install-Package BmpPwd
      ```
      or by browsing NuGet Marketplace for `BmpPwd`

   + Manually
      1. [Download the latest Library (.dll)](https://github.com/mrousavy/BmpPwd/releases/download/1.0.0.5/BmpPwd.dll)
      2. Add the .dll to your Project   (Right click `References` in the Project Tree View, click `Add References` and `Browse` to the `.dll` File)

# How to use

## Using the default AES Cipher (Rijndael)

### Encrypt a string
   * C#:
   ```cs
   using System.Drawing.Common;
   using BmpPwd;
   // ...
   var encryptedImage = BmpPwd.BmpPwd.Encrypt("MyPassword", "The string to be encrypted");
   ```

   * VB:
   ```vb
   Imports System.Drawing.Common
   Imports BmpPwd
   ' ...
   Dim encryptedImage = BmpPwd.BmpPwd.Encrypt("MyPassword", "The string to be encrypted")
   ```

**⚠️: Be careful when saving the Image to not use lossy image compression, like JPEG. Lossy-compressing the Image will result in losing details and results in an un-decryptable image. Save as PNG or BMP to maintain pixel quality.**

### Decrypt an Image
   * C#:
   ```cs
   using System.Drawing.Common;
   using BmpPwd;
   // ...
   string decryptedText = BmpPwd.Decrypt("MyPassword", encryptedImage);
   ```

   * VB:
   ```vb
   Imports System.Drawing.Common
   Imports BmpPwd
   ' ...
   Dim decryptedText = BmpPwd.Decrypt("MyPassword", encryptedImage)
   ```

## Custom Encryption Implementation

### Implement your custom Encryption class
   * C#:
   ```cs
   public class MyCryptoClass : ICrypto {
       // Implement Encrypt(..) and Decrypt(..) here
   }
   ```

   * VB:
   ```vb
   Public Class MyCryptoClass
       Implements ICrypto
           ' Implement Encrypt(..) and Decrypt(..) here
   End Class
   ```

### Use
   * C#:
   ```cs
   using System.Drawing.Common;
   using BmpPwd;
   // ...
   var encryptedImage = BmpPwd.BmpPwd.Encrypt("MyPassword",
                                        "The string to be encrypted",
                                        new MyCryptoClass(),
                                        BmpPwd.DrawingScheme.Square,
                                        BmpPwd.ColorScheme.BlueMixed);
   ```

   * VB:
   ```vb
   Imports System.Drawing.Common
   Imports BmpPwd
   ' ...
   Dim encryptedImage = BmpPwd.BmpPwd.Encrypt(
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


# Thanks for using BmpPwd!
> License: [MIT](https://github.com/mrousavy/BmpPwd/blob/master/LICENSE) | mrousavy 2020
