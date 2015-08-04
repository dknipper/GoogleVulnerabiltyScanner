
using System.Collections.Generic;
using System.Linq;
using DorkDataAccess;

namespace DorkBusiness.Google.Entities
{
    public class GoogleDorkVulnerableSite
    {
        public int Id { get; set; }
        public string Site { get; set; }
        public string Keywords { get; set; }
        public int GoogleDorkId { get; set; }

        public static List<GoogleDorkVulnerableSite> GetGoogleDorkVulnerableSites()
        {
            var googleDorkVulnerableSites = new List<GoogleDorkVulnerableSite>();

            using (var context = new DorkDatabaseContext())
            {
                var vulnerableSites = context.VulnerableSites;
                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (var vulnerableSite in vulnerableSites)
                {
                    googleDorkVulnerableSites.Add(
                        new GoogleDorkVulnerableSite
                        {
                            Id = vulnerableSite.Id,
                            GoogleDorkId = vulnerableSite.GoogleDorkId, 
                            Keywords = vulnerableSite.Keywords, 
                            Site = vulnerableSite.Site
                        });
                }
            }

            return googleDorkVulnerableSites;
        }

        public void Update()
        {
            using (var context = new DorkDatabaseContext())
            {
                var site = context.VulnerableSites.FirstOrDefault(x => x.Id == Id);
                if (site == null)
                {
                    return;
                }
                site.Keywords = Keywords;
                site.Site = Site;
                context.SaveChanges();
            }
        }

        public void Delete()
        {
            using (var context = new DorkDatabaseContext())
            {
                var site = context.VulnerableSites.FirstOrDefault(x => x.Id == Id);
                if (site == null)
                {
                    return;
                }
                context.VulnerableSites.Remove(site);
                context.SaveChanges();
            }
        }
    }
}
