using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Proxy;
using MailKit.Search;
using Microsoft.CSharp.RuntimeBinder;
using MimeKit;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

public static class CuaGetMail
{
    public static string getOtpMail(string email, string password, string string_0, int int_0 = 60, int int_1 = 8, string proxy = "")
    {
        string text = "";
        try
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return "Lỗi : Chưa đầy đủ thông tin đăng nhập email";
            }
            email = email.Replace(" ", "").Replace("\r", "").Replace("\n", "");
            password = password.Replace(" ", "").Replace("\r", "").Replace("\n", "");
            int num = 993;
            string text2;
            if (!email.Contains("gmail"))
            {
                if (!email.Contains("hotmail") && !email.Contains("outlook"))
                {
                    if (!Regex.IsMatch(email, "@([^.]+)[.]cz"))
                    {
                        return "Lỗi : Kiểu email chưa hỗ trợ";
                    }
                    text2 = "imap.seznam.cz";
                }
                else
                {
                    text2 = "outlook.office365.com";
                }
            }
            else
            {
                text2 = "imap.gmail.com";
            }
            using (ImapClient imapClient = new ImapClient())
            {
                imapClient.ServerCertificateValidationCallback = MySslCertificateValidationCallback;
                if (!string.IsNullOrEmpty(proxy))
                {
                    proxy = CuaGetMail.smethod_34(proxy);
                    string[] array = proxy.Replace(" ", "").Replace("|", ":").Split(new char[]
                    {
                        ':'
                    });
                    string text3 = array[0];
                    if (!string.IsNullOrEmpty(text3) && array.Length >= 2)
                    {
                        string s = array[1];
                        if (array.Length > 2)
                        {
                            string u = array[2];
                            string p = array[3];
                            NetworkCredential networkCredential = new NetworkCredential(u, p);
                            HttpProxyClient proxyClient = new HttpProxyClient(text3, int.Parse(s), networkCredential);
                            imapClient.ProxyClient = proxyClient;
                        }
                        else
                        {
                            HttpProxyClient proxyClient2 = new HttpProxyClient(text3, int.Parse(s));
                            imapClient.ProxyClient = proxyClient2;
                        }
                    }
                }
                imapClient.SslProtocols = (SslProtocols.Ssl3 | SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12);
                imapClient.Connect(text2, num, (MailKit.Security.SecureSocketOptions)2, default(CancellationToken));
                imapClient.Authenticate(email, password, default(CancellationToken));
                try
                {
                    IMailFolder mailFolder = imapClient.GetFolder(imapClient.PersonalNamespaces[0]).getImalFolder("junk");
                    if (mailFolder != null)
                    {
                        mailFolder.Open((FolderAccess)2, default(CancellationToken));
                        mailFolder.MoveTo(UniqueIdRange.All, imapClient.Inbox, default(CancellationToken));
                    }
                }
                catch
                {
                }
                try
                {
                    IMailFolder mailFolder2 = imapClient.GetFolder(imapClient.PersonalNamespaces[0]).getImalFolder("spam");
                    if (mailFolder2 != null)
                    {
                        mailFolder2.Open((FolderAccess)2, default(CancellationToken));
                        mailFolder2.MoveTo(UniqueIdRange.All, imapClient.Inbox, default(CancellationToken));
                    }
                }
                catch
                {
                }
                imapClient.Inbox.Open((FolderAccess)1, default(CancellationToken));
                int num2 = 0;
                new MimeMessage().Date = DateTime.Now;
                do
                {
                    Task.Delay(3000).Wait();
                    imapClient.Inbox.Open((FolderAccess)1, default(CancellationToken));
                    List<UniqueId> list = imapClient.Inbox.Search(SearchQuery.FromContains(string_0), default(CancellationToken)).ToList<UniqueId>();
                    int num3 = list.Count - 1;
                    while (list.Count > 0 && num3 >= 0)
                    {
                        MimeMessage message = imapClient.Inbox.GetMessage(list[num3], default(CancellationToken), null);
                        bool sss = !(DateTime.UtcNow - message.Date.UtcDateTime > TimeSpan.FromSeconds((double)int_0));
                        string tess = message.TextBody;
                        if (!(DateTime.UtcNow - message.Date.UtcDateTime > TimeSpan.FromSeconds((double)int_0)))
                        {
                            string text4 = message.TextBody;
                            if (string.IsNullOrEmpty(text4))
                            {
                                text4 = message.HtmlBody;
                            }
                            text4 = text4.Replace("\r", "").Replace("\n", "").Replace(email, "");
                            if (string.IsNullOrEmpty(text))
                            {
                                text = Regex.Match(text4, "(\\d{6,8})").Groups[1].Value;
                            }
                            if (string.IsNullOrEmpty(text))
                            {
                                text = Regex.Match(text4, ">(\\d{6,8})<").Groups[1].Value;
                            }
                            if (string.IsNullOrEmpty(text))
                            {
                                text = Regex.Match(text4, "confirmcontact[.]php[?]c=(\\d+)").Groups[1].Value;
                            }
                            if (!string.IsNullOrEmpty(text))
                            {
                                if (!string.IsNullOrEmpty(text))
                                {
                                    break;
                                }
                            }
                            else
                            {
                                num3--;
                            }
                        }
                        else
                        {
                            num3--;
                        }
                    }
                    num2++;
                }
                while (string.IsNullOrEmpty(text) && num2 < 10);
                imapClient.Disconnect(true, default(CancellationToken));
            }
        }
        catch (Exception ex)
        {
            return "";
        }
        string result;
        if (string.IsNullOrEmpty(text))
        {
            result = "";
        }
        else
        {
            result = text;
        }
        return result;
    }

    public static IMailFolder getImalFolder(this IMailFolder imailFolder, string name)
    {
        IMailFolder result;
        if (imailFolder != null)
        {
            List<IMailFolder> list = imailFolder.GetSubfolders(false, default(CancellationToken)).ToList<IMailFolder>();
            foreach (IMailFolder mailFolder in list)
            {
                if (!string.IsNullOrEmpty(mailFolder.Name) && mailFolder.Name.ToLower().smethod_82(name))
                {
                    return mailFolder;
                }
            }
            foreach (IMailFolder imailFolder_ in list)
            {
                IMailFolder mailFolder2 = imailFolder_.getImalFolder(name);
                if (mailFolder2 != null)
                {
                    return mailFolder2;
                }
            }
            result = null;
        }
        else
        {
            result = null;
        }
        return result;
    }
    public static bool smethod_82(this string string_0, string string_1)
    {
        bool result;
        try
        {
            if (!string.IsNullOrEmpty(string_0) && !string.IsNullOrEmpty(string_1))
            {
                result = string_0.Contains(string_1);
            }
            else
            {
                result = false;
            }
        }
        catch
        {
            result = false;
        }
        return result;
    }
    public static string smethod_34(string string_0)
    {
        if (!string.IsNullOrEmpty(string_0))
        {
            if (Regex.IsMatch(string_0, "(.*?):(.*?)@(\\d{1,3})[.](\\d{1,3})[.](\\d{1,3})[.](\\d{1,3}):(\\d+)"))
            {
                string value = Regex.Match(string_0, "(.*?):(.*?)@(\\d{1,3})[.](\\d{1,3})[.](\\d{1,3})[.](\\d{1,3}):(\\d+)").Groups[1].Value;
                string value2 = Regex.Match(string_0, "(.*?):(.*?)@(\\d{1,3})[.](\\d{1,3})[.](\\d{1,3})[.](\\d{1,3}):(\\d+)").Groups[2].Value;
                string value3 = Regex.Match(string_0, "(.*?):(.*?)@(.*?):(\\d+)").Groups[3].Value;
                string value4 = Regex.Match(string_0, "(.*?):(.*?)@(.*?):(\\d+)").Groups[4].Value;
                string_0 = string.Concat(new string[]
                {
                    value3,
                    ":",
                    value4,
                    "|",
                    value,
                    "|",
                    value2
                });
            }
            else if (Regex.IsMatch(string_0, "(\\d{1,3})[.](\\d{1,3})[.](\\d{1,3})[.](\\d{1,3}):(\\d+):(.*?):(.*?)"))
            {
                string value5 = Regex.Match(string_0, "(\\d{1,3})[.](\\d{1,3})[.](\\d{1,3})[.](\\d{1,3}):(\\d+):(.*?):(.*?)$").Groups[6].Value;
                string value6 = Regex.Match(string_0, "(\\d{1,3})[.](\\d{1,3})[.](\\d{1,3})[.](\\d{1,3}):(\\d+):(.*?):(.*?)$").Groups[7].Value;
                string value7 = Regex.Match(string_0, "(.*?):(.*?):(.*?):(.+)").Groups[1].Value;
                string value8 = Regex.Match(string_0, "(.*?):(.*?):(.*?):(.+)").Groups[2].Value;
                string_0 = string.Concat(new string[]
                {
                    value7,
                    ":",
                    value8,
                    "|",
                    value5,
                    "|",
                    value6
                });
            }
        }
        return string_0;
    }
    static bool MySslCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        // If there are no errors, then everything went smoothly.
        if (sslPolicyErrors == SslPolicyErrors.None)
            return true;

        // Note: MailKit will always pass the host name string as the `sender` argument.
        var host = (string)sender;

        if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateNotAvailable) != 0)
        {
            // This means that the remote certificate is unavailable. Notify the user and return false.
            Console.WriteLine("The SSL certificate was not available for {0}", host);
            return false;
        }

        if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateNameMismatch) != 0)
        {
            // This means that the server's SSL certificate did not match the host name that we are trying to connect to.
            var certificate2 = certificate as X509Certificate2;
            var cn = certificate2 != null ? certificate2.GetNameInfo(X509NameType.SimpleName, false) : certificate.Subject;

            Console.WriteLine("The Common Name for the SSL certificate did not match {0}. Instead, it was {1}.", host, cn);
            return false;
        }

        // The only other errors left are chain errors.
        Console.WriteLine("The SSL certificate for the server could not be validated for the following reasons:");

        // The first element's certificate will be the server's SSL certificate (and will match the `certificate` argument)
        // while the last element in the chain will typically either be the Root Certificate Authority's certificate -or- it
        // will be a non-authoritative self-signed certificate that the server admin created. 
        foreach (var element in chain.ChainElements)
        {
            // Each element in the chain will have its own status list. If the status list is empty, it means that the
            // certificate itself did not contain any errors.
            if (element.ChainElementStatus.Length == 0)
                continue;

            Console.WriteLine("\u2022 {0}", element.Certificate.Subject);
            foreach (var error in element.ChainElementStatus)
            {
                // `error.StatusInformation` contains a human-readable error string while `error.Status` is the corresponding enum value.
                Console.WriteLine("\t\u2022 {0}", error.StatusInformation);
            }
        }

        return false;
    }
}