using System;

namespace DorkBusiness.Google.Entities
{
    public class GoogleDork
    {
        public int GoogleDorkParentId { get; set; }
        public DateTime DiscoveryDate { get; set; }
        public string GoogleUrl { get; set; }
        public string Summary { get; set; }
        public string GhdbUrl { get; set; }
    }
}