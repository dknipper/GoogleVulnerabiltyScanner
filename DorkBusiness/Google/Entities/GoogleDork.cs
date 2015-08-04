using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Runtime.Remoting.Messaging;
using DorkDataAccess;

namespace DorkBusiness.Google.Entities
{
    public class GoogleDork
    {
        public DateTime? DiscoveryDate { get; set; }
        public string GoogleUrl { get; set; }
        public string Summary { get; set; }
        public string GhdbUrl { get; set; }
        public List<GoogleDorkVulnerableSite> VulnerableSites { get; set; }
        public int GoogleDorkParentId { get; set; }

        public static GoogleDork GetGoogleDork(int id)
        {
            using (var context = new DorkDatabaseContext())
            {
                var googleDorkFromDb = context.GoogleDorks.FirstOrDefault(x => x.Id == id);
                if (googleDorkFromDb == null)
                {
                    return null;
                }
                return
                    new GoogleDork
                    {
                        DiscoveryDate = googleDorkFromDb.DiscoveryDate,
                        GhdbUrl = googleDorkFromDb.GhdbUrl,
                        GoogleDorkParentId = googleDorkFromDb.GoogleDorkParentId,
                        GoogleUrl = googleDorkFromDb.GoogleUrl,
                        Summary = googleDorkFromDb.Summary
                    };
            }
        }
    }
}