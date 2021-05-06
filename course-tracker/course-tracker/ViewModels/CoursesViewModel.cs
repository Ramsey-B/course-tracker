using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using course_tracker.Models;
using course_tracker.Views;
using Xamarin.Forms;

namespace course_tracker.ViewModels
{
    public class CoursesViewModel : BaseViewModel
    {
        public ObservableCollection<Course> Courses { get; set; }
        public Command LoadCoursesCommand { get; set; }

        public readonly Term Term;

        public CoursesViewModel(Term term)
        { 
            Term = term;

            if (term != null)
            {
                Title = $"{term.Title}'s courses";
            }
            else
            {
                Title = "No Term Selected";
            }
            Courses = new ObservableCollection<Course>();
            LoadCoursesCommand = new Command(async () => await LoadCourses());

            MessagingCenter.Subscribe<NewCoursePage, Course>(this, "AddCourse", async (obj, course) =>
            {
                await LoadCourses();
            });
        }

        private static object coursesLock = new object();
        async Task LoadCourses()
        {
            IsBusy = true;
            try
            {
                if (Term == null) return;
                var courses = await SqliteConn.Table<Course>().Where(c => c.TermId == Term.Id).OrderBy(term => term.Start).ToListAsync();

                // Loading the data causes the refresh to trigger
                lock (coursesLock)
                {
                    Courses.Clear();
                    foreach (var course in courses)
                    {
                        Courses.Add(course);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task DeleteCourses(Course course)
        {
            await SqliteConn.DeleteAsync(course);
            await LoadCourses();
        }
    }
}
