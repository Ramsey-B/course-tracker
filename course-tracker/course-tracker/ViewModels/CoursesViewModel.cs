using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using course_tracker.Models;
using course_tracker.ViewModels;
using Xamarin.Forms;

namespace coursetracker.ViewModels
{
    public class CoursesViewModel : BaseViewModel
    {
        public ObservableCollection<Course> Courses { get; set; }
        public Command LoadCoursesCommand { get; set; }

        private readonly Term _term;

        public CoursesViewModel(Term term = null)
        {
            if (term == null) term = GetDefaultTerm();
            _term = term;

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

            //MessagingCenter.Subscribe<NewTermPage, Term>(this, "AddTerm", async (obj, term) =>
            //{
            //    await LoadTerms();
            //});
        }

        async Task LoadCourses()
        {
            IsBusy = true;
            try
            {
                Courses.Clear();
                if (_term == null) return;
                var courses = await SqliteConn.Table<Course>().Where(c => c.TermId == _term.Id).OrderBy(term => term.Start).ToListAsync();
                foreach (var course in courses)
                {
                    Courses.Add(course);
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
    }
}
