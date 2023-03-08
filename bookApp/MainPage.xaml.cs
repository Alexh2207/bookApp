using bookApp.Views;
using control_library.collections;
using control_library.data;
using control_library.data_retrieval;
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

        public ObservableCollection<searchs> searchedBooks { get; set; }

        private DateTime _lastDate;

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

            searchedBooks = new ObservableCollection<searchs>();
            searchList.ItemsSource = searchedBooks;
        }

        async void OnButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new BookDetailView(new Book()));
        }

        async void BookshelfSelected(object sender, System.EventArgs e)
        {
            
            await Navigation.PushAsync(new BookshelfView((Bookshelf)((ListView)sender).SelectedItem));
            collectionView.SelectedItem = null;
            
        }

        async void OnAddClicked(object sender, EventArgs e)
        {
            //App.Controller.addBook("12344567", "Max", "Newman", "Nuevo Libro", "Mistery");
            //App.Controller.addBookToBookshelf("12344567", Bookshelf1.BookshelfID);

            //Items.Add(App.Controller.AllBooks.find("12344567"));

            Navigation.PushAsync(new CreateBookshelfView(this));


        }

        async void OnTextChanged(object sender, EventArgs e)
        {
            //GET search results
            if (((SearchBar)sender).Text != string.Empty)
            {

                OpenLibraryClient client = new OpenLibraryClient();
            
                List<searchs> query = await client.GetSearch(((SearchBar)sender).Text);
           

                searchedBooks.Clear();
                foreach (searchs book in query)
                {
                    searchedBooks.Add(book);
                }
            }
            _lastDate = DateTime.Now;
        }

        async void OnDeleteClicked(object sender, EventArgs e)
        {
            //GET search results
            //await Navigation.PushAsync(new DeleteBookPage(new BookshelfView(ShelfItems[0])));
        }
    }
}
