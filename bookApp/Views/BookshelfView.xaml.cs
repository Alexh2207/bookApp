using control_library.collections;
using control_library.data;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace bookApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookshelfView : ContentPage
    {
        public ObservableCollection<Book> BookItems { get; set; }
        public ObservableCollection<Bookshelf> ShelfItems { get; set; }

        public Bookshelf Bookshelf1 { get; set; }

        public BookshelfView(Bookshelf bookshelf)
        {
            InitializeComponent();
            Bookshelf1 = bookshelf;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Bookshelf Shelf = App.Controller.AllBookshelves.find(Bookshelf1.BookshelfID);
            BookItems = new ObservableCollection<Book>(Shelf.Books);
            BookList.ItemsSource =  BookItems;
            ShelfItems = new ObservableCollection<Bookshelf>(App.Controller.AllBookshelves.find(Bookshelf1.BookshelfID).Shelves.Bookshelves);
            BookshelvesList.ItemsSource = ShelfItems;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await Navigation.PushAsync(new BookDetailView((Book)((ListView)sender).SelectedItem));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
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

        async void Handle_ShelfTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await Navigation.PushAsync(new BookshelfView((Bookshelf)((ListView)sender).SelectedItem));
            ((ListView)sender).SelectedItem = null;
        }

        void OnDeleteBookClicked(object sender, EventArgs e)
        {

            Navigation.PushAsync(new DeleteBookPage(this));

        }

        void OnDeleteShelfClicked(object sender, EventArgs e)
        {

            Navigation.PushAsync(new DeleteShelfPage(this));

        }
    }
}
