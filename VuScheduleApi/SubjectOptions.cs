using System.Collections.Generic;

namespace VuScheduleApi
{
    public class SubjectOptions
    {
        public string Title { get; set; }
        public List<int> Subgroups { get; set; } = new List<int>();
    }
}