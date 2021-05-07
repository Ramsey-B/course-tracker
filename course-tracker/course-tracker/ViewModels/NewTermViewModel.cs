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
                Title = $"Update {term.Title}";
            }
        }

        public async Task<bool> SaveTerm()
        {
            NewTerm.Start = StartDate;
            NewTerm.End = EndDate;
            if (await ValidateTerm())
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

        private async Task<bool> ValidateTerm()
        {
            ErrorText = "";

            if (NewTerm.End <= NewTerm.Start) ErrorText = $"* Term end date must be after start date.";

            if (NewTerm.Title.IsNull()) ErrorText = $"* Term must have a Title.";

            var existingTerm = await SqliteConn.Table<Term>()
                .FirstOrDefaultAsync(t =>
                    (NewTerm.Id != t.Id) && // is not the term being updated
                    ((NewTerm.Start >= t.Start && NewTerm.Start < t.End) || // new Term Start is not between start and end dates of existing term
                    (NewTerm.End > t.Start && NewTerm.End <= t.End))); // new Term end is not after another term starts and before the term ends

            if (existingTerm != null) ErrorText = $"A term already exists between {existingTerm.Start:MM/dd/yyyy} and {existingTerm.End:MM/dd/yyyy}.";

            return ErrorText.IsNull();
        }
    }
}
