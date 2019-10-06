using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Famly.Model.Calendar
{
    public class Day
    {
        [JsonProperty("day")]
        public DateTime? Date { get; set; }

        [JsonProperty("day_localdate")]
        public DateTime? LocalDate { get; set; }
        
        public ICollection<Event> Events { get; set; }


    }
}
