using FamlyCal.Clients;
using FamlyCal.Model;
using System;
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

            return new Calendar
            {
                ProductIdentifier = "-//Microservice.dk//FamlyCal//DA",
                Events = {
                    new Event
                    {
                        Start = DateTime.Now,
                        End = DateTime.Now,
                        Summary = "",
                        Description = ""
                    },
                    new Event
                    {
                        Start = DateTime.Now,
                        End = DateTime.Now,
                        Summary = "Testing",
                        Description = "Now with summary, and stuff"
                    }
                },
            };
        }
    }
}
