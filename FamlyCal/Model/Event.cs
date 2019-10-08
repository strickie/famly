using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamlyCal.Model
{
    public class Event
    {
        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public string Summary { get; set; }

        public string Description { get; set; }

        public TimeSpan? Alarm { get; set; }
    }
}
