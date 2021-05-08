using System;
using System.Collections.Generic;
using course_tracker.Models;
using course_tracker.ViewModels;
using Xamarin.Forms;

namespace course_tracker.Views
{
    public partial class CourseDetailsPage : ContentPage
    {
        CourseDetailsViewModel viewModel;
        private readonly Term _term;

        public CourseDetailsPage(Term term, Course course)
        {
            InitializeComponent();
            _term = term;
            BindingContext = viewModel = new CourseDetailsViewModel(course);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.ObjectiveAssessment == null || viewModel.PerformanceAssessment == null) viewModel.IsBusy = true;
        }

        async void AddObjectiveAssessment_Clicked(object sender, EventArgs e)
        { 
            await Navigation.PushModalAsync(new NewAssessmentPage(viewModel.Course, AssessmentType.Objective));
        }

        async void AddPerformanceAssessment_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NewAssessmentPage(viewModel.Course, AssessmentType.Performance));
        }

        async void EditCourse_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewCoursePage(_term, viewModel.Course)));
        }

        async void EditObjectiveAssessment_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewAssessmentPage(viewModel.Course, AssessmentType.Objective, viewModel.ObjectiveAssessment)));
        }

        async void EditPerformanceAssessment_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewAssessmentPage(viewModel.Course, AssessmentType.Objective, viewModel.ObjectiveAssessment)));
        }

        async void DeleteObjectiveAssessment_Clicked(object sender, EventArgs args)
        {
            bool answer = await DisplayAlert("Are you sure?", $"Are you sure you want to delete Objective Assessment '{viewModel.ObjectiveAssessment.Title}' from course '{viewModel.Course.Title}'?", "Yes", "No");
            if (answer)
            {
                await viewModel.DeleteAssessment(viewModel.ObjectiveAssessment);
            }
        }

        async void DeletePerformanceAssessment_Clicked(object sender, EventArgs args)
        {
            bool answer = await DisplayAlert("Are you sure?", $"Are you sure you want to delete Performance Assessment '{viewModel.PerformanceAssessment.Title}' from course '{viewModel.Course.Title}'?", "Yes", "No");
            if (answer)
            {
                await viewModel.DeleteAssessment(viewModel.PerformanceAssessment);
            }
        }

        async void EditNotes_Clicked(object sender, EventArgs args)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewNotesPage(_term, viewModel.Course)));
        }
    }
}
