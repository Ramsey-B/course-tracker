using System;
using System.Diagnostics;
using System.Threading.Tasks;
using course_tracker.Models;
using Xamarin.Forms;

namespace course_tracker.ViewModels
{
    public class CourseDetailsViewModel : BaseViewModel
    {
        private Course course;
        public Course Course
        {
            get { return course; }
            set { SetProperty(ref course, value); }
        }

        public Command LoadAssessmentsCommand { get; set; }

        private Assessment objectiveAssessment;
        public Assessment ObjectiveAssessment
        {
            get { return objectiveAssessment; }
            set { SetProperty(ref objectiveAssessment, value); }
        }

        private bool hasObjectiveAssessment = false;
        public bool HasObjectiveAssessment
        {
            get { return hasObjectiveAssessment; }
            set { SetProperty(ref hasObjectiveAssessment, value); }
        }

        private Assessment performanceAssessment;
        public Assessment PerformanceAssessment
        {
            get { return performanceAssessment; }
            set { SetProperty(ref performanceAssessment, value); }
        }

        private bool hasPerformanceAssessment = false;
        public bool HasPerformanceAssessment
        {
            get { return hasPerformanceAssessment; }
            set { SetProperty(ref hasPerformanceAssessment, value); }
        }

        public CourseDetailsViewModel(Course course)
        {
            Course = course;
            Title = course.Title;
            LoadAssessmentsCommand = new Command(async () => await LoadAssessments());

            //MessagingCenter.Subscribe<NewCoursePage, Course>(this, "AddCourse", async (obj, course) =>
            //{
            //    await LoadCourses();
            //});
        }

        private static object assessmentsLock = new object();
        private async Task LoadAssessments()
        {
            IsBusy = true;
            HasObjectiveAssessment = false;
            HasPerformanceAssessment = false;
            try
            {
                var assessments = await SqliteConn.Table<Assessment>().Where(a => a.CourseId == Course.Id).ToListAsync();

                lock (assessmentsLock)
                {
                    foreach (var assessment in assessments)
                    {
                        if (assessment.Type == AssessmentType.Objective)
                        {
                            ObjectiveAssessment = assessment;
                            HasObjectiveAssessment = true;
                        }
                        else
                        {
                            PerformanceAssessment = assessment;
                            HasObjectiveAssessment = true;
                        }
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
    }
}
