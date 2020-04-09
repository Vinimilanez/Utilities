using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Utilities.Log;

namespace Utilities.Cryptography
{
    public class Cryptography
    {
        public byte[] Key { get; private set; }
        public SymmetricAlgorithm Algorithm { get; private set; }

        public CryptProvider Provider { get; private set; }
        public Cryptography(CryptProvider provider, String key)
        {
            this.Provider = provider;

            switch (provider)
            {
                case CryptProvider.DES:
                    break;
                case CryptProvider.RC2:
                    break;
                case CryptProvider.TripleDES:
                    break;
                case CryptProvider.MD5:
                    break;
                case CryptProvider.Rijndael:
                    this.Algorithm = new RijndaelManaged();
                    this.Algorithm.Mode = CipherMode.CBC;
                    this.Algorithm.IV = new byte[] { 0xf, 0x6f, 0x13, 0x2e, 0x35, 0xc2, 0xcd, 0xf9, 0x5, 0x46, 0x9c, 0xea, 0xa8, 0x4b, 0x73, 0xcc };
                    break;
            }

            this.Key = GetKey(key);
            this.Algorithm.Key = this.Key;
            
        }

        private byte[] GetKey(String key)
        {
            String localKey = key;
            String salt = String.Empty;
            if (Algorithm.LegalKeySizes.Length > 0)
            {
                int keySize = localKey.Length * 8;
                
                if(keySize < Algorithm.LegalKeySizes[0].MinSize)
                {
                    localKey = localKey.PadRight(Algorithm.LegalKeySizes[0].MinSize / 8, '*');
                }
                else if(keySize > Algorithm.LegalKeySizes[0].MaxSize)
                {
                    localKey = localKey.Substring(0, Algorithm.LegalKeySizes[0].MinSize / 8);
                }
            }

            PasswordDeriveBytes passWord = new PasswordDeriveBytes(localKey, ASCIIEncoding.ASCII.GetBytes(salt));

            return passWord.GetBytes(localKey.Length);
        }

        public virtual String Encrypt(String text)
        {
            switch (this.Provider)
            {
                case CryptProvider.Rijndael:
                    return EncryptRijndael(text);
                default:
                    return null;
            }
        }

        public virtual String Decrypt(String text)
        {
            switch (this.Provider)
            {
                case CryptProvider.Rijndael:
                    return DecryptRijndael(text);
                default: 
                    return null;
            }
        }

        private String EncryptRijndael(String text)
        {
            byte[] plain = Encoding.UTF8.GetBytes(text);
            ICryptoTransform cryptoTransform = Algorithm.CreateEncryptor();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);
            cryptoStream.Write(plain, 0, plain.Length);
            cryptoStream.FlushFinalBlock();

            return Convert.ToBase64String(memoryStream.ToArray(), 0, memoryStream.ToArray().Length);
        }

        private String DecryptRijndael(String text)
        {
            byte[] crypto = Convert.FromBase64String(text);

            ICryptoTransform cryptoTransform = Algorithm.CreateDecryptor();

            try
            {
                MemoryStream memoryStream = new MemoryStream(crypto, 0, crypto.Length);
                CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read);
                StreamReader streamReader = new StreamReader(cryptoStream);

                return streamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                SystemLog.WriteLog(String.Format("{0} {1} >> {2} {0}", Environment.NewLine, ex.Message, ex.StackTrace));
                return null;
            }
        }

    }
}
