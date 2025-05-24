using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace RsaSecretManagement
{
    public static class RsaSecretMgmnt
    {
        public static void GenetateRSAKeyPair(string certSubject, int validityInYears, int keySize = 4096)
        {
            if (GetCert(certSubject) == null)
            {
                using RSA rsa = RSA.Create(keySize);
                string subject = $"CN={certSubject}";
                var request = new CertificateRequest(
                    subject,
                    rsa,
                    HashAlgorithmName.SHA256,
                    RSASignaturePadding.Pkcs1);

                X509Certificate2 cert = request.CreateSelfSigned(
                    DateTimeOffset.Now,
                    DateTimeOffset.Now.AddYears(validityInYears));

                var certWithKey = new X509Certificate2(
                    cert.Export(X509ContentType.Pfx),
                    (string?)null,
                    X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.UserKeySet);

                using var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadWrite);
                store.Add(certWithKey);
                store.Close();
            }
        }

        public static X509Certificate2? GetCert(string certSubject)
        {
            using var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            var cert = store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, $"CN={certSubject}", false);
            if (cert.Count() != 0)
            {
                return cert[0];
            }
            else
            {
                return null;
            }

        }

        public static byte[]? Encrypt(string plainText, string certSubject)
        {
            X509Certificate2? cert = GetCert(certSubject);

            if (cert != null)
            {
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                using RSA? publicRsa = cert.GetRSAPublicKey();
                byte[] encrypted = publicRsa.Encrypt(plainBytes, RSAEncryptionPadding.Pkcs1);
                return encrypted;
            }
            else { return null; }
        }
        public static string Decrypt(byte[] encryptedData, string certSubject)
        {
            X509Certificate2? cert = GetCert(certSubject);
            if (cert != null)
            {
                using RSA? privateRsa = cert.GetRSAPrivateKey();
                byte[] decrypted = privateRsa.Decrypt(encryptedData, RSAEncryptionPadding.Pkcs1);
                string result = Encoding.UTF8.GetString(decrypted);
                return result;
            }
            else { return string.Empty; }
        }
    }
}
