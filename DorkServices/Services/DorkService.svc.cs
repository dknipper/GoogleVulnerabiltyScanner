using System.Collections.Generic;
using AutoMapper;
using DorkBusiness.Google.Utilities;
using DorkServices.DataContracts;
using DorkServices.ServiceBehaviors;
using DorkServices.ServiceInterfaces;

namespace DorkServices.Services
{
    [AutomapServiceBehavior]
    public class DorkService : IDorkService
    {
        public List<GoogleDorkParent> GetGoogleDorks(string site)
        {
            var googleDorkUtilities = new GoogleDorkUtilities();
            return Mapper.Map<List<GoogleDorkParent>>(googleDorkUtilities.GetGoogleDorksFromDatabase(site));
        }

        public List<GoogleDorkParent> GetGoogleDorkParents(GoogleDorkParentSort googleDorkParentSort)
        {
            var googleDorkUtilities = new GoogleDorkUtilities();
            var sort = (DorkBusiness.Google.Enumerations.GoogleDorkParentSort) googleDorkParentSort;
            return Mapper.Map<List<GoogleDorkParent>>(googleDorkUtilities.GetGoogleDorkParentsFromDatabase(sort));
        }

        public List<GoogleDorkParent> SearchGoogleDorks(string site, string keywords, List<int> googleDorkParentsIds)
        {
            var googleDorkUtilities = new GoogleDorkUtilities();
            return Mapper.Map<List<GoogleDorkParent>>(googleDorkUtilities.SearchGoogleDorks(site, keywords, googleDorkParentsIds));
        }
    }
}
