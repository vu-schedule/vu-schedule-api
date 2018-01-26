namespace VuScheduleApi
{
    public class Subject
    {
        public string Title { get; set; }
        public int? SubgroupsCount { get; set; }
        public bool IsMandatory { get; internal set; }
    }
}