using System.Collections.Generic;
using System.Web.Mvc;

namespace DorkWeb.Models
{
    public class GoogleDorkMasterViewModel
    {
        public List<GoogleDorkParentViewModel> GoogleDorkParentViewModels { get; set;}
        public List<string> GoogleDorkParentValues { get; set; }
        public List<int> GoogleDorkParentIds { get; set; }
        public MultiSelectList GoogleDorkParentList { get; set; }
        public string SiteToSearch { get; set; }
        public string Keywords { get; set; }
    }
}