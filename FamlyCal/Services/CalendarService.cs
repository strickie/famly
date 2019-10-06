using Famly.Model.Calendar;
using FamlyCal.Clients;
using FamlyCal.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FamlyCal.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly FamlyClient _client;

        public CalendarService(FamlyClient client)
        {
            _client = client;
        }

        public async Task<Calendar> GetCalendar(string email, string password)
        {
            var accessToken = await _client.Login(email, password);

            var calendar = await _client.GetCalendar(new DateTime(2019, 1, 1), FamlyClient.CalendarUnit.MONTH, 12);

            var cal = MapFamlyCalToCalendar(calendar);

            return cal;
        }

        /*
          "embed": {
                            "leaveType": "VACATION",
                            "vacationId": "",
                            "timezone": "Europe/Copenhagen"
                        },

             "embed": {
                            "leaveType": "SICK",
                            "vacationId": "",
                            "timezone": "Europe/Copenhagen"
                        },
                         "embed": {
                            "type": "CHECK_IN",
                            "timezone": "Europe/Copenhagen",
                            "canEdit": false
                        },
                        "behaviors": []
                         "embed": {
                            "type": "CHECK_OUT",
                            "timezone": "Europe/Copenhagen",
                            "canEdit": false
                        },
             */

        private Calendar MapFamlyCalToCalendar(CalendarResponse res)
        {
            Calendar calendar = new Calendar
            {
                ProductIdentifier = "-//Microservice.dk//FamlyCal//DA",
            };

            string[] ignoreTypes = new[] { "CHECK_IN", "CHECK_OUT" };
            string[] ignoreLeaveTypes = new[] { "VACATION", "SICK" };

            foreach (var period in res.Periodes)
            {
                foreach (var day in period.Days)
                {
                    foreach (var ev in day.Events.Where(e => !ignoreTypes.Contains(e.Descriptor.Type) &&
                                                             !ignoreLeaveTypes.Contains(e.Descriptor.LeavyType)))
                    {
                        calendar.Events.Add(new Model.Event
                        {
                            Summary = ev.Title,
                            Description = ev.SubTitle,
                            Start = ev.From??day.Date,
                            End = ev.To??day.Date
                        });
                    }
                }
            }

            return calendar;
        }
    }
}
