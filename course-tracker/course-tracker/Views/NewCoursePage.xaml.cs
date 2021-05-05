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
                    Status = CourseStatus.PlanToTake
                };
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
