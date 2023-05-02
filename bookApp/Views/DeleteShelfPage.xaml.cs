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
	public partial class DeleteShelfPage : ContentPage
	{
        List<double> ToDelete { get; set; }

        BookshelfView Bookshelf { get; set; }

        public DeleteShelfPage(BookshelfView parent)
        {
            InitializeComponent();
            Bookshelf = parent;
            ToDelete = new List<double>();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            DeleteList.ItemsSource = Bookshelf.ShelfItems;
        }

        void OnCheckedChanged(object sender, EventArgs e)
        {
            //GET search results
            double isbn = double.Parse(((CheckBox)sender).BindingContext.ToString());

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
            foreach (double item in ToDelete)
            {

                Bookshelf.ShelfItems.Remove(App.Controller.AllBookshelves.find(item));
                App.Controller.removeBookshelf(item);
            }

            await Navigation.PopAsync();
        }
    }
}