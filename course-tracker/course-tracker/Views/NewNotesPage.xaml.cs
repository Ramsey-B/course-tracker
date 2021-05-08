using System;
using System.Collections.Generic;
using course_tracker.Models;
using course_tracker.ViewModels;
using Xamarin.Forms;

namespace course_tracker.Views
{
    public partial class NewNotesPage : ContentPage
    {
        NewCourseViewModel viewModel;
        public NewNotesPage(Term term, Course course)
        {
            InitializeComponent();

            BindingContext = viewModel = new NewCourseViewModel(term, course);
            Title = $"Update {course.Title}'s Notes";
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            var success = await viewModel.SaveCourse();
            if (success)
            {
                MessagingCenter.Send(this, "AddCourse", viewModel.NewCourse);
                await Navigation.PopModalAsync();
            }
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
