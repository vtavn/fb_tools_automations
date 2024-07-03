using Facebook_Tool_Request.core;
using Helpers;
using IWshRuntimeLibrary;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using OtpNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facebook_Tool_Request.Helpers
{
    public class Common
    {
        private static Random rd = new Random();

        private static int getWidthScreen = Screen.PrimaryScreen.WorkingArea.Width;

        private static int getHeightScreen = Screen.PrimaryScreen.WorkingArea.Height;

        public static void ExportError(Chrome chrome, Exception ex, string error = "")
        {
            try
            {
                if (!Directory.Exists("log"))
                {
                    Directory.CreateDirectory("log");
                }
                if (!Directory.Exists("log\\html"))
                {
                    Directory.CreateDirectory("log\\html");
                }
                if (!Directory.Exists("log\\images"))
                {
                    Directory.CreateDirectory("log\\images");
                }
                Random random = new Random();
                string text = DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + "_" + random.Next(1000, 9999);
                if (chrome != null)
                {
                    //error chorme
                }
                using (StreamWriter streamWriter = new StreamWriter("log\\log.txt", append: true))
                {
                    streamWriter.WriteLine("-----------------------------------------------------------------------------");
                    streamWriter.WriteLine("Date: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    streamWriter.WriteLine("File: " + text);
                    if (error != "")
                    {
                        streamWriter.WriteLine("Error: " + error);
                    }
                    streamWriter.WriteLine();
                    if (ex != null)
                    {
                        streamWriter.WriteLine("Type: " + ex.GetType().FullName);
                        streamWriter.WriteLine("Message: " + ex.Message);
                        streamWriter.WriteLine("StackTrace: " + ex.StackTrace);
                        ex = ex.InnerException;
                    }
                }
            }
            catch
            {
            }
        }

        public static void ExportError(Exception ex, string error = "")
        {
            try
            {
                using (StreamWriter streamWriter = new StreamWriter("log\\log.txt", append: true))
                {
                    streamWriter.WriteLine("-----------------------------------------------------------------------------");
                    streamWriter.WriteLine("Date: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    if (error != "")
                    {
                        streamWriter.WriteLine("Error: " + error);
                    }
                    streamWriter.WriteLine();
                    if (ex != null)
                    {
                        streamWriter.WriteLine("Type: " + ex.GetType().FullName);
                        streamWriter.WriteLine("Message: " + ex.Message);
                        streamWriter.WriteLine("StackTrace: " + ex.StackTrace);
                        ex = ex.InnerException;
                    }
                }
            }
            catch
            {
            }
        }
        public static void ShowForm(Form f)
        {
            try
            {
                f.ShowInTaskbar = false;
                f.StartPosition = FormStartPosition.CenterParent;
                f.Owner = (Form.ActiveForm != null) ? Form.ActiveForm : null;
                f.ShowDialog();
            }
            catch (Exception ex)
            {
                ExportError(null, ex, "Error showform");
            }
        }

        public static void ShowForm2(Form f)
        {
            try
            {
                Thread tt = new Thread(() => Application.Run(f));
                tt.Start();
            }
            catch (Exception ex)
            {
                ExportError(null, ex, "Error showform2");
            }
        }

        public static void DelayTime(double second)
        {
            Application.DoEvents();
            Thread.Sleep(Convert.ToInt32(second * 1000.0));
        }
        public static List<string> RemoveEmptyItems(List<string> lst)
        {
            List<string> list = new List<string>();
            string text = "";
            for (int i = 0; i < lst.Count; i++)
            {
                text = lst[i].Trim();
                if (text != "")
                {
                    list.Add(text);
                }
            }
            return list;
        }

        public static void CreateFolder(string pathFolder)
        {
            try
            {
                if (!Directory.Exists(pathFolder))
                {
                    Directory.CreateDirectory(pathFolder);
                }
            }
            catch
            {
            }
        }
        public static void CreateFile(string pathFile)
        {
            try
            {
                if (!File.Exists(pathFile))
                {
                    File.AppendAllText(pathFile, "");
                }
            }
            catch
            {
            }
        }
        public static string GetTotp(string input)
        {
            string text = GetTotpServer(input);
            if (text == "")
            {
                text = GetTotpClient(input);
            }
            return text;
        }

        public static string GetTotpClient(string input)
        {
            try
            {
                byte[] secretKey = Base32Encoding.ToBytes(input.Trim().Replace(" ", ""));
                Totp totp = new Totp(secretKey);
                return totp.ComputeTotp(DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                ExportError(ex, "GetTotp(" + input + ")");
            }
            return "";
        }

        public static string GetTotpServer(string input)
        {
            string text = "";
            try
            {
                string text2 = "";
                input = input.Replace(" ", "").Trim();
                string text3 = "http://app.minsoftware.vn/api/2fa1?secret=" + input;
                string text4 = "http://2fa.live/tok/" + input;
                for (int i = 0; i < 5; i++)
                {
                    text = "";
                    try
                    {
                        text2 = ReadHTMLCode(text4);
                        if (text2.Contains("token"))
                        {
                            JObject jObject = JObject.Parse(text2);
                            text = jObject["token"].ToString().Trim();
                        }
                    }
                    catch (Exception ex)
                    {
                        ExportError(ex, text4);
                    }
                    try
                    {
                        if (text.Trim() == "")
                        {
                            text = ReadHTMLCode(text3);
                        }
                    }
                    catch (Exception ex2)
                    {
                        ExportError(ex2, text3);
                    }
                    if (text != "" && IsNumber(text))
                    {
                        for (int j = text.Length; j < 6; j++)
                        {
                            text = "0" + text;
                        }
                        break;
                    }
                    DelayTime(1.0);
                }
            }
            catch
            {
            }
            return text;
        }
        public static string ReadHTMLCode(string Url)
        {
            try
            {
                return new RequestHttp().RequestGet(Url);
            }
            catch
            {
                return null;
            }
        }
        public static bool IsNumber(string pValue)
        {
            if (pValue == "")
            {
                return false;
            }
            foreach (char c in pValue)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool ChangeIP(int typeChangeIP, int typeDcom, string profileDcom, string urlHilink, int iTypeHotspot, string sLinkNord)
        {
            bool result = false;
            string text = "";
            try
            {
                switch (typeChangeIP)
                {
                    case 0:
                        return true;
                }
            }
            catch (Exception ex)
            {
                ExportError(null, ex, "Error ChangeIP");
            }
            return result;
        }
        public static void CreateShortcut(string shortcutName, string shortcutPath, string targetFileLocation, string arg, string WorkingDirectory = "C:\\Program Files (x86)\\Google\\Chrome\\Application", string icon = "")
        {
            string pathLink = Path.Combine(shortcutPath, shortcutName + ".lnk");
            WshShell wshShell = (WshShell)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")));
            IWshShortcut wshShortcut = (IWshShortcut)(dynamic)wshShell.CreateShortcut(pathLink);
            wshShortcut.Description = "Shortcut Chrome Cua ToolKit";
            wshShortcut.WorkingDirectory = WorkingDirectory;
            wshShortcut.IconLocation = icon;
            wshShortcut.TargetPath = targetFileLocation;
            wshShortcut.Arguments = arg;
            wshShortcut.Save();
        }
        public static string CreateRandomStringNumber(int lengText, Random rd = null)
        {
            string text = "";
            if (rd == null)
            {
                rd = new Random();
            }
            string text2 = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            for (int i = 0; i < lengText; i++)
            {
                text += text2[rd.Next(0, text2.Length)];
            }
            return text;
        }
        public static int GetIndexOfPossitionApp(ref List<int> lstPossition)
        {
            int result = 0;
            lock (lstPossition)
            {
                for (int i = 0; i < lstPossition.Count; i++)
                {
                    if (lstPossition[i] == 0)
                    {
                        result = i;
                        lstPossition[i] = 1;
                        break;
                    }
                }
            }
            return result;
        }
        public static string CheckProxy(string proxy, int typeProxy)
        {
            string text = "";
            try
            {
                RequestXNet requestXNet = new RequestXNet("", SetupFolder.GetUseragentIPhone(rd), proxy, typeProxy);
                return requestXNet.RequestGet("https://ipinfo.io/ip");
            }
            catch (Exception)
            {
                return CheckProxy2(proxy, typeProxy);
            }
        }
        public static string CheckProxy2(string proxy, int typeProxy)
        {
            string text = "";
            try
            {
                RequestXNet requestXNet = new RequestXNet("", SetupFolder.GetUseragentIPhone(rd), proxy, typeProxy);
                return requestXNet.RequestGet("https://api64.ipify.org/");
            }
            catch (Exception)
            {
                return CheckProxy3(proxy, typeProxy);
            }
        }
        public static string CheckProxy3(string proxy, int typeProxy)
        {
            string result = "";
            try
            {
                RequestXNet requestXNet = new RequestXNet("", SetupFolder.GetUseragentIPhone(rd), proxy, typeProxy);
                string input = requestXNet.RequestGet("https://showip.net/");
                result = Regex.Match(input, "value=\"(.*?)\"").Groups[1].Value;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, "Check Proxy2");
            }
            return result;
        }
        public static Point GetPointFromIndexPosition(int indexPos, int column, int row)
        {
            JSON_Settings jSON_Settings = new JSON_Settings("configChrome");
            if (jSON_Settings.GetValueInt("width") == 0)
            {
                jSON_Settings.Update("width", getWidthScreen);
                jSON_Settings.Update("heigh", getHeightScreen);
                jSON_Settings.Save();
            }
            getWidthScreen = jSON_Settings.GetValueInt("width");
            getHeightScreen = jSON_Settings.GetValueInt("heigh");
            Point result = default(Point);
            while (indexPos >= column * row)
            {
                indexPos -= column * row;
            }
            switch (row)
            {
                case 1:
                    result.Y = 0;
                    break;
                case 2:
                    if (indexPos < column)
                    {
                        result.Y = 0;
                    }
                    else if (indexPos < column * 2)
                    {
                        int num = indexPos / column;
                        result.Y = getHeightScreen / 2;
                        indexPos -= column;
                    }
                    break;
                case 3:
                    if (indexPos < column)
                    {
                        result.Y = 0;
                    }
                    else if (indexPos < column * 2)
                    {
                        result.Y = getHeightScreen / 3;
                        indexPos -= column;
                    }
                    else if (indexPos < column * 3)
                    {
                        result.Y = getHeightScreen / 3 * 2;
                        indexPos -= column * 2;
                    }
                    break;
                case 4:
                    if (indexPos < column)
                    {
                        result.Y = 0;
                    }
                    else if (indexPos < column * 2)
                    {
                        result.Y = getHeightScreen / 4;
                        indexPos -= column;
                    }
                    else if (indexPos < column * 3)
                    {
                        result.Y = getHeightScreen / 4 * 2;
                        indexPos -= column * 2;
                    }
                    else if (indexPos < column * 4)
                    {
                        result.Y = getHeightScreen / 4 * 3;
                        indexPos -= column * 3;
                    }
                    break;
                case 5:
                    if (indexPos < column)
                    {
                        result.Y = 0;
                    }
                    else if (indexPos < column * 2)
                    {
                        result.Y = getHeightScreen / 5;
                        indexPos -= column;
                    }
                    else if (indexPos < column * 3)
                    {
                        result.Y = getHeightScreen / 5 * 2;
                        indexPos -= column * 2;
                    }
                    else if (indexPos < column * 4)
                    {
                        result.Y = getHeightScreen / 5 * 3;
                        indexPos -= column * 3;
                    }
                    else
                    {
                        result.Y = getHeightScreen / 5 * 4;
                        indexPos -= column * 4;
                    }
                    break;
            }
            int num2 = getWidthScreen / column;
            result.X = indexPos * num2 - 10;
            return result;
        }
        public static Point GetSizeChrome(int column, int row)
        {
            JSON_Settings jSON_Settings = new JSON_Settings("configChrome");
            if (jSON_Settings.GetValueInt("width") == 0)
            {
                jSON_Settings.Update("width", getWidthScreen);
                jSON_Settings.Update("heigh", getHeightScreen);
                jSON_Settings.Save();
            }
            getWidthScreen = jSON_Settings.GetValueInt("width");
            getHeightScreen = jSON_Settings.GetValueInt("heigh");
            int x = getWidthScreen / column + 15;
            int y = getHeightScreen / row + 10;
            return new Point(x, y);
        }
        public static bool CopyFolder(string pathFrom, string pathTo)
        {
            try
            {
                CreateFolder(pathTo);
                string[] directories = Directory.GetDirectories(pathFrom, "*", SearchOption.AllDirectories);
                foreach (string text in directories)
                {
                    Directory.CreateDirectory(text.Replace(pathFrom, pathTo));
                }
                string[] files = Directory.GetFiles(pathFrom, "*.*", SearchOption.AllDirectories);
                foreach (string text2 in files)
                {
                    File.Copy(text2, text2.Replace(pathFrom, pathTo), overwrite: true);
                }
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }
        public static bool MoveFolder(string pathFrom, string pathTo)
        {
            try
            {
                Directory.Move(pathFrom, pathTo);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        public static bool DeleteFolder(string pathFolder)
        {
            try
            {
                Directory.Delete(pathFolder, recursive: true);
                return true;
            }
            catch
            {
            }
            return false;
        }
        public static Form GetFormByName(string name, string para)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            Type[] types = executingAssembly.GetTypes();
            foreach (Type type in types)
            {
                if (type.BaseType != null && type.BaseType.FullName == "System.Windows.Forms.Form" && type.FullName == name)
                {
                    return Activator.CreateInstance(Type.GetType(name), "", 1, para) as Form;
                }
            }
            return null;
        }
        public static void FillIndexPossition(ref List<int> lstPossition, int indexPos)
        {
            lock (lstPossition)
            {
                lstPossition[indexPos] = 0;
            }
        }

        public static DataTable ShuffleDataTable(DataTable dt)
        {
            DataTable result = new DataTable();
            try
            {
                result = (from DataRow r in dt.Rows
                          orderby Base.rd.Next()
                          select r).CopyToDataTable();
            }
            catch
            {
            }
            return result;
        }
        public static string ConvertSecondsToTime(int seconds)
        {
            try
            {
                TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
                if (seconds < 60)
                {
                    return TimeSpan.FromSeconds(seconds).ToString("ss");
                }
                if (seconds < 3600)
                {
                    return TimeSpan.FromSeconds(seconds).ToString("mm\\:ss");
                }
                return TimeSpan.FromSeconds(seconds).ToString("hh\\:mm\\:ss");
            }
            catch
            {
                return "";
            }
        }
        public static string SpinText(string text, Random rand)
        {
            int num = -1;
            char[] anyOf = new char[2] { '{', '}' };
            text += "~";
            do
            {
                int num2 = num;
                num = -1;
                while ((num2 = text.IndexOf('{', num2 + 1)) != -1)
                {
                    int num3 = num2;
                    while ((num3 = text.IndexOfAny(anyOf, num3 + 1)) != -1 && text[num3] != '}')
                    {
                        if (num == -1)
                        {
                            num = num2;
                        }
                        num2 = num3;
                    }
                    if (num3 != -1)
                    {
                        string[] array = text.Substring(num2 + 1, num3 - 1 - (num2 + 1 - 1)).Split('|');
                        text = text.Remove(num2, num3 - (num2 - 1)).Insert(num2, array[rand.Next(array.Length)]);
                    }
                }
            }
            while (num-- != -1);
            return text.Remove(text.Length - 1);
        }
        public static string CreateRandomNumber(int leng, Random rd = null)
        {
            string text = "";
            if (rd == null)
            {
                rd = new Random();
            }
            string text2 = "0123456789";
            for (int i = 0; i < leng; i++)
            {
                text += text2[rd.Next(0, text2.Length)];
            }
            return text;
        }
        public static string CreateRandomString(int lengText, Random rd = null)
        {
            string text = "";
            if (rd == null)
            {
                rd = new Random();
            }
            string text2 = "abcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < lengText; i++)
            {
                text += text2[rd.Next(0, text2.Length)];
            }
            return text;
        }
        public static bool CheckStringIsContainIcon(string content)
        {
            return content.Length != Regex.Replace(content, "\\p{Cs}", "").Length;
        }
        public static string ConvertToUnSign(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string input = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(input, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
        }
        public static bool CreateShortcutChrome(string shortcutName, string shortcutPath, string profilePath, string icon = "", string targetFileLocation = "\"C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe\"")
        {
            bool result = false;
            try
            {
                CreateShortcut(shortcutName, shortcutPath, targetFileLocation, "--user-data-dir=\"" + profilePath + "\"", targetFileLocation.Substring(0, targetFileLocation.LastIndexOf("\\")), icon);
                result = true;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, "chrome.CreateShortcut(" + shortcutName + "," + shortcutPath + "," + targetFileLocation + ")select");
            }
            return result;
        }
        public static string SelectFolder()
        {
            string result = "";
            try
            {
                using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
                {
                    DialogResult dialogResult = folderBrowserDialog.ShowDialog();
                    if (dialogResult == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                    {
                        result = folderBrowserDialog.SelectedPath;
                    }
                }
            }
            catch
            {
            }
            return result;
        }
        public static string SelectFile(string title = "Chọn File txt", string typeFile = "txt Files (*.txt)|*.txt|")
        {
            string result = "";
            try
            {
                using (System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog())
                {
                    openFileDialog.Filter = typeFile + "All files (*.*)|*.*";
                    openFileDialog.InitialDirectory = "C:\\";
                    openFileDialog.Title = title;
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        result = openFileDialog.FileName;
                    }
                }
            }
            catch
            {
            }
            return result;
        }
        public static string SelectFolderNew(string title = "Chọn Folder", string typeFile = "")
        {
            string result = "";
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.ValidateNames = false;
            openFileDialog.CheckFileExists = false;
            openFileDialog.CheckPathExists = true;
            openFileDialog.Title = title;
            openFileDialog.FileName = "Chọn Folder này.";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                result = Path.GetDirectoryName(openFileDialog.FileName);
            }

            //try
            //{
            //    using (OpenFileDialog openFileDialog = new OpenFileDialog())
            //    {
            //        openFileDialog.InitialDirectory = "C:\\";
            //        openFileDialog.Title = title;
            //        if (openFileDialog.ShowDialog() == DialogResult.OK)
            //        {
            //            string directoryPath = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
            //            result = directoryPath;
            //        }
            //    }
            //}
            //catch
            //{
            //}
            return result;
        }

        public static void SaveLog(string type, string msg = "", string log = "comment")
        {
            try
            {
                switch(log)
                {
                    case "comment":
                        if (type == "success")
                        {
                            using (StreamWriter streamWriter = new StreamWriter("log\\spam\\success.txt", append: true))
                            {
                                if (msg != "")
                                {
                                    streamWriter.WriteLine("Spam Thành Công: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - " + msg);
                                }
                            }
                        }
                        else
                        {
                            using (StreamWriter streamWriter = new StreamWriter("log\\spam\\error.txt", append: true))
                            {
                                if (msg != "")
                                {
                                    streamWriter.WriteLine("Spam Thất Bại: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - " + msg);
                                }

                            }
                        }
                        break;
                    case "created_page":
                        if (type == "success")
                        {
                            using (StreamWriter streamWriter = new StreamWriter("log\\create_pages\\success.txt", append: true))
                            {
                                if (msg != "")
                                {
                                    streamWriter.WriteLine("Tạo thành công: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - " + msg);

                                }
                            }
                        }
                        else
                        {
                            using (StreamWriter streamWriter = new StreamWriter("log\\create_pages\\error.txt", append: true))
                            {
                                if (msg != "")
                                {
                                    streamWriter.WriteLine("Tạo thất bại: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - " + msg);
                                }

                            }
                        }
                        break;
                    case "invitePage":
                        if (type == "success")
                        {
                            using (StreamWriter streamWriter = new StreamWriter("log\\invite_pages\\success.txt", append: true))
                            {
                                if (msg != "")
                                {
                                    streamWriter.WriteLine("Thành công nhận page: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - " + msg);

                                }
                            }
                        }
                        else
                        {
                            using (StreamWriter streamWriter = new StreamWriter("log\\invite_pages\\error.txt", append: true))
                            {
                                if (msg != "")
                                {
                                    streamWriter.WriteLine("Thất bại nhận page: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - " + msg);
                                }

                            }
                        }
                        break;
                    case "seeding_pages":
                        if (type == "success")
                        {
                            using (StreamWriter streamWriter = new StreamWriter("log\\seeding_pages\\success.txt", append: true))
                            {
                                if (msg != "")
                                {
                                    streamWriter.WriteLine("Cmt OK: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - " + msg);

                                }
                            }
                        }
                        else
                        {
                            using (StreamWriter streamWriter = new StreamWriter("log\\seeding_pages\\error.txt", append: true))
                            {
                                if (msg != "")
                                {
                                    streamWriter.WriteLine("Cmt Faild: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - " + msg);
                                }

                            }
                        }
                        break;
                }    
            }
            catch
            {
            }
        }
        public static string randomPageCategories()
        {
            List<string> idList = SetupFolder.getListIdCategoriesPage();
            List<string> randomIds = new List<string>();

            Random random = new Random();
            int count = random.Next(1, 4);

            for (int i = 0; i < count; i++)
            {
                int index = random.Next(idList.Count);
                randomIds.Add(idList[index]);
                idList.RemoveAt(index);
            }
            string[] result = randomIds.ToArray();
            return "[" + string.Join(",", result) + "]";
        }
        public static string ConvertTimeStampToDateTime(long timestamp)
        {
            DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
            return dateTime.ToString("dd-M-yyyy H:mm:ss");
        }

        public static string getNameVietNamRandom()
        {
            List<string> listHoVn = SetupFolder.GetListHoVN();
            List<string> listTenDem = SetupFolder.GetListTenDemVN();
            List<string> listTen = SetupFolder.GetListTenVN();

            string hoVn = "";
            string tenDem = "";
            string tenVn = "";
            hoVn = listHoVn[rd.Next(0, listHoVn.Count)];
            tenDem = listTenDem[rd.Next(0, listTenDem.Count)];
            tenVn = listTen[rd.Next(0, listTen.Count)];
            return (hoVn + " " + tenDem + " " + tenVn).Replace("  ", " ");
        }
        public static void KillProcess(string nameProcess)
        {
            try
            {
                Process[] processesByName = Process.GetProcessesByName(nameProcess);
                foreach (Process process in processesByName)
                {
                    process.Kill();
                }
            }
            catch
            {
            }
        }
        public static void OpenFileAndPressData(string linkPathFile, string title = "Nhập danh sách Uid cần clone", string status = "Danh sách Uid", string footer = "(Mỗi nội dung 1 dòng, spin nội dung {a|b|c})")
        {
            //Discarded unreachable code: IL_0051, IL_005e
            try
            {
                if (!File.Exists(linkPathFile))
                {
                    CreateFile(linkPathFile);
                }
                ShowForm(new fNhapDuLieu(linkPathFile, title, status, footer));
            }
            catch
            {
            }
        }
        public static string UrlEncode(string text)
        {
            return WebUtility.UrlEncode(text);
        }
        public static string UrlDecode(string text)
        {
            return WebUtility.UrlDecode(text);
        }
        public static bool IsVNName(string ten)
        {
            ten = ten.Trim();
            if (ten == "")
            {
                return false;
            }
            return ten != ConvertToUnSign(ten) && IsContainsVNChar(ten);
        }
        public static bool IsContainsVNChar(string text)
        {
            bool result = true;
            string text2 = " abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZaAeEoOuUiIdDyYáàạảãâấầậẩẫăắằặẳẵÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴéèẹẻẽêếềệểễÉÈẸẺẼÊẾỀỆỂỄóòọỏõôốồộổỗơớờợởỡÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠúùụủũưứừựửữÚÙỤỦŨƯỨỪỰỬỮíìịỉĩÍÌỊỈĨđĐýỳỵỷỹÝỲỴỶỸ";
            foreach (char value in text)
            {
                if (!text2.ToCharArray().Contains(value))
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
        public static bool IsContainNumber(string pValue)
        {
            foreach (char c in pValue)
            {
                if (char.IsDigit(c))
                {
                    return true;
                }
            }
            return false;
        }
        public static void ClearSelectedOnDatagridview(DataGridView dtgv)
        {
            for (int i = 0; i < dtgv.RowCount; i++)
            {
                dtgv.Rows[i].Selected = false;
            }
        }
        public static List<string> CloneList(List<string> lstFrom)
        {
            List<string> list = new List<string>();
            try
            {
                for (int i = 0; i < lstFrom.Count; i++)
                {
                    list.Add(lstFrom[i]);
                }
            }
            catch
            {
            }
            return list;
        }
        public static List<int> ShuffleList(List<int> lst)
        {
            int num = 0;
            int num2 = lst.Count;
            int num3 = 0;
            while (num2 != 0)
            {
                num3 = Base.rd.Next(0, lst.Count);
                num2--;
                num = lst[num2];
                lst[num2] = lst[num3];
                lst[num3] = num;
            }
            return lst;
        }
        public static List<string> ShuffleList(List<string> lst)
        {
            string text = "";
            int num = lst.Count;
            int num2 = 0;
            while (num != 0)
            {
                num2 = Base.rd.Next(0, lst.Count);
                num--;
                text = lst[num];
                lst[num] = lst[num2];
                lst[num2] = text;
            }
            return lst;
        }
        public static List<string> GetIMGfromFolder(string path)
        {
            List<string> list = new List<string>();
            foreach(string file in Directory.GetFiles(path))
            {
                if (file.Contains(".jpg") || file.Contains(".png") || file.Contains(".jpeg")) list.Add(file);
            }
            return list;
        }

        public static string GetRamdom(List<string> list)
        {
            if (list == null || list.Count == 0) return "";
            Random rd = new Random(Guid.NewGuid().GetHashCode());
            return list[rd.Next(list.Count)];
        }
        public static MatchCollection RegexMatches(string input, string pattern)
        {
            MatchCollection result;
            if (!string.IsNullOrEmpty(input))
            {
                result = Regex.Matches(input, pattern);
            }
            else
            {
                result = Regex.Matches("foo", "bar");
            }
            return result;
        }

        public static Match RegexMatch(string input, string pattern)
        {
            Match result;
            if (!string.IsNullOrEmpty(input))
            {
                result = Regex.Match(input, pattern);
            }
            else
            {
                result = Match.Empty;
            }
            return result;
        }
        public static int ToInt(string input, int i)
        {
            try
            {
                return int.Parse(input);
            }
            catch { return i; }
        }
        public static bool ToBool(string input, bool b)
        {
            try
            {
                return bool.Parse(input);
            }
            catch { return b; }
        }
        public static List<string> GetAllNodes(string input, string param)
        {
            return input.Split(new[] { param }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
        public static List<String> GetAllLine(string input)
        {
            return input.Split(new[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
        public static List<String> GetAllNote(string input, string param)
        {
            return input.Split(new[] { param }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
        public static string Base64Encode(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return "";
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            try
            {
                if (string.IsNullOrEmpty(base64EncodedData)) return "";
                var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
                return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }catch { return ""; }
        }
        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1,1,0,0,0,0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
        public static long DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (long)((TimeZoneInfo.ConvertTimeToUtc(dateTime)- new DateTime(1970,1,1,0,0,0,0, System.DateTimeKind.Utc)).TotalSeconds);
        }
        public static string RegexUnescape(string value)
        {
            string result;
            if (string.IsNullOrEmpty(value))
            {
                result = "";
            }
            else
            {
                try
                {
                    return Regex.Unescape(value);
                }
                catch
                {
                }
                result = value;
            }
            return result;
        }

        public static bool saveActivityLog(string cUid, string featured, string content)
        {
            bool result = false;
            try
            {
                string txtPathUid = Environment.CurrentDirectory + "\\log\\ActivityClone\\" + cUid + ".txt";
                if (!File.Exists(txtPathUid)) Helpers.Common.CreateFile(txtPathUid);
                using (StreamWriter streamWriter = new StreamWriter(txtPathUid, append: true))
                {
                    streamWriter.WriteLine(featured + ": " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - " + content);
                }
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public static bool saveLog(string cUid, string featured, string uid)
        {
            bool result = false;
            try
            {
                string txtPathUid = Environment.CurrentDirectory + "\\log\\logs\\" + featured + "\\" + cUid + ".txt";
                if (!File.Exists(txtPathUid)) Helpers.Common.CreateFile(txtPathUid);
                using (StreamWriter streamWriter = new StreamWriter(txtPathUid, append: true))
                {
                    streamWriter.WriteLine(uid);
                }
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }
        public static bool getLog(string cUid, string featured, string uid)
        {
            bool result = false;
            try
            {
                string filePath = Environment.CurrentDirectory + "\\log\\logs\\" + featured + "\\" + cUid + ".txt";
                string fileContent = File.ReadAllText(filePath);

                if(fileContent.Contains(uid))
                {
                    result = true;
                }
                else
                {
                    result= false;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public static bool checkContainsOrEmpty(string text1, string text2)
        {
            bool result;
            try
            {
                if (!string.IsNullOrEmpty(text1) && !string.IsNullOrEmpty(text2))
                {
                    result = text1.Contains(text2);
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

        public static string GetPathChromeEXE()
        {
            string result = "";
            string newValue = "chrome.exe";
            object value = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\<executableFileName>".Replace("<executableFileName>", newValue), "", null);
            bool flag = value == null;
            if (flag)
            {
                value = Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\App Paths\\<executableFileName>".Replace("<executableFileName>", newValue), "", null);
                bool flag2 = value != null;
                if (flag2)
                {
                    result = value.ToString();
                }
            }
            else
            {
                result = value.ToString();
            }
            return result;
        }
        public static string GetVersionChromeEXE()
        {
            return FileVersionInfo.GetVersionInfo(Common.GetPathChromeEXE().ToString()).FileVersion;
        }

        public static bool DeleteFile(string pathFile)
        {
            try
            {
                File.Delete(pathFile);
                return true;
            }
            catch
            {
            }
            return false;
        }
        public static void DownloadFile(string link)
        {
            new fDownloadFile(link).ShowDialog();
        }
        public static void Shutdown()
        {
            ManagementClass managementClass = new ManagementClass("Win32_OperatingSystem");
            managementClass.Get();
            managementClass.Scope.Options.EnablePrivileges = true;
            ManagementBaseObject methodParameters = managementClass.GetMethodParameters("Win32Shutdown");
            methodParameters["Flags"] = "1";
            methodParameters["Reserved"] = "0";
            foreach (ManagementBaseObject managementBaseObject in managementClass.GetInstances())
            {
                ManagementObject instance = (ManagementObject)managementBaseObject;
                instance.InvokeMethod("Win32Shutdown", methodParameters, null);
            }
        }
        internal static List<string> GetFiles(string folderPath)
        {
            bool flag = Directory.Exists(folderPath);
            List<string> result;
            if (flag)
            {
                result = Directory.GetFiles(folderPath).ToList<string>();
            }
            else
            {
                result = new List<string>();
            }
            return result;
        }
    }
}
