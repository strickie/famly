using System;
using System.Collections.Generic;

namespace FamlyCal.Model
{
    public class Calendar
    {
        public string ProductIdentifier { get; set; }

        public TimeSpan Alarm { get; set; } = new TimeSpan(1, 0, 0, 0);

        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}