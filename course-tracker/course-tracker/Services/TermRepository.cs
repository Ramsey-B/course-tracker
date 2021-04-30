using course_tracker.Models;
using course_tracker.Models.extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace course_tracker.Services
{
    public class TermRepository
    {
        private static List<Term> _terms = new List<Term>()
        {
            new Term
            {
                Title = "term 1",
                Id = "term-1-id",
                Start = DateTime.UtcNow.AddDays(-10),
                End = DateTime.UtcNow.AddDays(10).AddMonths(6)
            },
            new Term
            {
                Title = "term 2",
                Id = "term-2-id",
                Start = DateTime.UtcNow.AddDays(10).AddMonths(6),
                End = DateTime.UtcNow.AddDays(10).AddMonths(12)
            },
            new Term
            {
                Title = "term 3",
                Id = "term-3-id",
                Start = DateTime.UtcNow.AddDays(10).AddMonths(12),
                End = DateTime.UtcNow.AddDays(10).AddMonths(18)
            },
            new Term
            {
                Title = "term 4",
                Id = "term-4-id",
                Start = DateTime.UtcNow.AddDays(10).AddMonths(18),
                End = DateTime.UtcNow.AddDays(10).AddMonths(24)
            },
            new Term
            {
                Title = "term 5",
                Id = "term-5-id",
                Start = DateTime.UtcNow.AddDays(10).AddMonths(24),
                End = DateTime.UtcNow.AddDays(10).AddMonths(30)
            }
        };

        public List<Term> GetTerms()
        {
            return _terms.OrderBy(term => term.Start).ToList();
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
