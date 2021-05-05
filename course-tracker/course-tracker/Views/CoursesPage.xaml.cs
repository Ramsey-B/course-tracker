using System;
using System.Collections.Generic;
using course_tracker.Models;
using coursetracker.ViewModels;
using Xamarin.Forms;

namespace course_tracker.Views
{
    public partial class CoursesPage : ContentPage
    {
        CoursesViewModel viewModel;

        public CoursesPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new CoursesViewModel();
        }

        public CoursesPage(Term term)
        {
            InitializeComponent();

            BindingContext = viewModel = new CoursesViewModel(term);
        }

        async void OnCourseSelected(object sender, EventArgs args)
        {

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Courses.Count == 0) viewModel.IsBusy = true;
        }
    }
}
