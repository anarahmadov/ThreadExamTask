using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ThreadExamTask.Commands;

namespace ThreadExamTask.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public string ChoosenFileName { get; set; }
        private MainWorks mainWorks;
        public bool IsChooseFile { get; set; }

        private double progressBarValue;
        public double ProgressBarValue
        {
            get
            {
                return progressBarValue;
            }
            set
            {
                progressBarValue = value;
                OnProppertyChanged(new PropertyChangedEventArgs(nameof(ProgressBarValue)));
            }
        }

        public MainCommand ChooseFileCommand => new MainCommand(body =>
        {
            mainWorks = new MainWorks(this);

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
                Task.Run(() => mainWorks.GetAllFilesCount(mainWorks.path))
                .ContinueWith((task) =>
                {
                    Task.Run(() => mainWorks.ScanRecursively(mainWorks.path));
                    Task.Run(() => mainWorks.CopyToSpecialFolder());
                });
            }
            else MessageBox.Show("There is no restricted words file.");
        });

        [Obsolete]
        public MainCommand PauseCommand => new MainCommand((body) =>
        {
            Thread.CurrentThread.Suspend();
        });

        [Obsolete]
        public MainCommand ResumeCommand => new MainCommand((body) =>
        {
            Thread.CurrentThread.Resume();
        });

        [Obsolete]
        public MainCommand StopCommand => new MainCommand((body) =>
        {
            Thread.CurrentThread.Abort();
        });
    }
}
