namespace GarminIntegration.SettingsModels
{
    public class MessageReceivingEndpoint
    {
        public string DailySummaries { get; set; }
        public string ActivitySummaries { get; set; }
        public string BackfillSummaries { get; set; }
        public string SleepSummaries { get; set; }
        public string EpochSummaries { get; set; }
        public string BodyCompositionSummaries { get; set; }
    }

}
