using System.ServiceModel;
using DorkServices.DataContracts;

namespace DorkServices.ServiceInterfaces
{
    [ServiceContract(CallbackContract = typeof(IDorkSyncServiceProgress))]
    public interface IDorkSyncService
    {
        [OperationContract(IsOneWay = true)]
        void SyncGoogleDorks();
    }

    public interface IDorkSyncServiceProgress
    {
        [OperationContract]
        void GoogleDorksProcessed(GoogleDorkSyncProgress processedItem);
    }
}
