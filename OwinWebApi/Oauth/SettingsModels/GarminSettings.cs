using System.Collections.Generic;

namespace Oauth.SettingsModels
{
    public class GarminSettings
    {
        public string Running { get; set; }
        public List<Environment> Environments { get; set; }
    }
}
