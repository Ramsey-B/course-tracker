using System;
using System.Collections.Generic;

namespace course_tracker.Models
{
    public class Term
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool NotifcationsOn { get; set; } = false;
        public List<Course> Courses { get; set; }
        public bool IsCurrent => Start <= DateTime.UtcNow && End > DateTime.UtcNow;
    }
}
