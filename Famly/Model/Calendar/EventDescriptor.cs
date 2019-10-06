using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Famly.Model.Calendar
{
    public class EventDescriptor
    {
        public string LeavyType { get; set; }

        public String Type { get; set; }
    }
}

/*
 type {CHECK_OUT, CHECK_IN}
 leaveType { VACATION, SICK }
     */
