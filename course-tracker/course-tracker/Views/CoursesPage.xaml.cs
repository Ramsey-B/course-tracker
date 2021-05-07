using System;
using System.Collections.Generic;
using course_tracker.Models;
using course_tracker.ViewModels;
using course_tracker.Views;
using Xamarin.Forms;

namespace course_tracker.Views
{
    public partial class CoursesPage : ContentPage
    {
        CoursesViewModel viewModel;

        public CoursesPage(Term term)
        {
            InitializeComponent();

            BindingContext = viewModel = new CoursesViewModel(term);
        }

        async void AddCourse_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewCoursePage(viewModel.Term)));
        }

        async void OnCourseSelected(object sender, EventArgs args)
        {
            var layout = (BindableObject)sender;
            var course = (Course)layout.BindingContext;
            await Navigation.PushAsync(new CourseDetailsPage(viewModel.Term, course));
        }

        async void EditCourse_Clicked(object sender, EventArgs e)
        {
            var layout = (BindableObject)sender;
            var course = (Course)layout.BindingContext;
            await Navigation.PushModalAsync(new NavigationPage(new NewCoursePage(viewModel.Term, course)));
        }

        async void DeleteCourse_Clicked(object sender, EventArgs args)
        {
            var layout = (BindableObject)sender;
            var course = (Course)layout.BindingContext;

            bool answer = await DisplayAlert("Are you sure?", $"Are you sure you want to delete '{course.Title}' from your courses?", "Yes", "No");
            if (answer)
            {
                await viewModel.DeleteCourses(course);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Courses.Count == 0) viewModel.IsBusy = true;
        }
    }
}
