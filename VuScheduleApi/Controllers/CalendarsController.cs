using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;


namespace VuScheduleApi.Controllers
{
    [Route("api/[controller]")]
    public class CalendarsController : Controller
    {
        private CalendarService _service;

        public CalendarsController(CalendarService service)
        {
            _service = service;
        }

        [HttpPost("{groupId}")]
        public async Task<IActionResult> Post([FromBody] SubjectEntry[] subjectEntries, string groupId)
        {
            try
            {
                var task = "https://script.google.com/macros/s/AKfycbyUlYFDJQylwNYhuMvgGJF6-zjO_ByVP9O_ViCIEis8xho-1xot/exec"
                    .SetQueryParams(new
                    {
                        Group = groupId,
                        Time = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss"),
                        IP = HttpContext.Connection.RemoteIpAddress
            })
                    .GetAsync();

                var calendar = await _service.GetCalendarAsync(subjectEntries.GroupBy(x=>x.Title).Select(x=> 
                {
                    var subgroups = x.GroupBy(y => y.Subgroup).Select(y=>y.First().Subgroup).ToList();

                    return 
                        new SubjectOptions
                        {
                            Title = x.First().Title,
                            Subgroups = (subgroups.Count() == 1 && subgroups.First() == null ? new List<int>() : subgroups.Select(y=> (int)y).ToList())
                        };
                }).ToArray(), "mif", groupId);
                var encodedCalendar = Encoding.UTF8.GetBytes(calendar);

                await task;

                var response = File(encodedCalendar, "application/octet-stream");
                return response;
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
