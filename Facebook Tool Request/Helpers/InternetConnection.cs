using System;
using System.Runtime.InteropServices;

namespace Facebook_Tool_Request.Helpers
{
    internal class InternetConnection
    {
        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(out int description, int reservedValuine);

        public static bool IsConnectedToInternet()
        {
            int description;
            return InternetConnection.InternetGetConnectedState(out description, 0);
        }
    }
}
