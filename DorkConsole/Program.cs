using System;
using System.Net;
using System.ServiceModel;
using DorkConsole.DorkSyncServiceReference;

namespace DorkConsole
{
    class Program
    {
        // ReSharper disable once UnusedParameter.Local
        static void Main(string[] args)
        {
            var ctx = new InstanceContext(new Callback());
            var dorkSyncProxy = new DorkSyncServiceClient(ctx);

            if (dorkSyncProxy.ClientCredentials != null)
            {
                dorkSyncProxy.ClientCredentials.Windows.ClientCredential = new NetworkCredential(AppSettings.Config.ServiceUser, AppSettings.Config.ServicePassword);
                dorkSyncProxy.SyncGoogleDorks();
            }

            Console.ReadLine();
        }
    }

    public class Callback : IDorkSyncServiceCallback
    {
        void IDorkSyncServiceCallback.GoogleDorksProcessed(GoogleDorkSyncProgress processedItem)
        {
            Console.WriteLine("Complete:  {0}{1}", processedItem.PercentageComplete.ToString("N2"), "%");
            Console.WriteLine(string.Concat("Processed: ", processedItem.ProcessedNumber));
            Console.WriteLine(string.Concat("Parent:    ", processedItem.GoogleDorkParentName));
            Console.WriteLine(string.Concat("GHDB Url:  ", processedItem.GhdbUrl));
            Console.WriteLine(string.Concat("Google URL:  ", processedItem.Title));
            Console.WriteLine("");
        }
    }
}
