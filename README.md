<p align="center">
  <img src=https://devnullsec.hu/logo_small.svg height="64" width="64">
</p>

## RSA Secret Management
### C# helper class for handling RSA Encryption and Decription


- Usage
> The class allows you to use the RSA encryption algorithm securely. Public and private keys are created in the Windows Certificate store, so the keys are properly protected. Example: 

```csharp
using RsaSecretManagement;
namespace RsaDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //1. Generate RSA keypair. It will save your windows certificate store (certmgr)
            //The key pair generation should be run only once, not before each encryption/decryption.
            //If a key pair with the specified name already exists, it will not be regenerated.
            string myCertName = "MyFirstCert";
            int validity = 100;
            int keySize = 2048;
            string plainText = "HelloRSA";
            RsaSecretMgmnt.GenetateRSAKeyPair(myCertName, validity, keySize);
            Console.WriteLine("RSA key pair generated...");

            //2. Encrypt
            var encryptedData = RsaSecretMgmnt.Encrypt(plainText, myCertName);
            Console.WriteLine($"Encrypted : {encryptedData}");

            //3. Decrypt
            var decryptedData = RsaSecretMgmnt.Decrypt(encryptedData, myCertName);
            Console.WriteLine($"Decrypted : {decryptedData}");
        }
    }
}
```
<br>
<p align="center">
  <a href="https://www.nuget.org/packages/RsaSecretManagement/1.0.0" target="_blank" >
    <img src="./nuget_logo.png" alt="NuGet Page">
  </a>
</p>
