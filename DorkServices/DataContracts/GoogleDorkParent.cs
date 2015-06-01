using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DorkServices.DataContracts
{
    [DataContract]
    public class GoogleDorkParent
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public List<GoogleDork> GoogleDorks { get; set; }
    }
}