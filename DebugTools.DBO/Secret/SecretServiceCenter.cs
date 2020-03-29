using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Linq;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Media;
using System.Windows.Controls;
using System;
using System.Xml.Linq;
using System.Windows.Navigation;
using System.IO;
using DebugTools.Secret;

namespace DebugTools.DBO
{
    public class SecretServiceCenter
    {
        private static SecretServiceCenter _instance;
        public static SecretServiceCenter GetInstance()
        {
            if (_instance == null)
                _instance = new SecretServiceCenter();
            return _instance;
        }

        public ISecretService GetSecretService(SecretMode mode, int keySize)
        {
            return new RSASecretService(keySize);
        }

        public ISecretService GetSecretService(SecretMode mode, string keyName)
        {
            if (string.IsNullOrWhiteSpace(keyName))
                return null;
            string keyPath = PathHelper.GetPublicKeyPath(keyName);
            if (!File.Exists(keyPath))
                keyPath = PathHelper.GetPrivateKeyPath(keyName);
            if (!File.Exists(keyPath))
                throw new FileNotFoundException(string.Format("キー【{0}】は見つけない", keyName));
            return new RSASecretService(keyPath);
        }
    }
}
