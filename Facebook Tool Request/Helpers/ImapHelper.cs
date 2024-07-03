using AE.Net.Mail;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Facebook_Tool_Request.Helpers
{
    internal class ImapHelper
    {
        public static string validMail(string email)
        {
            string text = "";
            bool flag = email.Contains("@hotmail.") || email.Contains("@live.") || email.Contains("@rickystar.") || email.Contains("@outlook.");
            if (flag)
            {
                text = "outlook.office365.com|imap-mail.outlook.com";
            }
            else
            {
                bool flag2 = email.Contains("@yandex.");
                if (flag2)
                {
                    text = "imap.yandex.com";
                }
                else
                {
                    bool flag3 = email.Contains("@gmail.");
                    if (flag3)
                    {
                        text = "imap.gmail.com";
                    }
                }
            }
            return text;
        }

        public static ImapClient imapCall(string email, string passmail)
        {
            ImapClient imapClient = null;
            string text = ImapHelper.validMail(email);
            bool flag2 = !(text == "");
            if (flag2)
            {
                List<string> list = text.Split(new char[]
                {
                    '|'
                }).ToList<string>();
                for (int i = 0; i < 10; i++)
                {
                    bool flag = false;
                    for (int j = 0; j < list.Count; j++)
                    {
                        text = list[j];
                        try
                        {
                            ServicePointManager.SecurityProtocol = (SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12);
                            return new ImapClient(text, email, passmail, AuthMethods.Login, 993, true, false);
                        }
                        catch (Exception ex)
                        {
                            flag = (ex.ToString().Contains("The remote certificate is invalid according to the validation procedure") || ex.ToString().Contains("An established connection was aborted by the software in your host machine"));
                        }
                    }
                    bool flag3 = !flag;
                    if (flag3)
                    {
                        break;
                    }
                }
            }
            return imapClient;
        }
        public static void imapClose(ImapClient imapClient)
        {
            try
            {
                bool flag = !imapClient.IsDisposed;
                if (flag)
                {
                    imapClient.Dispose();
                }
                bool isConnected = imapClient.IsConnected;
                if (isConnected)
                {
                    imapClient.Disconnect();
                }
            }
            catch
            {
            }
        }
        public static bool CheckConnectImap(string email, string passmail)
        {
            ImapClient imapClient = ImapHelper.imapCall(email, passmail);
            bool flag2 = imapClient != null;
            bool flag;
            if (flag2)
            {
                ImapHelper.imapClose(imapClient);
                flag = true;
            }
            else
            {
                flag = false;
            }
            return flag;
        }
    }
}
