using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ThreadExamTask.ViewModels
{
    public class MainWorks
    {
        public List<string> RestrictedWords { get; set; }
        private DriveInfo[] drives = DriveInfo.GetDrives();
        string specialFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}" + @"\FindRestrictedApp";

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

        public void Scan()
        {
            #region Real code
            //foreach (var drive in drives)
            //{
            //    foreach (var directory in drive.RootDirectory.GetDirectories())
            //    {
            //        var lastdirectory = SearchRecursively(directory.FullName);
            //
            //        foreach (var file in lastdirectory.GetFiles())
            //        {
            //            //if (File.ReadAllText(file.Name).Contains())
            //        }
            //    }
            //}

            #endregion

            #region test

            string path = $@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\Desktop\test";

            var StartDirectory = new DirectoryInfo(path);

            string allText = "";

            foreach (var directory in StartDirectory.GetDirectories())
            {
                var lastdirectory = SearchRecursively(directory.FullName);

                foreach (var file in lastdirectory.GetFiles())
                {
                    allText = File.ReadAllText(file.FullName);

                    for (int i = 0; i < RestrictedWords.Count; i++)
                    {
                        if (allText.Contains(RestrictedWords[i]))
                        {
                            CopyToSpecialFolder(file.FullName);
                            break;
                        }
                    }
                }
            }

            #endregion
        }

        private DirectoryInfo SearchRecursively(string directoryname)
        {
            var lastDirectory = new DirectoryInfo(directoryname);

            if (Directory.GetDirectories(directoryname).Count() != 0)
            {
                directoryname = lastDirectory.GetDirectories()[0].
                    FullName;
                return SearchRecursively(directoryname);
            }

            return lastDirectory;
        }

        public void CopyToSpecialFolder(string filename)
        {
            File.Copy(filename, specialFolder + $@"\{new FileInfo(filename).Name}");
        }
    }
}
