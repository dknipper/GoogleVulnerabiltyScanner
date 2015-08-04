using System.Runtime.Serialization;

namespace DorkServices.DataContracts
{
    [DataContract]
    public class GoogleDorkVulnerableSite
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int GoogleDorkId { get; set; }

        [DataMember]
        public string Site { get; set; }

        [DataMember]
        public string Keywords { get; set; }
    }
}
