using course_tracker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace course_tracker.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class NewTermPage : ContentPage
    {
        public Term Term { get; set; }

        public NewTermPage(Term existingTerm = null)
        {
            InitializeComponent();

            if (existingTerm == null)
            {
                Term = new Term
                {
                    Title = "Term name"
                };
            }
            else
            {
                Term = existingTerm;
            }

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddTerm", Term);
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}