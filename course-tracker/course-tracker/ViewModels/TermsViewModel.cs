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
            LoadTermsCommand = new Command(() => LoadTerms());

            MessagingCenter.Subscribe<NewItemPage, Term>(this, "AddTerm", (obj, term) =>
            {
                var newTerm = term as Term;
                TermService.AddTerm(newTerm);
                LoadTerms();
            });
        }

        void LoadTerms()
        {
            IsBusy = true;
            try
            {
                Terms.Clear();
                var terms = TermService.GetTerms();
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