using System;

namespace DorkBusiness.Google.Entities
{
    public class GoogleDorkSyncProgressChangeEventArgs : EventArgs
    {
        public GoogleDorkSyncProgress ProcessedItem { get; private set; }

        public GoogleDorkSyncProgressChangeEventArgs(GoogleDorkSyncProgress processedItem)
        {
            ProcessedItem = processedItem;
        }
    }
}
