using control_library.collections;
using control_library.data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace bookApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddBookPage : ContentPage
	{

        List<string> ToAdd { get; set; }

        BookshelfView Bookshelf { get; set; }

        public AddBookPage (BookshelfView parent)
		{
            InitializeComponent();
            Bookshelf = parent;
            ToAdd = new List<string>();
		}


        protected override void OnAppearing()
        {
            base.OnAppearing();
            AddList.ItemsSource = App.Controller.AllBooks.Books;
        }

        void OnCheckedChanged(object sender, EventArgs e)
        {
            //GET search results
            string isbn = ((CheckBox)sender).BindingContext.ToString();

            if (((CheckBox)sender).IsChecked)
            {
                ToAdd.Add(isbn);
            }
            else
            {
                ToAdd.Remove(isbn);
            }
        }

        async void OnAddClicked(object sender, EventArgs e)
        {
            foreach (string item in ToAdd)
            {

                Bookshelf.BookItems.Add(App.Controller.AllBooks.find(item));
                App.Controller.addBookToBookshelf(item, Bookshelf.Bookshelf1.BookshelfID);

            }

            await Navigation.PopAsync();
        }

    }
}