using Xamarin.Forms;
using course_tracker.Views;
using SQLite;
using course_tracker.Models;
using System;
using coursetracker.SQL;

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
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private void CreateDummyData()
        {
            var termCount = sqlConn.Table<Term>().CountAsync().Result;
            var today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

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
                    Start = today.AddDays(10),
                    End = today.AddDays(10).AddMonths(1),
                    Status = "Plan to Take",
                    InstructorName = "James Bond",
                    InstructorPhone = "(555) 867-5309",
                    InstructorEmail = "james.bond@email.com",
                    NotificationEnabled = true,
                    Notes = "This is a dummy course"
                };

                var objectiveAssessment = new Assessment
                {
                    Title = "Dummy Objective Assessment",
                    Start = today.AddDays(15),
                    End = today.AddDays(20),
                    Type = AssessmentType.Objective,
                    NotificationEnabled = false
                };

                var performanceAssessment = new Assessment
                {
                    Title = "Dummy Performance Assessment",
                    Start = today.AddDays(21),
                    End = today.AddDays(25),
                    Type = AssessmentType.Performance,
                    NotificationEnabled = true
                };

                var termId = sqlConn.InsertAsync(term).Result;

                course.TermId = termId;
                var courseId = sqlConn.InsertAsync(course).Result;

                objectiveAssessment.CourseId = courseId;
                performanceAssessment.CourseId = courseId;
                sqlConn.InsertAsync(objectiveAssessment).Wait();
                //sqlConn.InsertAsync(performanceAssessment).Wait();
            }
        }
    }
}
