﻿using System;
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

        public void PutRestrictedWordsFrom(string filename)
        {
            RestrictedWords = new List<string>();

            var arr = File.ReadAllText(filename).Split(' ');

            for (int i = 0; i < arr.Count(); i++)
            {
                RestrictedWords.Add(arr[i]);
            }
        }

        public void Scan()
        {
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

            #region test

            string path = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}";

            string allText = "";

            foreach (var item in )
            {
                foreach (var file in Directory.GetFiles(path))
                {
                    allText = File.ReadAllText(path);
                    if (allText.Contains()) ;
                }
            }

            


            #endregion
        }

        private DirectoryInfo SearchRecursively(string directoryname)
        {
            if (Directory.GetDirectories(directoryname) != null)
            {
                SearchRecursively(directoryname);
            }

            return Directory.CreateDirectory(directoryname);
        }

        public void CopyToSpecialFolder(string filename)
        {

        }
    }
}
