using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CuaPackage
{
    internal class CuaHelpers
    {
        public static string RealTime()
        {
            double currentTime = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            return Math.Floor(currentTime).ToString();
        }

        public static string GetDeviceUuid()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }

        public static string getNid()
        {
            byte[] randomBytes = GenerateRandomBytes(9);
            string base64Encoded = Convert.ToBase64String(randomBytes);
            return base64Encoded;
        }

        public static string GetSessionId(string nid, string _cid)
        {
            return $"nid={nid};pid=Main;tid=200;nc=0;fc=1;bc=0,cid={GetCid(_cid)}";
        }

        public static string GetCid(string _cid, int _cidTs = 0, bool freezeCid = false)
        {
            int newTs = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds / 60;

            if (string.IsNullOrEmpty(_cid) || (_cidTs != newTs && !freezeCid))
            {
                _cidTs = newTs;
                byte[] randomBytes = new byte[16];
                new Random().NextBytes(randomBytes);
                _cid = BitConverter.ToString(randomBytes).Replace("-", "").ToLower();
            }

            return _cid;
        }

        public static string GetDeviceAdId()
        {
            const string hexDigits = "0123456789abcdef";
            Random random = new Random();

            string deviceAdId = new string(Enumerable.Repeat(hexDigits, 16)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return deviceAdId;
        }
        public static string GetJazoest()
        {
            string deviceUuid = GetDeviceUuid();
            int sumOfAscii =  deviceUuid.Sum(c => (int)c);
            return "2" + (sumOfAscii.ToString());
        }
        
        public static string EncodePassword(string password)
        {
            return "#PWD_BROWSER:0:" + RealTime() + ":" + password;
        }

        public static byte[] GenerateRandomBytes(int length)
        {
            Random random = new Random();
            byte[] randomBytes = new byte[length];
            random.NextBytes(randomBytes);
            return randomBytes;
        }

    }
}
