using System.Collections.Generic;
using AutoMapper;
using DorkBusiness.Google.Entities;
using DorkServices.DataContracts;
using DorkServices.ServiceBehaviors;
using DorkServices.ServiceInterfaces;
using GoogleDorkParent = DorkServices.DataContracts.GoogleDorkParent;

namespace DorkServices.Services
{
    [AutomapServiceBehavior]
    public class DorkService : IDorkService
    {
        public List<GoogleDorkParent> GetGoogleDorks(string site)
        {
            return Mapper.Map<List<GoogleDorkParent>>(new GoogleDorkMaster().GetGoogleDorksForSite(site));
        }

        public List<GoogleDorkParent> GetGoogleDorkParents(GoogleDorkParentSort googleDorkParentSort)
        {
            var sort = (DorkBusiness.Google.Enumerations.GoogleDorkParentSort) googleDorkParentSort;
            return Mapper.Map<List<GoogleDorkParent>>(new GoogleDorkMaster().GetGoogleDorkParents(sort));
        }

        public List<GoogleDorkParent> SearchGoogleDorks(string site, string keywords, List<int> googleDorkParentsIds)
        {
            return Mapper.Map<List<GoogleDorkParent>>(new GoogleDorkMaster().SearchGoogleDorks(site, keywords, googleDorkParentsIds));
        }
    }
}
