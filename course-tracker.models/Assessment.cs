using System;

namespace course_tracker.models
{
    public class Assessment
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public AssessmentType Type { get; set; }
        public bool NotifcationsOn { get; set; } = false;
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

    public enum AssessmentType
    {
        Objective,
        Performance
    }
}
