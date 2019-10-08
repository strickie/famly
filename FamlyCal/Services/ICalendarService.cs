using FamlyCal.Model;
using System;
using System.Threading.Tasks;

namespace FamlyCal.Services
{
    public interface ICalendarService
    {
        Task<Calendar> GetCalendar(string email, string password, TimeSpan? alarm = null);
        Task<Calendar> GetCalendar(Guid accessToken, TimeSpan? alarm = null);
    }
}
