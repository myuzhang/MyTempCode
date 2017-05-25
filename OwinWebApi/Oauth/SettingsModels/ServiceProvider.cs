namespace Oauth.SettingsModels
{
    public class ServiceProvider
    {
        public string RequestTokenEndpoint { get; set; }
        public string UserAuthorizationEndpoint { get; set; }
        public string AccessTokenEndpoint { get; set; }
    }
}
