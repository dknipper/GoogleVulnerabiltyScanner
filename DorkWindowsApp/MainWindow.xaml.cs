using System;
using System.ComponentModel;
using System.Net;
using System.ServiceModel;
using System.Threading;
using System.Windows;
using DorkWindowsApp.DorkSyncServiceServiceReference;

namespace DorkWindowsApp
{
    public partial class MainWindow
    {
        private GoogleDorkSyncCallback _googleDorkSyncCallback;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            if (button != null)
            {
                button.IsEnabled = false;
                button.Content = "Syncing!";
            }

            _googleDorkSyncCallback = new GoogleDorkSyncCallback();

            var ctx = new InstanceContext(_googleDorkSyncCallback);
            var dorkSyncProxy = new DorkSyncServiceClient(ctx);

            if (dorkSyncProxy.ClientCredentials != null)
            {
                dorkSyncProxy.ClientCredentials.Windows.ClientCredential = new NetworkCredential(AppSettings.Config.ServiceUser, AppSettings.Config.ServicePassword);
                dorkSyncProxy.SyncGoogleDorks();
            }

            var worker = 
                new BackgroundWorker
                {
                    WorkerReportsProgress = true
                };

            worker.DoWork += WorkerDoWork;
            worker.ProgressChanged += WorkerProgressChanged;
            worker.RunWorkerAsync();
        }

        void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            if (worker == null)
            {
                return;
            }
            while (true)
            {
                if (_googleDorkSyncCallback == null || _googleDorkSyncCallback.GoogleDorkSyncProgress == null || _googleDorkSyncCallback.GoogleDorkSyncProgress.PercentageComplete.Equals(100))
                {
                    worker.ReportProgress(100);
                    break;
                }
                worker.ReportProgress(Convert.ToInt32(_googleDorkSyncCallback.GoogleDorkSyncProgress.PercentageComplete));
                Thread.Sleep(100);
            }
            SyncButton.IsEnabled = true;
            SyncButton.Content = "Start!";
        }

        void WorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProcessedTotal.Content = string.Concat("Processed: ", _googleDorkSyncCallback.GoogleDorkSyncProgress.ProcessedNumber);
            DorkParent.Content = string.Concat("Parent:    ", _googleDorkSyncCallback.GoogleDorkSyncProgress.GoogleDorkParentName);
            GhdbUrl.Content = string.Concat("GHDB Url:  ", _googleDorkSyncCallback.GoogleDorkSyncProgress.GhdbUrl);
            GoogleUrl.Content = string.Concat("Google URL:  ", _googleDorkSyncCallback.GoogleDorkSyncProgress.Title);
            PbStatus.Value = e.ProgressPercentage;
        }
    }
}
