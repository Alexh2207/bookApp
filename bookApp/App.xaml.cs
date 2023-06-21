using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using control_library;
using System.Text.Json;
using System.IO;
using System.Diagnostics;
using System.Text;
using Xamarin.Essentials;

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

            MainPage = new NavigationPage( new MainPage());
        }

        protected override void OnStart()
        {
            try
            {
                controller = JsonSerializer.Deserialize<DataController>(File.ReadAllText(Path.Combine(FileSystem.AppDataDirectory, "data.json")));
                controller.AllBooks = controller.AllBookshelves.find(controller.AllBooks.BookshelfID);
                controller.Wishlist = controller.AllBookshelves.find(controller.Wishlist.BookshelfID);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                controller = new DataController(1);
            }
        }

        protected override void OnSleep()
        {
            Debug.WriteLine("---------------SLEEPING--------------");

            var dataStream = File.OpenWrite(Path.Combine(FileSystem.AppDataDirectory, "data.json"));

            dataStream.SetLength(0);

            dataStream.Flush();

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
