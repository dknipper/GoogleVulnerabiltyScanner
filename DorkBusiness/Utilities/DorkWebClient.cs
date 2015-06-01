using System;
using System.Net;

namespace DorkBusiness.Utilities
{
    public class DorkWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address) as HttpWebRequest;
            if (request == null)
            {
                return null;
            }
            request.UserAgent = AppSettings.Config.FakeScraperUserAgent;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            return request;
        }
    }
}