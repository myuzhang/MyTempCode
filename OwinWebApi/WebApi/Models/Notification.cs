namespace WebApi.Models
{
    public class Notification
    {
        public string UserAccessToken { get; set; }
        public int UploadStartTimeInSeconds { get; set; }
        public int UploadEndTimeInSeconds { get; set; }
    }
}