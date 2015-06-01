using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace DorkWeb.Models
{
    public class GoogleDorkViewModel
    {
        public int GoogleDorkParentId { get; set; }
        public string GoogleUrl { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DiscoveryDate { get; set; }

        private string _summary;
        public string Summary
        {
            get
            {
                _summary = (string.IsNullOrEmpty(_summary)) ? string.Empty : HttpUtility.HtmlDecode(_summary);
                return _summary;
            }
            set
            {
                _summary = value;
            }
        }

        public string GhdbUrl { get; set; }
    }
}