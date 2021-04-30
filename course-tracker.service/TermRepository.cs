using course_tracker.models;
using course_tracker.models.extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace course_tracker.service
{
    public class TermRepository
    {
        private static List<Term> _terms = new List<Term>()
        {
            new Term
            {
                Title = "term 1",
                Id = "term-1-id",
                Start = DateTime.Now.AddDays(10),
                End = DateTime.Now.AddDays(10).AddMonths(6)
            }
        };

        public List<Term> GetTerms()
        {
            return _terms.OrderBy(term => term.Start).ToList();
        }

        public Term GetCurrentTerm()
        {
            var currentTerm = GetTerms().FindLast(term => term.Start >= DateTime.UtcNow && term.End < DateTime.UtcNow);
            return currentTerm ?? GetTerms().FindLast(term => term.Start >= DateTime.UtcNow); // Find the next term
        }

        public Term GetTermById(string id)
        {
            return _terms.Find(term => term.Id == id);
        }

        public Term AddTerm(Term term)
        {
            term.Id = Guid.NewGuid().ToString();
            _terms.Add(term);
            return term;
        }

        public Term UpdateTerm(string id, Term term)
        {
            _terms = _terms.Replace(t => t.Id == id, term).ToList();
            return term;
        }

        public bool RemoveTerm(string id)
        {
            return _terms.Remove(t => t.Id == id);
        }
    }
}
