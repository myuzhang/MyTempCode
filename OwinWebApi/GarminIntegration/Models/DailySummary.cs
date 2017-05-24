using System.Collections.Generic;

namespace GarminIntegration.Models
{
    public class DailySummary
    {
        public string SummaryId { get; set; }
        public string ActivityType { get; set; }
        public int ActiveKilocalories { get; set; }
        public int Steps { get; set; }
        public double DistanceInMeters { get; set; }
        public int DurationInSeconds { get; set; }
        public int ActiveTimeInSeconds { get; set; }
        public int StartTimeInSeconds { get; set; }
        public int StartTimeOffsetInSeconds { get; set; }
        public int ModerateIntensityDurationInSeconds { get; set; }
        public int VigorousIntensityDurationInSeconds { get; set; }
        public int FloorsClimbed { get; set; }
        public int MinHeartRateInBeatsPerMinute { get; set; }
        public int AverageHeartRateInBeatsPerMinute { get; set; }
        public int MaxHeartRateInBeatsPerMinute { get; set; }
        public Dictionary<string, int> TimeOffsetHeartRateSamples { get; set; }
    }
}
