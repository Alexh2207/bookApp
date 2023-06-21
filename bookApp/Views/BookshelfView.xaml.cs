using control_library.collections;
using control_library.data;
using System;
using System.Collections.Generic;
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
        public List<double> ShelfIDs { get; set; }
        public ObservableCollection<Bookshelf> ShelfItems { get; set; }

        public Bookshelf Bookshelf1 { get; set; }

        public BookshelfView(Bookshelf bookshelf)
        {
            InitializeComponent();
            Bookshelf1 = bookshelf;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Bookshelf Shelf = App.Controller.AllBookshelves.find(Bookshelf1.BookshelfID);
            BookItems = new ObservableCollection<Book>(Shelf.Books);
            BookList.ItemsSource =  BookItems;
            ShelfIDs = new List<double>(App.Controller.AllBookshelves.find(Bookshelf1.BookshelfID).Shelves);
            ShelfItems = new ObservableCollection<Bookshelf>(App.Controller.GetBookshelves(ShelfIDs));
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

            Navigation.PushAsync(new CreateBookshelfView(this));

        }

        async void Handle_ShelfTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            //Navegate to the subshelf pressed

            await Navigation.PushAsync(new BookshelfView((Bookshelf)((ListView)sender).SelectedItem));
            ((ListView)sender).SelectedItem = null;
        }

        void OnDeleteBookClicked(object sender, EventArgs e)
        {

            Navigation.PushAsync(new DeleteBookPage(this));

        }

        void OnAddBookClicked(object sender, EventArgs e)
        {

            Navigation.PushAsync(new AddBookPage(this));

        }

        void OnDeleteShelfClicked(object sender, EventArgs e)
        {

            Navigation.PushAsync(new DeleteShelfPage(this));

        }
    }
}
