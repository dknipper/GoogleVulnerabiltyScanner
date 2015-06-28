using System.Configuration;

namespace DorkWeb.AppSettings
{
    public class Config
    {
        public static string DorkServiceActiveEndpoint 
        {
            get { return ConfigurationManager.AppSettings["DorkServiceActiveEndpoint"] ?? string.Empty; }
        }
    }
}