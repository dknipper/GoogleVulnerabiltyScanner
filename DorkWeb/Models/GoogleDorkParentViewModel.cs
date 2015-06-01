using System.Collections.Generic;

namespace DorkWeb.Models
{
    public class GoogleDorkParentViewModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public List<GoogleDorkViewModel> GoogleDorks { get; set; }
    }
}