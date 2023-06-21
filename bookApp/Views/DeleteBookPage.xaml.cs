using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace bookApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DeleteBookPage : ContentPage
	{

        List<string> ToDelete { get; set; }

        BookshelfView Bookshelf { get; set; }

        public DeleteBookPage (BookshelfView parent)
		{
            InitializeComponent();
            Bookshelf = parent;
            ToDelete = new List<string>();
		}


        protected override void OnAppearing()
        {
            base.OnAppearing();
            DeleteList.ItemsSource = Bookshelf.BookItems;
        }

        void OnCheckedChanged(object sender, EventArgs e)
        {
            //GET search results
            string isbn = ((CheckBox)sender).BindingContext.ToString();

            if (((CheckBox)sender).IsChecked)
            {
                ToDelete.Add(isbn);
            }
            else
            {
                ToDelete.Remove(isbn);
            }
        }

        async void OnRemoveClicked(object sender, EventArgs e)
        {
            foreach (string item in ToDelete)
            {

                Bookshelf.BookItems.Remove(App.Controller.AllBooks.find(item));
                if (!App.Controller.TopLayerBookshelves.Bookshelves.Contains(Bookshelf.Bookshelf1))
                    App.Controller.removeBook(item);
            }

            await Navigation.PopAsync();
        }

    }
}