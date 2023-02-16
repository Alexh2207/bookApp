using bookApp.Views;
using control_library.collections;
using control_library.data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace bookApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            CollectionBookshelves TShelves = await App.Controller.GetTShelfAsync();
            collectionView.ItemsSource = TShelves.Bookshelves;
        }

        int count = 0;
        void OnButtonClicked(object sender, System.EventArgs e)
        {
            count++;
            ((Button)sender).Text = $"You clicked {count} times.";
        }

        async void BookshelfSelected(object sender, System.EventArgs e)
        {
            bool boolean = await DisplayAlert(((Bookshelf)((ListView)sender).SelectedItem).BookshelfID.ToString(), ((Bookshelf)((ListView)sender).SelectedItem).ShelfName, "Yes", "No");
            if(boolean)
            {
                await Navigation.PushAsync(new BookshelfView((Bookshelf)((ListView)sender).SelectedItem));
            }
        }
    }
}
