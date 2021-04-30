using course_tracker.Models;
using course_tracker.Models.exceptions;
using course_tracker.Models.extensions;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace course_tracker.Services
{
    public class CourseRepository
    {
        private readonly SQLiteAsyncConnection _sqlConn;
        public CourseRepository(SQLiteAsyncConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public async Task<List<Course>> GetCoursesAsync(int termId)
        {
            return await _sqlConn.Table<Course>().Where(c => c.TermId == termId).ToListAsync();
        }

        public async Task<Course> GetCourseByIdAsync(int courseId)
        {
            return await _sqlConn.Table<Course>().FirstOrDefaultAsync(c => c.Id == courseId);
        }

        public async Task<Course> AddCourseAsync(Course course)
        {
            var term = await _sqlConn.Table<Term>().FirstOrDefaultAsync(t => t.Id == course.TermId);
            if (term == null) throw new PublicException("Course must be tied to Term.");

            var courses = await GetCoursesAsync(course.TermId);

            if (courses.Count > 5) throw new PublicException("Cannot add course to term. Maximum number of 6 courses reached.");
            ValidateCourse(term, course);

            var id = await _sqlConn.InsertAsync(course);
            return await GetCourseByIdAsync(id);
        }

        public async Task<Course> UpdateCourseAsync(Course course)
        {
            var term = await _sqlConn.Table<Term>().FirstOrDefaultAsync(t => t.Id == course.TermId);
            if (term == null) throw new PublicException("Course must be tied to Term.");

            ValidateCourse(term, course);
            await _sqlConn.UpdateAsync(course);
            return course;
        }

        public async Task<bool> RemoveCourseAsync(int id)
        {
            var count = await _sqlConn.DeleteAsync(new Course { Id = id });
            return count > 0;
        }

        private void ValidateCourse(Term term, Course course)
        {
            if (course.Title.IsNull()) throw new PublicException("Must provide a course name.");

            if (course.Start == null || course.End == null) throw new PublicException("Must provide a course start and end date.");

            if (course.Start >= course.End) throw new PublicException("Course start date can not be after course end date.");

            if (course.InstructorName == null) throw new PublicException("Must provide course instructor's information.");

            if (course.InstructorEmail.IsValidEmail()) throw new PublicException("Must provide a valid email for course instructor.");

            if (course.InstructorPhone.IsValidPhoneNumber()) throw new PublicException("Must provide a valid phone number for course instructor.");

            if (term.Start < course.Start) throw new PublicException("Course cannot begin before term start date.");

            if (term.End > course.End) throw new PublicException("Course cannot end after term end date.");
        }
    }
}
