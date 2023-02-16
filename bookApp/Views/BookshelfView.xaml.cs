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
        public ObservableCollection<string> Items { get; set; }

        public BookshelfView(Bookshelf bookshelf)
        {
            InitializeComponent();
            MyListView.ItemsSource = bookshelf.Books;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await Navigation.PushAsync(new BookDetailView((Book)((ListView)sender).SelectedItem));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
