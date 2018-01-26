using System.Collections.Generic;

namespace VuScheduleApi
{
    public class StudyProgram
    {
        public string Title { get; set; }
        public List<StudyYear> StudyYears { get; set; } = new List<StudyYear>();
    }

    public class StudyYear
    {
        public string Title { get; set; }
        public List<StudyGroup> StudyGroups { get; set; } = new List<StudyGroup>();
    }

    public class StudyGroup
    {
        public string Id { get; set; }
        public string Title { get; set; }
    }
}