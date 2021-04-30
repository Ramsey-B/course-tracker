using Xamarin.Forms;
using course_tracker.Views;
using course_tracker.Services;
using SQLite;
using System.Xml.Serialization;
using System.IO;
using course_tracker.Models;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace course_tracker
{
    public partial class App : Application
    {
        private readonly SQLiteAsyncConnection sqlConn;
        public App()
        {
            InitializeComponent();
            sqlConn = GetSQLConnection();
            DependencyService.Register<SQLiteAsyncConnection>();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            sqlConn.CreateTableAsync<Term>().Wait();
            sqlConn.CreateTableAsync<Course>().Wait();
            sqlConn.CreateTableAsync<Assessment>().Wait();
            CreateDummyData().Wait();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private SQLiteAsyncConnection GetSQLConnection()
        {
            var docPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(docPath, "CourseTracker.db3");
            return new SQLiteAsyncConnection(path);
        }

        private async Task CreateDummyData()
        {
            var terms = await sqlConn.Table<Term>().ToListAsync();

            if (!terms.Any())
            {
                var term = new Term
                {
                    Title = "Dummy Term",
                    Start = DateTime.UtcNow.AddDays(-1),
                    End = DateTime.UtcNow.AddMonths(6),
                };

                var course = new Course
                {
                    Title = "Dummy Course",
                    Start = DateTime.UtcNow.AddDays(10),
                    End = DateTime.UtcNow.AddDays(10).AddMonths(1),
                    Status = CourseStatus.PlanToTake,
                    InstructorName = "James Bond",
                    InstructorPhone = "(555) 867-5309",
                    InstructorEmail = "james.bond@email.com",
                    NotificationEnabled = true,
                    Notes = "This is a dummy course"
                };

                var objectiveAssessment = new Assessment
                {
                    Title = "Dummy Objective Assessment",
                    Start = DateTime.UtcNow.AddDays(15),
                    End = DateTime.UtcNow.AddDays(20),
                    Type = AssessmentType.Objective,
                    NotificationEnabled = false
                };

                var performanceAssessment = new Assessment
                {
                    Title = "Dummy Objective Assessment",
                    Start = DateTime.UtcNow.AddDays(21),
                    End = DateTime.UtcNow.AddDays(25),
                    Type = AssessmentType.Objective,
                    NotificationEnabled = true
                };

                var termId = await sqlConn.InsertAsync(term);

                course.TermId = termId;
                var courseId = await sqlConn.InsertAsync(course);

                objectiveAssessment.CourseId = courseId;
                performanceAssessment.CourseId = courseId;
                await sqlConn.InsertAsync(objectiveAssessment);
                await sqlConn.InsertAsync(performanceAssessment);
            }
        }
    }
}
