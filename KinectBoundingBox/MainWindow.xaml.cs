using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace KinectBoundingBox
{

    public partial class MainWindow : Window
    {
        BackgroundWorker worker;
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
            this.Closing += (s, a) =>
            {
                this.worker.DoWork -= worker_DoWork;
                this.worker.RunWorkerCompleted -= worker_RunWorkerCompleted;
            };
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.worker = new BackgroundWorker();
            this.worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            this.worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            this.worker.RunWorkerAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(20);
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (this.DataContext != null)
                {
                    var viewModel = (ViewModel)this.DataContext;
                    var image = viewModel.VideoImage;
                    if (image != null)
                    {
                        this.videoImage.Source = image;
                    }
                }
            }));
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.worker.RunWorkerAsync();
        }
    }

}
