using Xamarin.Forms;
using course_tracker.Views;
using SQLite;
using course_tracker.Models;
using System;
using course_tracker.SQL;
using Plugin.LocalNotifications;

namespace course_tracker
{
    public partial class App : Application
    {
        private readonly SQLiteAsyncConnection sqlConn;
        public App()
        {
            InitializeComponent();
            sqlConn = new SQLConnection().GetAsyncConnection();
            DependencyService.Register<SQLConnection>();
            MainPage = new NavigationPage(new TermsPage());
        }

        protected override void OnStart()
        {
            sqlConn.DropTableAsync<Term>().Wait();
            sqlConn.DropTableAsync<Course>().Wait();
            sqlConn.DropTableAsync<Assessment>().Wait();
            sqlConn.CreateTableAsync<Term>().Wait();
            sqlConn.CreateTableAsync<Course>().Wait();
            sqlConn.CreateTableAsync<Assessment>().Wait();
            CreateDummyData();

            SetNotifications();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private void SetNotifications()
        {
            var courses = sqlConn.Table<Course>().Where(c => c.NotificationsEnabled).ToListAsync().Result;
            var assessments = sqlConn.Table<Assessment>().Where(a => a.NotificationsEnabled).ToListAsync().Result;

            foreach (var course in courses)
            {
                if (course.Start == DateTime.Today)
                {
                    CrossLocalNotifications.Current.Show("Reminder", $"Course '{course.Title}' begins today!", course.Id);
                }
                if (course.End == DateTime.Today)
                {
                    CrossLocalNotifications.Current.Show("Reminder", $"Course '{course.Title}' ends today!", course.Id);
                }
            }

            foreach (var assessment in assessments)
            {
                if (assessment.Start == DateTime.Today)
                {
                    CrossLocalNotifications.Current.Show("Reminder", $"Assessment '{assessment.Title}' begins today!", assessment.Id);
                }
                if (assessment.End == DateTime.Today)
                {
                    CrossLocalNotifications.Current.Show("Reminder", $"Assessment '{assessment.Title}' ends today!", assessment.Id);
                }
            }
        }

        private void CreateDummyData()
        {
            var termCount = sqlConn.Table<Term>().CountAsync().Result;
            var today = DateTime.Today;

            if (termCount == 0)
            {
                var term = new Term
                {
                    Title = "Dummy Term",
                    Start = today.AddDays(-1),
                    End = today.AddMonths(6),
                };

                var course = new Course
                {
                    Title = "Dummy Course",
                    Start = today,
                    End = today.AddDays(10).AddMonths(1),
                    Status = "Plan to Take",
                    InstructorName = "Ramsey Bland",
                    InstructorPhone = "(208) 488-1434",
                    InstructorEmail = "rbland4@wgu.edu",
                    NotificationsEnabled = true,
                    Notes = "This is a dummy course"
                };

                var objectiveAssessment = new Assessment
                {
                    Title = "Dummy Objective Assessment",
                    Start = today,
                    End = today.AddDays(20),
                    Type = AssessmentType.Objective,
                    NotificationsEnabled = false
                };

                var performanceAssessment = new Assessment
                {
                    Title = "Dummy Performance Assessment",
                    Start = today.AddDays(21),
                    End = today.AddDays(25),
                    Type = AssessmentType.Performance,
                    NotificationsEnabled = true
                };

                var termId = sqlConn.InsertAsync(term).Result;

                course.TermId = termId;
                var courseId = sqlConn.InsertAsync(course).Result;

                objectiveAssessment.CourseId = courseId;
                performanceAssessment.CourseId = courseId;
                sqlConn.InsertAsync(objectiveAssessment).Wait();
                sqlConn.InsertAsync(performanceAssessment).Wait();
            }
        }
    }
}
