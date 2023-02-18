using control_library.data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace bookApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateBookView : ContentPage
    {

        BookshelfView bookshelf;

        public CreateBookView(BookshelfView parent)
        {
            InitializeComponent();
            bookshelf = parent;
        }

        public void OnAddNewBook(object sender, EventArgs args)
        {
            App.Controller.addBook(ISBN.Text,AuthorName.Text,AuthorSurname.Text, BookTitle.Text,Genre.Text);
            App.Controller.addBookToBookshelf(ISBN.Text, bookshelf.Bookshelf1.BookshelfID);
            bookshelf.BookItems.Add(App.Controller.AllBooks.find(ISBN.Text));
        }
    }
}