using System.IO;
using Newtonsoft.Json;

namespace OwinService
{
    public class PlatformManager
    {
        private static PlatformManager _instance;

        private readonly string _json;

        private PlatformManager()
        {
            using (StreamReader r = new StreamReader("Platform.json"))
            {
                _json = r.ReadToEnd();
            }
        }

        public static PlatformManager Instance => _instance ?? (_instance = new PlatformManager());

        public Platform Platform => JsonConvert.DeserializeObject<Platform>(_json);
    }
}
