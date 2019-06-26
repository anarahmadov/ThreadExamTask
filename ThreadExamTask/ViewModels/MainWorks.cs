using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Security;
using System.Security.AccessControl;

namespace ThreadExamTask.ViewModels
{
    public class MainWorks
    {
        public List<string> RestrictedWords { get; set; }
        private DriveInfo[] drives = DriveInfo.GetDrives();

        string specialFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}" + @"\FindRestrictedApp";
        static string path = $@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}";

        public MainWorks()
        {
            Directory.CreateDirectory(specialFolder);
        }

        public void PutRestrictedWordsFrom(string filename)
        {
            if (filename != null)
            {
                RestrictedWords = new List<string>();

                var arr = File.ReadAllText(filename).Split(' ');

                for (int i = 0; i < arr.Count(); i++)
                {
                    RestrictedWords.Add(arr[i]);
                }
            }
        }

        public void ScanRecursively()
        {
            string allText = "";

            #region Real code

            //foreach (var drive in drives)
            //{
            //    foreach (var di in drive.RootDirectory.GetDirectories())
            //    {
            //        var dirpath = di.FullName;

            //        if (new DirectoryInfo(dirpath).Name != "FindRestrictedApp" &&
            //        IsAccessToFolder(dirpath))
            //        {
            //            path = Path.GetFullPath(di.FullName);

            //            foreach (string filePath in Directory.GetFiles(dirpath))
            //            {
            //                if (new FileInfo(filePath).Extension == ".txt")
            //                {
            //                    string filename = Path.GetFullPath(filePath);

            //                    // handle that thrown exception by files currently in use.
            //                    try
            //                    {
            //                        allText = File.ReadAllText(filename);
            //                    }
            //                    catch (Exception)
            //                    {
            //                        continue;
            //                    }

            //                    // find from list of RestrictedWords 
            //                    for (int i = 0; i < RestrictedWords.Count(); i++)
            //                    {
            //                        if (allText.Contains(RestrictedWords[i]))
            //                        {
            //                            CopyToSpecialFolder(filename);
            //                            break;
            //                        }
            //                    }
            //                }
            //            }
            //            ScanRecursively();
            //        }
            //    }
            //}

            #endregion

            #region test

            foreach (string dirPath in Directory.GetDirectories(path))
            {
                if (new DirectoryInfo(dirPath).Name != "FindRestrictedApp" &&
                    IsAccessToFolder(dirPath))
                {
                    path = Path.GetFullPath(dirPath);

                    foreach (string filePath in Directory.GetFiles(dirPath))
                    {
                        if (new FileInfo(filePath).Extension == ".txt")
                        {
                            string filename = Path.GetFullPath(filePath);

                            // handle that thrown exception by files currently in use.
                            try
                            {
                                allText = File.ReadAllText(filename);
                            }
                            catch (Exception)
                            {
                                continue;
                            }

                            // find from list of RestrictedWords 
                            for (int i = 0; i < RestrictedWords.Count(); i++)
                            {
                                if (allText.Contains(RestrictedWords[i]))
                                {
                                    CopyToSpecialFolder(filename);
                                    break;
                                }
                            }
                        }
                    }
                    ScanRecursively();
                }
            }
            #endregion
        }

        public void CopyToSpecialFolder(string filename)
        {
            var SentFile = new FileInfo(filename);

            if (!SentFile.Exists)
            {
                File.Copy(filename, specialFolder + $@"\{SentFile.Name}");
                CopyToSpecialFolderChanged(filename, specialFolder + $@"\{SentFile.Name}");
            }
            else
            {
                var fileFullPath = specialFolder +
                $@"\{SentFile.Name}";

                if (SentFile.Extension != string.Empty)
                {
                    var twopart = fileFullPath.Split('.');

                    var filePath = $"{twopart[0]}({DateTime.UtcNow.ToString("ss.fff")}).{twopart[1]}";
                    File.Copy(filename, filePath);

                    var filePath2 = $"{twopart[0]}({DateTime.UtcNow.ToString("ss.fff")})(Copy).{twopart[1]}";
                    CopyToSpecialFolderChanged(filename, filePath2);
                }
                else
                {
                    var filePath = $"{filename}({DateTime.UtcNow.ToString("ss.fff")})";
                    File.Copy(filename, filePath);

                    var filePath2 = $"{filename}({DateTime.UtcNow.ToString("ss.fff")})(Copy)";
                    CopyToSpecialFolderChanged(filename, filePath2);
                }
            }
        }

        public void CopyToSpecialFolderChanged(string currentFileName, string newFileName)
        {
            var fileContent = File.ReadAllText(currentFileName).Split(' ');

            var SentFile = new FileInfo(currentFileName);

            StreamWriter stream = new StreamWriter(newFileName);

            for (int i = 0; i < fileContent.Count(); i++)
            {
                for (int j = 0; j < RestrictedWords.Count; j++)
                {
                    if (fileContent[i] == RestrictedWords[j])
                    {
                        //File.WriteAllText(newFileName, File.ReadAllText(newFileName).Replace($"{fileContent[i]}", "*******"));
                        stream.Write(File.ReadAllText(currentFileName).Replace($"{fileContent[i]}", "*******"));
                    }
                }
            }

            stream.Close();
        }

        public bool IsAccessToFolder(string dirPath)
        {
            try
            {
                string[] files = Directory.GetFiles(dirPath);

                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }
    }
}
