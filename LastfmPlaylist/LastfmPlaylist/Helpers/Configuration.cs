using System.Configuration;

namespace LastfmPlaylist.Helpers
{
    public class Configuration
    {
        public static string DeveloperKey
        {
            get
            {
                return ConfigurationManager.AppSettings["DeveloperKey"];
            }
            
        }

        public static string GDataApp
        {
            get
            {
                return ConfigurationManager.AppSettings["GDataApp"];
            }
        }
    }
}