using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facebook_Tool_Request.Helpers
{
    public class FileHelper
    {
        public static string GetPathToCurrentFolder()
        {
            return Path.GetDirectoryName(Application.ExecutablePath);
        }

        public static string SelectFolder()
        {
            string result = "";
            try
            {
                using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
                {
                    DialogResult dialogResult = folderBrowserDialog.ShowDialog();
                    bool flag = dialogResult == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath);
                    if (flag)
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

        public static string SelectFile(string title = "Chọn File txt", string defaultFolder = "C:\\", string filter = "txt Files (*.txt)|*.txt|All files (*.*)|*.*")
        {
            string result = "";
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = filter;
                    openFileDialog.InitialDirectory = defaultFolder;
                    openFileDialog.Title = title;
                    bool flag = openFileDialog.ShowDialog() == DialogResult.OK;
                    if (flag)
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

        public static bool DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirName);
                bool flag = !directoryInfo.Exists;
                if (flag)
                {
                    return false;
                }
                DirectoryInfo[] directories = directoryInfo.GetDirectories();
                Directory.CreateDirectory(destDirName);
                FileInfo[] files = directoryInfo.GetFiles();
                foreach (FileInfo fileInfo in files)
                {
                    string destFileName = Path.Combine(destDirName, fileInfo.Name);
                    fileInfo.CopyTo(destFileName, true);
                }
                if (copySubDirs)
                {
                    foreach (DirectoryInfo directoryInfo2 in directories)
                    {
                        string destDirName2 = Path.Combine(destDirName, directoryInfo2.Name);
                        FileHelper.DirectoryCopy(directoryInfo2.FullName, destDirName2, copySubDirs);
                    }
                }
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        public static void CreateFile(string pathFile)
        {
            try
            {
                File.AppendAllText(pathFile, "");
            }
            catch
            {
            }
        }

        public static void CreateFolderIfNotExist(string pathFolder)
        {
            try
            {
                Directory.CreateDirectory(pathFolder);
            }
            catch
            {
            }
        }

        public static string CoppyFile(string pathOld, string pathNew)
        {
            try
            {
                File.Copy(pathOld, pathNew, true);
                return Path.GetFullPath(pathNew).ToString();
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static string renameFile(string filePath, string newName)
        {
            string newFileName = newName + Path.GetExtension(filePath);
            string directory = Path.GetDirectoryName(filePath);
            string newPath = Path.Combine(directory, newFileName);

            if (File.Exists(filePath))
            {
                File.Move(filePath, newPath);
                return newPath;
            }
            else
            {
                return null;
            }
        }
    }
}
