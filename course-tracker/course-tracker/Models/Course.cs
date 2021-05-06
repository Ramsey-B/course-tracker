using SQLite;
using System;
using System.Collections.Generic;

namespace course_tracker.Models
{
    [Table("Courses")]
    public class Course
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        [NotNull]
        public int TermId { get; set; }
        [NotNull]
        public string Title { get; set; }
        [NotNull]
        public string Status { get; set; }
        [NotNull]
        public DateTime Start { get; set; }
        [NotNull]
        public DateTime End { get; set; }
        [NotNull]
        public string InstructorName { get; set; }
        public string InstructorPhone { get; set; }
        public string InstructorEmail { get; set; }
        public string Notes { get; set; }
        public bool NotificationEnabled { get; set; }
    }
}
