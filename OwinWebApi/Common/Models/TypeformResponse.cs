using System;
using System.Collections.Generic;

namespace Common.Models
{
    public class TypeformResponse
    {
        public DateTime DateTaken { get; set; }

        public TypeformResponseHiddenField Hidden { get; set; }

        public Dictionary<string, string> Answers { get; set; }
    }
}
