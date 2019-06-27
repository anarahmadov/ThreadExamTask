using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ThreadExamTask.Domain;

namespace ThreadExamTask.ViewModels
{
    public class MainWorks
    {
        public List<string> RestrictedWords { get; set; }
        private DriveInfo[] drives = DriveInfo.GetDrives();

        public List<CustomFile> FileDetailsForReport { get; set; }

        string specialFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}" + @"\FindRestrictedApp";
        string reportfile = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}" + @"\FindRestrictedApp\report\report.json";
        string reportFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}" + @"\FindRestrictedApp\report";

        //static string path = $@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}";

        string path = @"\\STHQ01DC01\dfr$\Ahma_pf84\Desktop\practice";

        public MainWorks()
        {
            Directory.CreateDirectory(reportFolder);
            Directory.CreateDirectory(specialFolder);
            File.Create(reportfile);
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
                                //MessageBox.Show(allText);
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
                WriteToReportFile(specialFolder + $@"\{SentFile.Name}");
            }
            else
            {
                var fileFullPath = specialFolder +
                $@"\{SentFile.Name}";

                if (SentFile.Extension != string.Empty)
                {
                    var twopart = fileFullPath.Split('.');

                    var filePath = $"{twopart[0]}({DateTime.UtcNow.ToString("ss.fff")}).{twopart.Last()}";
                    File.Copy(filename, filePath);

                    var filePath2 = $"{twopart[0]}({DateTime.UtcNow.ToString("ss.fff")})(Copy).{twopart.Last()}";

                    // copy file with restricted words changes
                    CopyToSpecialFolderChanged(filePath, filePath2);

                    // copy file details to report file
                    WriteToReportFile(filePath);
                }
                else
                {
                    var filePath = $"{filename}({DateTime.UtcNow.ToString("ss.fff")})";
                    File.Copy(filename, filePath);

                    var filePath2 = $"{filename}({DateTime.UtcNow.ToString("ss.fff")})(Copy)";

                    // copy file with restricted words changes
                    CopyToSpecialFolderChanged(filePath, filePath2);

                    // copy file details to report file
                    WriteToReportFile(filePath);
                }
            }
        }

        public void CopyToSpecialFolderChanged(string currentFileName, string newFileName)
        {
            var fileContent = File.ReadAllText(currentFileName).Split(' ');

            //var SentFile = new FileInfo(currentFileName);

            StreamWriter stream = new StreamWriter(newFileName);

            for (int i = 0; i < fileContent.Count(); i++)
            {
                for (int j = 0; j < RestrictedWords.Count; j++)
                {
                    if (fileContent[i] == RestrictedWords[j])
                    {
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

        public void WriteToReportFile(string filename)
        {           
            var fi = new FileInfo(filename);

            FileDetailsForReport = new List<CustomFile>();

            FileDetailsForReport.Add(new CustomFile()
            {
                CreationTime = fi.CreationTime,
                Fullname = fi.FullName,
                Lenght = fi.Length
            });

            var file = new CustomFile()
            {
                CreationTime = fi.CreationTime,
                Lenght = fi.Length,
                Fullname = fi.FullName
            };

            try
            {
                File.AppendAllText(reportfile, JsonConvert.SerializeObject(file));
            }
            catch (Exception)
            {
                
            }

            //File.WriteAllText(reportfile, content);
        }
    }
}
