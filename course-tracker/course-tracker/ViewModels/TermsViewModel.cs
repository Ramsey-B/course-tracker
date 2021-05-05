using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;
using course_tracker.Views;
using course_tracker.Models;

namespace course_tracker.ViewModels
{
    public class TermsViewModel : BaseViewModel
    {
        public ObservableCollection<Term> Terms { get; set; }
        public Command LoadTermsCommand { get; set; }

        public TermsViewModel()
        {
            Title = "Terms";
            Terms = new ObservableCollection<Term>();
            LoadTermsCommand = new Command(async () => await LoadTerms());

            MessagingCenter.Subscribe<NewTermPage, Term>(this, "AddTerm", async (obj, term) =>
            {
                await LoadTerms();
            });
        }

        private static object termsLock = new object();
        async Task LoadTerms()
        {
            IsBusy = true;
            try
            {
                var terms = await SqliteConn.Table<Term>().OrderBy(term => term.Start).ToListAsync();

                lock (termsLock)
                {
                    Terms.Clear();
                    foreach (var term in terms)
                    {
                        Terms.Add(term);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task DeleteTerm(Term term)
        {
            await SqliteConn.DeleteAsync(term);
            await LoadTerms();
        }
    }
}