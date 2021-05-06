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

        private readonly bool isUpdate = false;

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
            BindingContext = viewModel = new NewCourseViewModel(term);

            if (course != null)
            {
                viewModel.NewCourse = course;
                isUpdate = true;
            }
            else
            {
                viewModel.NewCourse = new Course
                {
                    TermId = term.Id,
                    Title = "",
                    Start = DateTime.Now,
                    End = DateTime.Now,
                    InstructorEmail = "",
                    InstructorName = "",
                    InstructorPhone = "",
                    Status = "Plan to Take"
                };
            }
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            bool success;
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
