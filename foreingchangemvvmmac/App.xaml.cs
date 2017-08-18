

namespace foreingchangemvvmmac
{
    using Xamarin.Forms;
    using Views;

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainView();
        }

        protected override void OnStart() //Cuenado la aplicación cambia
        {
            // Handle when your app starts
        }

        protected override void OnSleep() // Cuando la aplicación pierde foco
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
