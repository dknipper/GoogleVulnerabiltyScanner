using System.Configuration;

namespace DorkBusiness.AppSettings
{
    public class Config
    {
        public static string GhdbHomePage 
        {
            get { return ConfigurationManager.AppSettings["GhdbHomePage"] ?? string.Empty; }
        }

        public static string FakeScraperUserAgent
        {
            get { return ConfigurationManager.AppSettings["FakeScraperUserAgent"] ?? string.Empty; }
        }
    }
}