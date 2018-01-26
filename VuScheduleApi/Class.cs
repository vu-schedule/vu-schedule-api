using System;

namespace VuScheduleApi
{
    public class Class
    {
        public int? Subgroup { get; set; }
        // null - if the class is for everyone
        // any number - the subgroup id. If this exists,
        //there also exists Class with null subgroup
        public string Title { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public string Teacher { get; set; }
        public bool IsMandatory { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}