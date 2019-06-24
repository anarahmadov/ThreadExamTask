using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ThreadExamTask
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static bool createdNew;
        static Mutex Mutex = new Mutex(true, "ThreadExamTask", out createdNew);

        public App()
        {
            if (!createdNew)
            {
                MessageBox.Show("Application is already running", "Warning");
                App.Current.Shutdown();
            }
        }
    }
}
