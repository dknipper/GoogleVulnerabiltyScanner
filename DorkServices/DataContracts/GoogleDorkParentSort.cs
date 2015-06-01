
using System.Runtime.Serialization;

namespace DorkServices.DataContracts
{
    [DataContract]
    public enum GoogleDorkParentSort
    {
        [EnumMember]
        Id = 0,

        [EnumMember]
        Name = 1
    }
}