using System;
using System.Collections.Generic;
using course_tracker.Models;
using course_tracker.ViewModels;
using Xamarin.Forms;

namespace coursetracker.Views
{
    public partial class CourseDetailsPage : ContentPage
    {
        CourseDetailsViewModel viewModel;

        public CourseDetailsPage(Course course)
        {
            InitializeComponent();
            BindingContext = viewModel = new CourseDetailsViewModel(course);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.ObjectiveAssessment == null || viewModel.PerformanceAssessment == null) viewModel.IsBusy = true;
        }
    }
}
