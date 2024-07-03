using Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace Facebook_Tool_Request.License
{
    internal class Hardware
    {
        public static string maHoa(string sChuoi)
        {
            MD5 md = MD5.Create();
            byte[] array = md.ComputeHash(Encoding.UTF8.GetBytes(sChuoi));
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < array.Length - 6; i++)
            {
                stringBuilder.Append(string.Format("{0:x2}", array[i]));
            }

            return string.Join("-", SplitIntoGroups(stringBuilder.ToString(), 4));
        }

        public static string getHDD()
        {
            string result = "";
            ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_DiskDrive");
            using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = managementObjectSearcher.Get().GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    ManagementObject managementObject = (ManagementObject)enumerator.Current;
                    object obj = managementObject["SerialNumber"];
                    result = maHoa(Hardware.EncryptHDD(((obj != null) ? obj.ToString() : null) ?? "", true)).ToLower();
                }
            }
            return result;
        }

        public static IEnumerable<string> SplitIntoGroups(string input, int groupSize)
        {
            for (int i = 0; i < input.Length; i += groupSize)
            {
                yield return input.Substring(i, Math.Min(groupSize, input.Length - i));
            }
        }


        public static string EncryptHDD(string toEncrypt, bool useHashing)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(toEncrypt);
            AppSettingsReader appSettingsReader = new AppSettingsReader();
            string s = "#CuaVipProMax#";
            byte[] key;
            if (useHashing)
            {
                MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider();
                key = md5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(s));
                md5CryptoServiceProvider.Clear();
            }
            else
            {
                key = Encoding.UTF8.GetBytes(s);
            }
            TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
            tripleDESCryptoServiceProvider.Key = key;
            tripleDESCryptoServiceProvider.Mode = CipherMode.ECB;
            tripleDESCryptoServiceProvider.Padding = PaddingMode.PKCS7;
            ICryptoTransform cryptoTransform = tripleDESCryptoServiceProvider.CreateEncryptor();
            byte[] array = cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length);
            tripleDESCryptoServiceProvider.Clear();
            return Convert.ToBase64String(array, 0, array.Length);
        }
    }
}
