﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DorkServices.DataContracts
{
    [DataContract]
    public class GoogleDork
    {
        [DataMember]
        public int GoogleDorkParentId { get; set; }

        [DataMember]
        public DateTime? DiscoveryDate { get; set; }

        [DataMember]
        public string GoogleUrl { get; set; }

        [DataMember]
        public string Summary { get; set; }

        [DataMember]
        public string GhdbUrl { get; set; }

        [DataMember]
        public List<GoogleDorkVulnerableSite> VulnerableSites { get; set; }
    }
}