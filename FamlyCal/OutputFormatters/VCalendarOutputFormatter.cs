using FamlyCal.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamlyCal.OutputFormatters
{
    public class VCalendarOutputFormatter : TextOutputFormatter
    {
        public VCalendarOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/calendar"));

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanWriteType(Type type)
        {
            if (typeof(Calendar).IsAssignableFrom(type)
             || typeof(IEnumerable<Calendar>).IsAssignableFrom(type))
            {
                return base.CanWriteType(type);
            }
            return false;
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;

            var buffer = new StringBuilder();

            if (context.Object is Calendar)
            {
                FormatVCalendar(buffer, context.Object as Calendar);
            }

            await response.WriteAsync(buffer.ToString());
        }

        private void FormatVCalendar(StringBuilder buffer, Calendar calendar)
        {
            buffer.AppendLine("BEGIN:VCALENDAR");
            buffer.AppendLine("VERSION:2.0");
            buffer.AppendLine("CALSCALE:GREGORIAN");
            buffer.AppendLine("METHOD:PUBLISH");
            buffer.AppendLine("X-WR-CALNAME:Famly kalender");
            buffer.AppendLine("X-WR-TIMEZONE:Europe/Copenhagen");
            buffer.AppendLine("X-PUBLISHED-TTL:PT1H");

            buffer.AppendLine("BEGIN:VTIMEZONE");
            buffer.AppendLine("TZID:Europe/Copenhagen");
            buffer.AppendLine("X-LIC-LOCATION:Europe/Copenhagen");

            buffer.AppendLine("BEGIN:DAYLIGHT");
            buffer.AppendLine("TZOFFSETFROM:+0100");
            buffer.AppendLine("TZOFFSETTO:+0200");
            buffer.AppendLine("TZNAME:CEST");
            buffer.AppendLine("DTSTART:19700329T020000");
            buffer.AppendLine("RRULE:FREQ=YEARLY;BYMONTH=3;BYDAY=-1SU");
            buffer.AppendLine("END:DAYLIGHT");

            buffer.AppendLine("BEGIN:STANDARD");
            buffer.AppendLine("TZOFFSETFROM:+0200");
            buffer.AppendLine("TZOFFSETTO:+0100");
            buffer.AppendLine("TZNAME:CET");
            buffer.AppendLine("DTSTART:19701025T030000");
            buffer.AppendLine("RRULE:FREQ=YEARLY;BYMONTH=10;BYDAY=-1SU");
            buffer.AppendLine("END:STANDARD");

            buffer.AppendLine("END:VTIMEZONE");

            foreach (var ev in calendar.Events)
            {
                FormatVEvent(buffer, ev);
            }

            buffer.AppendLine("END:VCALENDAR");
        }

        private void FormatVEvent(StringBuilder buffer, Event ev)
        {
            buffer.AppendLine("BEGIN:VEVENT");

            // 20140719T063000Z
            buffer.AppendLine($"DTSTART;VALUE=DATE:{ToICalDate(ev.Start)}");
            buffer.AppendLine($"DTEND;VALUE=DATE:{ToICalDate(ev.End)}");
            buffer.AppendLine("DTSTAMP:20191006T172925Z");
            buffer.AppendLine("UID:4frf7tg4omssvfg98kd9fmn09h@google.com");
            //buffer.AppendLine("CREATED:20191003T065401Z");
            buffer.AppendLine($"DESCRIPTION:{ToICalString(ev.Description)}");
            buffer.AppendLine("LAST-MODIFIED:20191003T065401Z");
            buffer.AppendLine("LOCATION:");
            buffer.AppendLine("SEQUENCE:0");
            buffer.AppendLine("STATUS:CONFIRMED");
            buffer.AppendLine($"SUMMARY:{ToICalString(ev.Summary)}");
            buffer.AppendLine("TRANSP:TRANSPARENT");

            buffer.AppendLine("END:VEVENT");
        }

        private string ToICalDate(DateTime date)
        {
            //string DateFormat = "yyyyMMddTHHmmssZ";
            string dateFormat = "yyyyMMddTHHmmssZ";

            return date.ToUniversalTime().ToString(dateFormat);
        }

        private string ToICalString(string str)
        {
            return str.Replace(",", "\\,");
        }
    }


}

/*
 * buffer.AppendLine("VERSION:2.1");
            buffer.AppendFormat($"N:{contact.LastName};{contact.FirstName}\r\n");
            buffer.AppendFormat($"FN:{contact.FirstName} {contact.LastName}\r\n");
            buffer.AppendFormat($"UID:{contact.ID}\r\n");
            
 */
