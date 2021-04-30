using Xamarin.Forms;
using course_tracker.Views;
using course_tracker.Services;

namespace course_tracker
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            DependencyService.Register<TermRepository>();
            DependencyService.Register<TermService>();
            DependencyService.Register<CourseService>();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
