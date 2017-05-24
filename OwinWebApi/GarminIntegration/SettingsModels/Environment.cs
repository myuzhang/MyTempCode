namespace GarminIntegration.SettingsModels
{
    public class Environment
    {
        public string Platform { get; set; }
        public Credential Credential { get; set; }
        public ServiceProvider ServiceProvider { get; set; }
        public MessageReceivingEndpoint MessageReceivingEndpoint { get; set; }
    }
}
