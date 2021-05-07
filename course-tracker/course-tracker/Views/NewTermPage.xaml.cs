using course_tracker.Models;
using course_tracker.ViewModels;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace course_tracker.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class NewTermPage : ContentPage
    {
        NewTermViewModel viewModel;
        private bool isUpdate = false;

        public NewTermPage(Term existingTerm = null)
        {
            InitializeComponent();
            BindingContext = viewModel = new NewTermViewModel(existingTerm);
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            var success = await viewModel.SaveTerm();
            if (success)
            {
                MessagingCenter.Send(this, "AddTerm", viewModel.NewTerm);
                await Navigation.PopModalAsync();
            }
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}