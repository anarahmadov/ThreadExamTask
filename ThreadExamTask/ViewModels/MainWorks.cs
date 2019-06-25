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

        static string path = $@"\\STHQ01DC01\dfr$\Ahma_pf84\Desktop\practice";

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

            string allText = "";

            foreach (string dirPath in Directory.GetDirectories(path))
            {
                path = Path.GetFullPath(dirPath);

                foreach (string filePath in Directory.GetFiles(dirPath))
                {
                    string filename = Path.GetFullPath(filePath);

                    allText = File.ReadAllText(filename);

                    for (int i = 0; i < RestrictedWords.Count(); i++)
                    {
                        if (allText.Contains(RestrictedWords[i]))
                        {
                            CopyToSpecialFolder(filename);
                            break;
                        }
                    }
                }

                Scan();

                #endregion
            }
        }

        public void CopyToSpecialFolder(string filename)
        {
            File.Copy(filename, specialFolder + $@"\{new FileInfo(filename).Name}");           
        }

        public void CopyToSpecialFolderChanged(string filename)
        {
            var fileContent = File.ReadAllText(filename);

            for (int i = 0; i < File.ReadAllText(filename).Count(); i++)
            {
                
            }
        }
    }
}
