using System;
using System.Threading.Tasks;
using course_tracker.Models;
using course_tracker.Models.extensions;

namespace course_tracker.ViewModels
{
    public class NewAssessmentViewModel : BaseViewModel
    {
        private Assessment newAssessment = null;
        public Assessment NewAssessment
        {
            get { return newAssessment; }
            set { SetProperty(ref newAssessment, value); }
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

        public readonly Course Course;
        private readonly bool isUpdate = false;

        public NewAssessmentViewModel(Course course, AssessmentType type, Assessment assessment = null)
        {
            MaxDate = course.End;
            if (assessment == null)
            {
                StartDate = course.Start;
                EndDate = course.Start;
                Title = type == AssessmentType.Objective ? "New Objective Assessment" : "New Performance Assessment";
                NewAssessment = new Assessment()
                {
                    Title = ""
                };
            }
            else
            {
                Title = $"Update {assessment.Title}";
                StartDate = assessment.Start;
                EndDate = assessment.End;
                NewAssessment = assessment;
            }
            Course = course;
            NewAssessment.Type = type;
        }

        public async Task<bool> SaveAssessment()
        {
            NewAssessment.Start = StartDate;
            NewAssessment.End = EndDate;
            if (!ValidateAssessment()) return false;
            if (isUpdate)
            {
                await SqliteConn.UpdateAsync(NewAssessment);
            }
            else
            {
                await SqliteConn.InsertAsync(NewAssessment);
            }
            return true;
        }

        private bool ValidateAssessment()
        {
            ErrorText = "";
            if (NewAssessment.Title.IsNull()) ErrorText = "Assement must have a name.";

            if (NewAssessment.Start >= NewAssessment.End) ErrorText = "Assements start date must be before the end date.";

            if (Course.Start > NewAssessment.Start) ErrorText = $"Assement cannot begin before course start date of {Course.Start:MM/dd/yyyy}.";

            if (Course.End < NewAssessment.End) ErrorText = $"Assement cannot end after course end date of {Course.End:MM/dd/yyyy}.";

            return ErrorText.IsNull();
        }
    }
}
