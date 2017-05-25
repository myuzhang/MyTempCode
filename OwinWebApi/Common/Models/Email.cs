using System.Collections.Generic;

namespace Common.Models
{
    public class Email
    {
        public string From { get; set; }
        public string To { get; set; }
        public IList<string> ToList { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }
    }
}
