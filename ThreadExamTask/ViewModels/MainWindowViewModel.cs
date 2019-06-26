using Microsoft.Win32;
using System;
using System.IO;
using System.Threading;
using System.Windows;
using ThreadExamTask.Commands;

namespace ThreadExamTask.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public string ChoosenFileName { get; set; }
        private MainWorks mainWorks = new MainWorks();
        public bool IsChooseFile { get; set; }

        public MainCommand ChooseFileCommand => new MainCommand(body =>
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "";

            if (dialog.ShowDialog() == true && dialog.FileName != null)
            {
                ChoosenFileName = dialog.FileName;
                mainWorks.PutRestrictedWordsFrom(ChoosenFileName);
                IsChooseFile = true;
            }

        });

        public MainCommand StartCommand => new MainCommand(body =>
        {
            if (IsChooseFile)
            {
                Thread tr = new Thread(mainWorks.ScanRecursively);

                tr.Name = "ScanThread";

                tr.Start();
            }
            else MessageBox.Show("There are no restricted words");
        });
    }
}
