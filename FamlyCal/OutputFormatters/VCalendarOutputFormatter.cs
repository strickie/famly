using FamlyCal.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FamlyCal.OutputFormatters
{
    public class VCalendarOutputFormatter : TextOutputFormatter
    {
        private const int VCalMaxLineLength = 75;
        private const string CRLF = "\r\n";
        private readonly TimeSpan PublishedTTL = new TimeSpan(days: 0, hours: 1, minutes: 0, seconds: 0);

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
            buffer.Append("BEGIN:VCALENDAR\r\n");
            buffer.AppendLine($"PRODID:{calendar.ProductIdentifier}");
            buffer.AppendLine("VERSION:2.0");
            buffer.AppendLine("CALSCALE:GREGORIAN");
            buffer.AppendLine("METHOD:PUBLISH");
            buffer.AppendLine("X-WR-CALNAME:B\u00f8rnehuset ved Damhuss\u00f8en");
            buffer.AppendLine("X-WR-CALDESC:B\u00f8rnehuset ved Damhuss\u00f8en");
            buffer.AppendLine("X-WR-TIMEZONE:Europe/Copenhagen");
            buffer.AppendLine($"X-PUBLISHED-TTL:{ToICalTimeSpan(PublishedTTL)}");

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

            if (ev.Start.HasValue)
            {
                buffer.AppendLine(ToICalDate("DTSTART", ev.Start.Value));
            }

            if (ev.End.HasValue)
            {
                buffer.AppendLine(ToICalDate("DTEND", ev.End.Value));
            }

            //buffer.AppendLine("DTSTAMP:20191006T172925Z");
            buffer.AppendLine($"UID:{Guid.NewGuid()}");
            //buffer.AppendLine("CREATED:20191003T065401Z");
            buffer.Append(FoldLine($"DESCRIPTION:{ev.Description}"));
            //buffer.AppendLine("LAST-MODIFIED:20191003T065401Z");
            //buffer.AppendLine("LOCATION:");
            //buffer.AppendLine("SEQUENCE:0");
            buffer.AppendLine("STATUS:CONFIRMED");
            buffer.Append(FoldLine($"SUMMARY:{ev.Summary}"));
            buffer.AppendLine("TRANSP:TRANSPARENT");

            // Alarm
            if (ev.Alarm.HasValue)
            {
                buffer.AppendLine("BEGIN:VALARM");
                buffer.AppendLine("ACTION:DISPLAY");
                buffer.AppendLine("DESCRIPTION:P\u00e5mindelse");
                buffer.AppendLine($"TRIGGER:-{ToICalTimeSpan(ev.Alarm.Value)}");
                buffer.AppendLine("END:VALARM");
            }

            buffer.AppendLine("END:VEVENT");
        }

        private string FoldLine(string line)
        {
            StringBuilder output = new StringBuilder();

            if (string.IsNullOrWhiteSpace(line))
            {
                return line;
            }

            string input = EscapeStrings(line);

            if (input.Length > VCalMaxLineLength)
            {
                output.Append($"{input.Substring(0, 75)}{CRLF}");
                input = input.Remove(0, 75);

                while (input.Length > 74)
                {
                    output.Append($" {input.Substring(0, 74)}{CRLF}");
                    input = input.Remove(0, 74);
                }

                output.Append($" {input}{CRLF}");

                return output.ToString();
            }

            return $"{input}{CRLF}";
        }

        public static string ReplaceText(string value, IList<(string from, string to)> pairs)
        {
            foreach (var pair in pairs)
            {
                value = value.Replace(pair.from, pair.to);
            }

            return value;
        }
        public static string EscapeStrings(string value)
        {
            return ReplaceText(value, new List<(string, string)>
            {
                ("\\", "\\\\"),
                (";",  @"\;"),
                (",",  @"\,"),
                ("\r\n",  @"\n"),
                ("\n",  @"\n"),
            });
        }

        private string ToICalTimeSpan(TimeSpan timeSpan)
        {
            return XmlConvert.ToString(timeSpan);
        }

        private string ToICalDate(string type, DateTime date)
        {
            string dateTimeFormat = "yyyyMMddTHHmmssZ";
            string dateFormat = "yyyyMMdd";

            string format = string.Empty;
            string valueType = string.Empty;

            // No time portion
            if (date.Date == date)
            {
                valueType = "DATE";
                format = dateFormat;
            }
            else
            {
                valueType = "DATE-TIME";
                format = dateTimeFormat;
            }

            return $"{type};VALUE={valueType}:{date.ToUniversalTime().ToString(format)}";
        }
    }
}