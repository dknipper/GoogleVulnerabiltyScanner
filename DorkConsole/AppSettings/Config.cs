using System.Configuration;

namespace DorkConsole.AppSettings
{
    public class Config
    {
        public static string ServiceUser 
        {
            get { return ConfigurationManager.AppSettings["ServiceUser"] ?? ""; }
        }

        public static string ServicePassword
        {
            get { return ConfigurationManager.AppSettings["ServicePassword"] ?? ""; }
        }
    }
}