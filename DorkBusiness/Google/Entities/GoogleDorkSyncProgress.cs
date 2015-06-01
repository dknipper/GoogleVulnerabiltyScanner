using System;

namespace DorkBusiness.Google.Entities
{
    public class GoogleDorkSyncProgress
    {
        public int ProcessedNumber { get; set; }
        public double PercentageComplete { get; set; }
        public string GoogleDorkParentName { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string GhdbUrl { get; set; }
    }
}