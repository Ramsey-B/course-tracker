using System;
using System.Threading.Tasks;
using course_tracker.Models;
using course_tracker.Models.extensions;

namespace course_tracker.ViewModels
{
    public class NewCourseViewModel : BaseViewModel
    {
        Course newCourse = null;
        public Course NewCourse
        {
            get { return newCourse; }
            set { SetProperty(ref newCourse, value); }
        }

        string errorText = "";
        public string ErrorText
        {
            get { return errorText; }
            set { SetProperty(ref errorText, value); }
        }

        private readonly Term _term;

        public NewCourseViewModel(Term term)
        {
            _term = term;
        }

        public async Task<bool> AddTerm()
        {
            if (await ValidateCourse(NewCourse))
            {
                var id = await SqliteConn.InsertAsync(NewCourse);
                return id > 0;
            }
            return false;
        }

        public async Task<bool> UpdateTerm()
        {
            if (await ValidateCourse(NewCourse))
            {
                var rowCount = await SqliteConn.UpdateAsync(NewCourse);
                return rowCount > 0;
            }
            return false;
        }

        private async Task<bool> ValidateCourse(Course course)
        {
            if (course.Title.IsNull()) ErrorText = "* Must provide a course name.";

            if (course.Start == null || course.End == null) ErrorText ="* Must provide a course start and end date.";

            if (course.Start >= course.End) ErrorText = "* Course start date can not be after course end date.";

            if (course.InstructorName == null) ErrorText = "* Must provide course instructor's information.";

            if (!course.InstructorEmail.IsValidEmail()) ErrorText = "* Must provide a valid email for course instructor.";

            if (!course.InstructorPhone.IsValidPhoneNumber()) ErrorText = "* Must provide a valid phone number for course instructor.";

            if (_term.Start > course.Start) ErrorText = "* Course cannot begin before term start date.";

            if (_term.End < course.End) ErrorText = "* Course cannot end after term end date.";

            var courseCount = await SqliteConn.Table<Course>().Where(c => c.TermId == _term.Id).CountAsync();
            if (courseCount > 5) ErrorText = $"* Term '{_term.Title}' has already reached the maximum number of courses of 6.";

            return ErrorText.IsNull();
        }
    }
}
