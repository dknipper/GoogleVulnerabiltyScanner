using System;
using System.Runtime.Serialization;

namespace DorkServices.DataContracts
{
    [DataContract]
    public class GoogleDorkSyncProgress
    {
        [DataMember]
        public int ProcessedNumber { get; set; }

        [DataMember]
        public double PercentageComplete { get; set; }

        [DataMember]
        public string GoogleDorkParentName { get; set; }

        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Summary { get; set; }

        [DataMember]
        public string GhdbUrl { get; set; }
    }
}