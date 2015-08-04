using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DorkWeb.Models
{
    public class GoogleDorkViewModel
    {
        public int GoogleDorkParentId { get; set; }
        public string GoogleUrl { get; set; }
        public string Summary { get; set; }
        public string GhdbUrl { get; set; }
        public List<GoogleDorkVulnerableSiteViewModel> VulnerableSites { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? DiscoveryDate { get; set; }
    }
}