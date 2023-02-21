using bookApp.Views;
using control_library.collections;
using control_library.data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace bookApp
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Bookshelf> ShelfItems { get; set; }

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            CollectionBookshelves TShelves = await App.Controller.GetTShelfAsync();
            ShelfItems = new ObservableCollection<Bookshelf>(TShelves.Bookshelves);
            collectionView.ItemsSource = ShelfItems;
            
        }

        int count = 0;
        async void OnButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new BookDetailView(new Book()));
        }

        async void BookshelfSelected(object sender, System.EventArgs e)
        {
            
            await Navigation.PushAsync(new BookshelfView((Bookshelf)((ListView)sender).SelectedItem));
            collectionView.SelectedItem = null;
            
        }

        void OnAddClicked(object sender, EventArgs e)
        {
            //App.Controller.addBook("12344567", "Max", "Newman", "Nuevo Libro", "Mistery");
            //App.Controller.addBookToBookshelf("12344567", Bookshelf1.BookshelfID);

            //Items.Add(App.Controller.AllBooks.find("12344567"));

            Navigation.PushAsync(new CreateBookshelfView(this));

            /*            MyListView.ItemsSource = null;  // This works, but it's DIRTY
                        MyListView.ItemsSource = App.Controller.AllBookshelves.find(Bookshelf1.BookshelfID).Books;*/



        }

        async void OnTextChanged(object sender, EventArgs e)
        {
            //GET search results
        }

        async void OnDeleteClicked(object sender, EventArgs e)
        {
            //GET search results
            //await Navigation.PushAsync(new DeleteBookPage(new BookshelfView(ShelfItems[0])));
        }
    }
}
