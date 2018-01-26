using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

                var response = File(encodedCalendar, "application/octet-stream");
                return response;
            }
            catch
            {
                return null;
            }
        }
    }
}
