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

        DateTime startDate;
        public DateTime StartDate
        {
            get { return startDate; }
            set { SetProperty(ref startDate, value); }
        }

        DateTime endDate;
        public DateTime EndDate
        {
            get { return endDate; }
            set { SetProperty(ref endDate, value); }
        }

        DateTime maxDate;
        public DateTime MaxDate
        {
            get { return maxDate; }
            set { SetProperty(ref maxDate, value); }
        }

        public readonly Term Term;
        private readonly bool isUpdate = false;

        public NewCourseViewModel(Term term, Course course = null)
        {
            MaxDate = term.End;
            Term = term;

            if (course != null)
            {
                StartDate = course.Start;
                EndDate = course.End;
                NewCourse = course;
                isUpdate = true;
            }
            else
            {
                StartDate = term.Start;
                EndDate = term.Start;
                NewCourse = new Course
                {
                    TermId = term.Id,
                    Title = "",
                    InstructorEmail = "",
                    InstructorName = "",
                    InstructorPhone = "",
                    Status = "Plan to Take"
                };
            }
        }

        public async Task<bool> SaveCourse()
        {
            NewCourse.Start = StartDate;
            NewCourse.End = EndDate;
            if (await ValidateCourse(NewCourse))
            {
                if (isUpdate)
                {
                    await SqliteConn.UpdateAsync(NewCourse);
                }
                else
                {
                    await SqliteConn.InsertAsync(NewCourse);
                }
                return true;
            }
            return false;
        }

        private async Task<bool> ValidateCourse(Course course)
        {
            ErrorText = "";
            if (course.Title.IsNull()) ErrorText = "* Must provide a course name.";

            if (course.Start == null || course.End == null) ErrorText ="* Must provide a course start and end date.";

            if (course.Start >= course.End) ErrorText = "* Course start date can not be after course end date.";

            if (course.Status.IsNull()) ErrorText = "* Must select a course status.";

            if (course.InstructorName.IsNull()) ErrorText = "* Must provide course instructor's information.";

            if (!course.InstructorEmail.IsValidEmail()) ErrorText = "* Must provide a valid email for course instructor.";

            if (!course.InstructorPhone.IsValidPhoneNumber()) ErrorText = "* Must provide a valid phone number for course instructor.";

            if (Term.Start > course.Start) ErrorText = $"* Course cannot begin before term start date of {Term.Start}.";

            if (Term.End < course.End) ErrorText = $"* Course cannot end after term end date of {Term.End}.";

            var courseCount = await SqliteConn.Table<Course>().Where(c => c.TermId == Term.Id).CountAsync();
            if (courseCount > 5) ErrorText = $"* Term '{Term.Title}' has already reached the maximum number of courses of 6.";

            return ErrorText.IsNull();
        }
    }
}
