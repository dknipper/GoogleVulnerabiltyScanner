using System;

namespace DorkWindowsApp.ViewModels
{
    public class GoogleDorkViewModel : BaseViewModel
    {
        public int GoogleDorkParentId { get; set; }
        public string GoogleUrl { get; set; }
        public string Summary { get; set; }
        public string GhdbUrl { get; set; }
        public DateTime DiscoveryDate { get; set; }
    }
}