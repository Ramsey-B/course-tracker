using System;
using System.Threading.Tasks;
using course_tracker.Models;
using course_tracker.Models.extensions;
using course_tracker.ViewModels;

namespace coursetracker.ViewModels
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

        public NewTermViewModel()
        {
        }

        public async Task<bool> AddTerm()
        {
            if (await ValidateTerm())
            {
                var id = await SqliteConn.InsertAsync(NewTerm);
                return id > 0;
            }
            return false;
        }

        public async Task<bool> UpdateTerm()
        {
            if (await ValidateTerm())
            {
                var rowCount = await SqliteConn.UpdateAsync(NewTerm);
                return rowCount > 0;
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
