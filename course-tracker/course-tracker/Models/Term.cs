using SQLite;
using System;
using System.Collections.Generic;

namespace course_tracker.Models
{
    [Table("Terms")]
    public class Term
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        [NotNull]
        public string Title { get; set; }
        [NotNull]
        public DateTime Start { get; set; }
        [NotNull]
        public DateTime End { get; set; }
        [Ignore]
        public bool IsCurrent => Start <= DateTime.UtcNow && End > DateTime.UtcNow;
    }
}
