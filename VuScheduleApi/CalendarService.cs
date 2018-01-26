using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VuScheduleApi
{
    public class CalendarService
    {
        private WebClient _client;

        public CalendarService(WebClient client)
        {
            _client = client;
        }

        public async Task<string> GetCalendarAsync(SubjectOptions[] options, string facultyId, string groupId)
        {
            var page = await _client.GetClassesPageAsync(facultyId, groupId);

            var classes = ClassParser.Parse(page, groupId);

            var calendarEvents = new List<CalendarEvent>();

            foreach (var c in classes)
            {
                if (!options.Any(x => x.Title == c.Title))
                    continue;

                if (c.Subgroup != null && !options.Any(x => x.Subgroups.Contains((int)c.Subgroup)))
                    continue;

                calendarEvents.Add(new CalendarEvent
                {
                    End = new CalDateTime(c.End, "Europe/Vilnius"),
                    Start = new CalDateTime(c.Start, "Europe/Vilnius"),
                    Summary = c.Title,
                    Location = c.Location,
                    Description = $"{c.Teacher}\n{c.Type}{(c.Subgroup != null ? $"\nPogrupis {c.Subgroup}" : $"")}"
                });
            }

            var calendar = new Calendar();
            calendar.Events.AddRange(calendarEvents);

            var serializer = new CalendarSerializer();
            return serializer.SerializeToString(calendar);
        }
    }
}