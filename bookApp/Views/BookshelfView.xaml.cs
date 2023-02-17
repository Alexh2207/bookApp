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
        public ObservableCollection<Book> Items { get; set; }

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
            Items = new ObservableCollection<Book>(Shelf.Books);
            MyListView.ItemsSource =  Items;

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

            Navigation.PushAsync(new CreateBookView(this));

            /*            MyListView.ItemsSource = null;  // This works, but it's DIRTY
                        MyListView.ItemsSource = App.Controller.AllBookshelves.find(Bookshelf1.BookshelfID).Books;*/



        }
    }
}
