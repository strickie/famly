using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Famly.Model.Calendar
{
    public class PeriodDescriptor
    {
        public DateTime? From { get; set; }

        [JsonProperty("from_localdate")]
        public DateTime? FromLocalDate { get; set; }

        public DateTime? To { get; set; }

        [JsonProperty("to_localdate")]
        public DateTime? ToLocalDate { get; set; }        
    }
}
