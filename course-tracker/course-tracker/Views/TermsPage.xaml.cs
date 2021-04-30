using System;
using System.ComponentModel;
using Xamarin.Forms;
using course_tracker.ViewModels;
using course_tracker.Models;

namespace course_tracker.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class TermsPage : ContentPage
    {
        TermsViewModel viewModel;

        public TermsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new TermsViewModel();
        }

        async void OnTermSelected(object sender, EventArgs args)
        {
            var layout = (BindableObject)sender;
            var term = (Term)layout.BindingContext;
            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(term)));
        }

        async void AddTerm_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Terms.Count == 0) viewModel.IsBusy = true;
        }
    }
}