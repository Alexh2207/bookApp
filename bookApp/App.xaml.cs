using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using control_library;

namespace bookApp
{
    public partial class App : Application
    {

        static DataController controller;

        public static DataController Controller
        {
            get
            {
                if (controller == null)
                {
                    controller = new DataController();
                }
                return controller;
            }
        }

        public App()
        {
            InitializeComponent();

            controller = new DataController();

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
