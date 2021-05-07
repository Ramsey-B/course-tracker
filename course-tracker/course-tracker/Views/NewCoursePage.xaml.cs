using System;
using System.Collections.Generic;
using course_tracker.Models;
using course_tracker.ViewModels;
using Xamarin.Forms;

namespace course_tracker.Views
{
    public partial class NewCoursePage : ContentPage
    {
        NewCourseViewModel viewModel;

        public List<string> CourseStatuses { get; set; }  = new List<string>
        {
            "Plan to Take",
            "Completed",
            "Dropped",
            "In Progress"
        };

        public NewCoursePage(Term term, Course course = null)
        {
            InitializeComponent();
            BindingContext = viewModel = new NewCourseViewModel(term, course);
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
