using System.Collections.Generic;

namespace DorkBusiness.Google.Entities
{
    public class GoogleDorkParent
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public List<GoogleDork> GoogleDorks { get; set; }
    }
}