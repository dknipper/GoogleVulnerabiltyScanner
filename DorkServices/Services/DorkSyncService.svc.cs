using System;
using System.ServiceModel;
using AutoMapper;
using DorkBusiness.Google.Entities;
using DorkBusiness.Google.Utilities;
using DorkServices.ServiceBehaviors;
using DorkServices.ServiceInterfaces;

namespace DorkServices.Services
{
    [AutomapServiceBehavior]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    public class DorkSyncService : IDorkSyncService
    {
        private IDorkSyncServiceProgress _callback;

        public void SyncGoogleDorks()
        {
            _callback = OperationContext.Current.GetCallbackChannel<IDorkSyncServiceProgress>();
            Action serviceAction = ServiceAction;
            serviceAction.BeginInvoke(ar => serviceAction.EndInvoke(ar), null);
        }

        public void ServiceAction()
        {
            var dorkParent = new GoogleDorkUtilities();
            dorkParent.OnGoogleDorkSyncProgressChange += GoogleDorkSyncProgressChange;
            dorkParent.SyncGoogleDorkParents();
            dorkParent.SyncGoogleDorks();
        }

        public void GoogleDorkSyncProgressChange(object sender, GoogleDorkSyncProgressChangeEventArgs e)
        {
            _callback.GoogleDorksProcessed(Mapper.Map<DataContracts.GoogleDorkSyncProgress>(e.ProcessedItem));
        }
    }
}
