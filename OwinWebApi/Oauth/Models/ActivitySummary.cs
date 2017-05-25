namespace Oauth.Models
{
    public class ActivitySummary
    {
        public string SummaryId { get; set; }
        public string ActivityType { get; set; }
        public int StartTimeInSeconds { get; set; }
        public int StartTimeOffsetInSeconds { get; set; }
        public int DurationInSeconds { get; set; }
        public double AverageSpeedInMetersPerSecond { get; set; }
        public double DistanceInMeters { get; set; }
        public int ActiveKilocalories { get; set; }
        public string DeviceName { get; set; }
        public double AveragePaceInMinutesPerKilometer { get; set; }
    }
}
