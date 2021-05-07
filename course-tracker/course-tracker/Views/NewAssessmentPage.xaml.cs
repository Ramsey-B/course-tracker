using System;
using System.Collections.Generic;
using course_tracker.Models;
using course_tracker.ViewModels;
using Xamarin.Forms;

namespace course_tracker.Views
{
    public partial class NewAssessmentPage : ContentPage
    {
        NewAssessmentViewModel viewModel;

        public NewAssessmentPage(Course course, AssessmentType type, Assessment assessment = null)
        {
            InitializeComponent();

            BindingContext = viewModel = new NewAssessmentViewModel(course, type, assessment);
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            var success = await viewModel.SaveAssessment();
            if (success)
            {
                var msg = viewModel.NewAssessment.Type == AssessmentType.Objective ? "AddObjectiveAssessment" : "AddPerformanceAssessment";
                MessagingCenter.Send(this, msg, viewModel.NewAssessment);
                await Navigation.PopModalAsync();
            }
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
