using SQLite;
using System;

namespace course_tracker.Models
{
    [Table("Assessments")]
    public class Assessment
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        [NotNull]
        public int CourseId { get; set; }
        [NotNull]
        public string Title { get; set; }
        [NotNull]
        public DateTime Start { get; set; }
        [NotNull]
        public DateTime End { get; set; }
        [NotNull]
        public AssessmentType Type { get; set; }
        public bool NotificationsEnabled { get; set; }
    }

    public enum AssessmentType
    {
        Objective,
        Performance
    }
}
