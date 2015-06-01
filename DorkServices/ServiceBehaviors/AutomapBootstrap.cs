using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using DorkServices.DataContracts;

namespace DorkServices.ServiceBehaviors
{
    public class AutomapBootstrap
    {
        public static void InitializeMap()
        {
            Mapper.CreateMap<DorkBusiness.Google.Entities.GoogleDork, GoogleDork>();
            Mapper.CreateMap<GoogleDork, DorkBusiness.Google.Entities.GoogleDork>();

            Mapper.CreateMap<DorkBusiness.Google.Entities.GoogleDorkParent, GoogleDorkParent>();
            Mapper.CreateMap<GoogleDorkParent, DorkBusiness.Google.Entities.GoogleDorkParent>();

            Mapper.CreateMap<DorkBusiness.Google.Entities.GoogleDorkSyncProgress, GoogleDorkSyncProgress>();
            Mapper.CreateMap<GoogleDorkSyncProgress, DorkBusiness.Google.Entities.GoogleDorkSyncProgress>();
        }
    }
}