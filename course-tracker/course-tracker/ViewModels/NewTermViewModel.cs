using System;
using System.Threading.Tasks;
using course_tracker.Models;
using course_tracker.Models.extensions;

namespace course_tracker.ViewModels
{
    public class NewTermViewModel : BaseViewModel
    {
        Term newTerm = null;
        public Term NewTerm
        {
            get { return newTerm; }
            set { SetProperty(ref newTerm, value); }
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

        private readonly bool isUpdate = false;

        public NewTermViewModel(Term term = null)
        {
            if (term == null)
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                EndDate = StartDate;
                NewTerm = new Term
                {
                    Title = ""
                };
                Title = "New Term";
            }
            else
            {
                StartDate = term.Start;
                EndDate = term.End;
                isUpdate = true;
                NewTerm = term;
                Title = $"Edit {term.Title}";
            }
        }

        public async Task<bool> SaveTerm()
        {
            NewTerm.Start = StartDate;
            NewTerm.End = EndDate;
            if (ValidateTerm())
            {
                if (isUpdate)
                {
                    await SqliteConn.UpdateAsync(NewTerm);
                }
                else
                {
                    await SqliteConn.InsertAsync(NewTerm);
                }
                return true;
            }
            return false;
        }

        private bool ValidateTerm()
        {
            ErrorText = "";

            if (NewTerm.End <= NewTerm.Start) ErrorText = $"* Term end date must be after start date.";

            if (NewTerm.Title.IsNull()) ErrorText = $"* Term must have a Title.";

            return ErrorText.IsNull();
        }
    }
}
