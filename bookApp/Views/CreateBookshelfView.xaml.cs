using control_library.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace bookApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateBookshelfView : ContentPage
    {
        BookshelfView bookshelf;

        MainPage mainPage;

        public CreateBookshelfView(BookshelfView parent)
        {
            InitializeComponent();
            bookshelf = parent;
        }

        public CreateBookshelfView(MainPage parent)
        {
            InitializeComponent();
            mainPage = parent;
        }

        public void OnAddNewBookShelf(object sender, EventArgs args)
        {
            double id;
            if(bookshelf == null)
            {
                id = App.Controller.createTopBookshelf(Name.Text,Concept.Text);
                mainPage.ShelfItems.Add(App.Controller.AllBookshelves.find(id));
            }
            else
            {
                id = App.Controller.createSubBookshelf(Name.Text, Concept.Text, bookshelf.Bookshelf1.BookshelfID);
                bookshelf.ShelfItems.Add(App.Controller.AllBookshelves.find(id));
            }

            
        }
    }
}