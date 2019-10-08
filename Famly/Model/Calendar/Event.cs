using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Famly.Model.Calendar
{
    public class Event
    {
        public string Title { get; set; }

        public string SubTitle { get; set; }

        public DateTime? From { get; set; }
        
        public DateTime? To { get; set; }

        [JsonProperty("embed")]
        public EventDescriptor Descriptor { get; set; }

        [JsonProperty("originator")]
        public EventOriginator Originator { get; set; }
    }
}
