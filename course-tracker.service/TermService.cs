using course_tracker.models;
using course_tracker.models.exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace course_tracker.service
{
    public class TermService
    {
        private readonly TermRepository _termRepository;

        public TermService(TermRepository termRepository)
        {
            _termRepository = termRepository;
        }

        public TermService()
        {

        }

        public List<Term> GetTerms()
        {
            return _termRepository.GetTerms();
        }

        public Term GetTermsById(string id)
        {
            return _termRepository.GetTermById(id);
        }

        public Term GetCurrentTerm()
        {
            return _termRepository.GetCurrentTerm();
        }

        public Term AddTerm(Term term)
        {
            var terms = GetTerms();
            if (term.Courses != null && term.Courses.Count > 6)
            {
                throw new PublicException("Terms have a maximum of 6 courses.");
            }
            var existingTerm = terms.Find(t =>
            {
                var startDate = t.Start >= term.Start && t.End < term.Start; // new Term Start is not between start and end dates of existing term
                var endDate = t.Start < term.End && t.End <= term.End; // new Term end is not after another term starts and before the term ends
                return startDate && endDate;
            });

            if (existingTerm != null)
            {
                throw new PublicException($"A term already exists between {existingTerm.Start} and {existingTerm.End}.");
            }

            return _termRepository.AddTerm(term);
        }

        public Term UpdateTerm(string id, Term term)
        {
            if (term.Courses == null) term.Courses = new List<Course>();

            if (term.Courses.Count > 6)
            {
                throw new PublicException("Terms have a maximum of 6 courses.");
            }

            var terms = GetTerms();
            var existingTerm = terms.Find(t =>
            {
                var startDate = t.Start >= term.Start && t.End < term.Start; // new Term Start is not between start and end dates of existing term
                var endDate = t.Start < term.End && t.End <= term.End; // new Term end is not after another term starts and before the term ends
                return startDate && endDate && t.Id != id;
            });

            if (existingTerm != null)
            {
                throw new PublicException($"A term already exists between {existingTerm.Start} and {existingTerm.End}.");
            }

            return _termRepository.UpdateTerm(id, term);
        }

        public bool RemoveTerm(string id)
        {
            return _termRepository.RemoveTerm(id);
        }
    }
}
