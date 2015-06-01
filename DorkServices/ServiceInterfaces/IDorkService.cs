using System.Collections.Generic;
using System.ServiceModel;
using DorkServices.DataContracts;

namespace DorkServices.ServiceInterfaces
{
    [ServiceContract]
    public interface IDorkService
    {
        [OperationContract]
        List<GoogleDorkParent> GetGoogleDorks(string site);

        [OperationContract]
        List<GoogleDorkParent> SearchGoogleDorks(string site, string keywords, List<int> googleDorkParentIds);

        [OperationContract]
        List<GoogleDorkParent> GetGoogleDorkParents(GoogleDorkParentSort googleDorkParentSort);
    }
}
