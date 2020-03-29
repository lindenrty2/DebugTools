using System.Windows.Shapes;
using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using DebugTools.Secret;

namespace DebugTools.DBO
{
    public class RSASecretService : ISecretService
    {
        private RSACryptoServiceProvider _provider;

        private string _keyPath;
        public string KeyName
        {
            get
            {
                return System.IO.Path.GetFileNameWithoutExtension(_keyPath);
            }
        }

        public SecretMode SecretMode
        {
            get
            {
                return SecretMode.RSA;
            }
        }

        public RSASecretService(int KeySize)
        {
            _provider = new RSACryptoServiceProvider(KeySize);
        }

        public RSASecretService(string keyPath)
        {
            _provider = new RSACryptoServiceProvider();
            _keyPath = keyPath;
            LoadKey(keyPath);
        }

        public void SaveKey(string keyPath, bool isPrivateKey)
        {
            FileStream fs = new FileStream(keyPath, FileMode.Create, FileAccess.Write);
            string key = _provider.ToXmlString(isPrivateKey);
            fs.Write(Encoding.UTF8.GetBytes(key), 0, key.Length);
            fs.Close();
            fs.Dispose();
        }

        public void LoadKey(string keyPath)
        {
            FileStream fs = new FileStream(keyPath, FileMode.Open, FileAccess.Read);
            byte[] data = new byte[System.Convert.ToInt32(fs.Length) + 1];
            fs.Read(data, 0, System.Convert.ToInt32(fs.Length));
            fs.Close();
            fs.Dispose();
            _provider.FromXmlString(Encoding.UTF8.GetString(data));
        }

        public byte[] Dencrypt(byte[] bytes)
        {
            int keySize = System.Convert.ToInt32(((double)_provider.KeySize / (double)8));
            byte[] buffer = new byte[keySize - 1 + 1];
            MemoryStream msInput = new MemoryStream(bytes);
            MemoryStream msOutput = new MemoryStream();
            int readLen = msInput.Read(buffer, 0, keySize);

            while ((readLen > 0))
            {
                byte[] dataToDec = new byte[readLen - 1 + 1];
                Array.Copy(buffer, 0, dataToDec, 0, readLen);
                byte[] decData = _provider.Decrypt(dataToDec, false);
                msOutput.Write(decData, 0, decData.Length);
                readLen = msInput.Read(buffer, 0, keySize);
            }

            msInput.Close();
            var result = msOutput.ToArray();
            msOutput.Close();
            return result;
        }

        public byte[] Encrypt(byte[] bytes)
        {
            int keySize = System.Convert.ToInt32(((double)_provider.KeySize / (double)8));
            int bufferSize = keySize - 12;
            byte[] buffer = new byte[bufferSize - 1 + 1];

            MemoryStream msInput = new MemoryStream(bytes);
            MemoryStream msOutput = new MemoryStream();
            int readLen = msInput.Read(buffer, 0, bufferSize);

            while ((readLen > 0))
            {
                byte[] dataToEnc = new byte[readLen - 1 + 1];
                Array.Copy(buffer, 0, dataToEnc, 0, readLen);
                byte[] encData = _provider.Encrypt(dataToEnc, false);
                msOutput.Write(encData, 0, encData.Length);
                readLen = msInput.Read(buffer, 0, bufferSize);
            }

            msInput.Close();
            var result = msOutput.ToArray();
            msOutput.Close();
            return result;
        }
    }
}
