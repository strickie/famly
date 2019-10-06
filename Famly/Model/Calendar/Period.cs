using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Famly.Model.Calendar
{
    public class Period
    {
        [JsonProperty("period")]
        public PeriodDescriptor Descriptor { get; set; }

        public ICollection<Day> Days { get; set; }
    }
}
