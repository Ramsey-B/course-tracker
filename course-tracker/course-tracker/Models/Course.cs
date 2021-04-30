using System;
using System.Collections.Generic;

namespace course_tracker.Models
{
    public class Course
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public CourseStatus Status { get; set; }
        public Assessment ObjectiveAssessment { get; set; }
        public Assessment PerformanceAssessment { get; set; }
        public Instructor Instructor { get; set; }
        public List<string> Notes { get; set; } = new List<string>();
        public bool NotifcationsOn { get; set; } = false;
    }

    public enum CourseStatus
    {
        InProgress,
        Completed,
        Dropped,
        PlanToTake
    }
}
