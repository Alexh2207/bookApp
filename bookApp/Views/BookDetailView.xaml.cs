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
    public partial class BookDetailView : ContentPage
    {
        
        public BookDetailView(Book book)
        {
            InitializeComponent();
            Title1.Text = book.Title;
            Author.Text = book.Author.ToString();
        }
    }
}