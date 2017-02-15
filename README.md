# <img src="https://github.com/mrousavy/BmpPwd/blob/master/Images/Logo.png?raw=true" width="42"> BmpPwd
**BmpPwd** is a _Class Library_ for **en/decrypting** Text and visualizing it with a **System.Drawing.Bitmap**

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
   System.Drawing.Bitmap result = BmpPwd.Encrypt("MyPassword", "The string to be encrypted");
   ```
   
   * VB:
   ```VB
   //Needs Reference to System.Drawing dll
   Dim result As System.Drawing.Bitmap = BmpPwd.Encrypt("MyPassword", "The string to be encrypted")
   ```
   
### 4. Import your own Encryption
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
   End MyCryptoClass
   ```

### 5. Use custom Parameters:
   * C#:
   ```C#
   System.Drawing.Bitmap result = BmpPwd.Encrypt("MyPassword", "The string to be encrypted", new MyCryptoClass(), BmpPwd.DrawingScheme.Square, BmpPwd.ColorScheme.BlueMixed);
   ```
   
   * VB:
   ```VB
   Dim result As System.Drawing.Bitmap = BmpPwd.Encrypt("MyPassword", "The string to be encrypted", new MyCryptoClass(), BmpPwd.DrawingScheme.Square, BmpPwd.ColorScheme.BlueMixed)
   ```

# Screenshots
<img src="https://github.com/mrousavy/BmpPwd/blob/master/Images/Screenshots.gif?raw=true" alt="Screenshots in a gif">


<img src="https://github.com/mrousavy/BmpPwd/blob/master/Images/password.png?raw=true" alt="The Password 'password' in Rainbow/Square">

# Thanks for using BmpPwd!
