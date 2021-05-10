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

        Term _term;
        public Term Term
        {
            get { return _term; }
            set { SetProperty(ref _term, value); }
        }

        bool canAddCourse = true;
        public bool CanAddCourse
        {
            get { return canAddCourse; }
            set { SetProperty(ref canAddCourse, value); }
        }

        public CoursesViewModel(Term term)
        { 
            Term = term;
            SubTitle = $"{term.Start:MM/dd/yyyy} - {term.End:MM/dd/yyyy}";
            Title = $"{term.Title}'s courses";

            Courses = new ObservableCollection<Course>();
            LoadCoursesCommand = new Command(async () => await LoadCourses());

            MessagingCenter.Subscribe<NewCoursePage, Course>(this, "AddCourse", async (obj, course) =>
            {
                await LoadCourses();
            });

            MessagingCenter.Subscribe<NewTermPage, Term>(this, "AddTerm", (obj, t) =>
            {
                Term = t;
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
                    CanAddCourse = courses.Count < 6;
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
