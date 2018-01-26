using System;
using VuScheduleApi;

namespace VuScheduleScrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new WebClient("https://mif.vu.lt/timetable", "78");

            var groupId = "612i30001-programu-sistemos-3-k-1-gr-2017";

            var data = client.GetClassesPageAsync("mif", groupId).Result;

            var classes = ClassParser.Parse(data, groupId);

        }
    }
}
