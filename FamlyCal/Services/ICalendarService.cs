using FamlyCal.Model;
using System.Threading.Tasks;

namespace FamlyCal.Services
{
    public interface ICalendarService
    {
        Task<Calendar> GetCalendar(string email, string password);
    }
}
