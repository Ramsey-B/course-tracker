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
                var newTerm = term as Term;
                await TermRepository.AddTermAsync(newTerm);
                await LoadTerms();
            });
        }

        async Task LoadTerms()
        {
            IsBusy = true;
            try
            {
                Terms.Clear();
                var terms = await TermRepository.GetTermsAsync();
                foreach (var term in terms)
                {
                    Terms.Add(term);
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
    }
}