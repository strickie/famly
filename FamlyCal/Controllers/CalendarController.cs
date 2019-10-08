using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamlyCal.Model;
using FamlyCal.Services;
using Microsoft.AspNetCore.Mvc;

namespace FamlyCal.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarService _service;

        public CalendarController(ICalendarService service)
        {
            _service = service;
        }

        // GET calendar
        [HttpGet]
        [Produces("text/calendar")/*, new[] {"application/json"})*/]
        public async Task<ActionResult<Calendar>> GetByCredentialsAsync([FromQuery] Credentials credentials, TimeSpan? alarm = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Calendar calendar = await _service.GetCalendar(credentials.Email, credentials.Password, alarm);

            return calendar;
        }

        // GET calendar
        [HttpGet("{accessToken}")]
        [Produces("text/calendar")]
        public async Task<ActionResult<Calendar>> GetByAccessTokenAsync(Guid accessToken, TimeSpan? alarm = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Calendar calendar = await _service.GetCalendar(accessToken);

            return calendar;
        }
    }
}
