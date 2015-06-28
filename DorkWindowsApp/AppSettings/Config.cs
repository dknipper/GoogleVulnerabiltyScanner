using System.Configuration;

namespace DorkWindowsApp.AppSettings
{
    public class Config
    {
        public static string ServiceUser 
        {
            get { return ConfigurationManager.AppSettings["ServiceUser"] ?? string.Empty; }
        }

        public static string ServicePassword
        {
            get { return ConfigurationManager.AppSettings["ServicePassword"] ?? string.Empty; }
        }
    }
}