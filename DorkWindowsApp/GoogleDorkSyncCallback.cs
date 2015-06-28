using DorkWindowsApp.DorkSyncServiceServiceReference;

namespace DorkWindowsApp
{
    public class GoogleDorkSyncCallback : IDorkSyncServiceCallback
    {
        public GoogleDorkSyncProgress GoogleDorkSyncProgress = new GoogleDorkSyncProgress();
        
        void IDorkSyncServiceCallback.GoogleDorksProcessed(DorkSyncServiceServiceReference.GoogleDorkSyncProgress processedItem)
        {
            GoogleDorkSyncProgress = processedItem;
        }
    }
}