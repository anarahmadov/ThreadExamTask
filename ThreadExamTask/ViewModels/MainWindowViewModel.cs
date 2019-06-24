using Microsoft.Win32;
using System.Windows;
using ThreadExamTask.Commands;

namespace ThreadExamTask.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public string ChoosenFileName { get; set; }
        private MainWorks mainWorks = new MainWorks();

        public MainCommand ChooseFileCommand => new MainCommand(body =>
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "";

            if (dialog.ShowDialog() == true)
            {
                ChoosenFileName = dialog.FileName;
            }

            mainWorks.PutRestrictedWordsFrom(ChoosenFileName);
        });

        public MainCommand StartCommand => new MainCommand(body =>
        {
            mainWorks.Scan();
        });
    }
}
