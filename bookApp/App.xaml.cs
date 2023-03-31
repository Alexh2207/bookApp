using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using control_library;
using System.Text.Json;
using System.IO;
using System.Diagnostics;
using System.Text;

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
                    int i = 0;
                    controller = new DataController();
                    
                }
                return controller;
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage( new MainPage());
        }

        protected override void OnStart()
        {
            controller = JsonSerializer.Deserialize<DataController>(File.ReadAllText(Path.Combine("/storage/self/primary/Documents", "data.json")));
        }

        protected override void OnSleep()
        {
            Debug.WriteLine("---------------SLEEPING--------------");

            var dataStream = File.OpenWrite(Path.Combine("/storage/self/primary/Documents", "data.json"));

            string info = App.Controller.serializeAll();

            Console.WriteLine(info);

            var bytes = Encoding.UTF8.GetBytes(info);

            Console.WriteLine(bytes.Length);

            dataStream.Write(bytes, 0, bytes.Length);

            dataStream.Close();
        }

        protected override void OnResume()
        {
        }
    }
}
