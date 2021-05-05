using course_tracker.Models;
using course_tracker.Models.exceptions;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace course_tracker.Services
{
    public class TermRepository
    {
        private readonly SQLiteAsyncConnection _sqlConn;
        public TermRepository(SQLiteAsyncConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public async Task<List<Term>> GetTermsAsync()
        {
            return await _sqlConn.Table<Term>().OrderBy(term => term.Start).ToListAsync();
        }

        public async Task<Term> GetTermByIdAsync(int id)
        {
            return await _sqlConn.Table<Term>().FirstOrDefaultAsync(term => term.Id == id);
        }

        public async Task<Term> AddTermAsync(Term term)
        {
            var id = await _sqlConn.InsertAsync(term);
            return await GetTermByIdAsync(id);
        }

        public async Task<Term> UpdateTermAsync(Term term)
        {
            await TermDateValidation(term.Start, term.End);
            await _sqlConn.UpdateAsync(term);
            return term;
        }

        public async Task<bool> RemoveTermAsync(int id)
        {
            var count = await _sqlConn.DeleteAsync(new Term { Id = id });
            return count > 0;
        }

        private async Task TermDateValidation(DateTime start, DateTime end)
        {
            if (end <= start) throw new PublicException($"Term end date must be after start date.");

            var existingTerm = await _sqlConn.Table<Term>()
                .FirstOrDefaultAsync(t => 
                    (t.Start >= start && t.End < start) && // new Term Start is not between start and end dates of existing term
                    (t.Start < end && t.End <= end)); // new Term end is not after another term starts and before the term ends

            if (existingTerm != null) throw new PublicException($"A term already exists between {existingTerm.Start} and {existingTerm.End}.");
        }
    }
}
