using course_tracker.Models;
using course_tracker.Models.extensions;
using course_tracker.ViewModels;
using course_tracker.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
            BindingContext = viewModel = new NewTermViewModel();

            if (existingTerm == null)
            {
                viewModel.NewTerm = new Term 
                {
                    Title = "",
                    Start = DateTime.Now,
                    End = DateTime.Now
                };
            }
            else
            {
                isUpdate = true;
                viewModel.NewTerm = existingTerm;
            }
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            var success = false;
            if (isUpdate)
            {
                success = await viewModel.UpdateTerm();
            }
            else
            {
                success = await viewModel.AddTerm();
            }
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