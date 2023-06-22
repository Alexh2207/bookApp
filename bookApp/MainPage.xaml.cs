using bookApp.Views;
using control_library.collections;
using control_library.data;
using control_library.data_retrieval;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace bookApp
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Bookshelf> ShelfItems { get; set; }

        public ObservableCollection<searchs> searchedBooks { get; set; }

        private string PhotoPath;

        public MainPage()
        {
            InitializeComponent();

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //Get the top shelves asynchronically

            CollectionBookshelves TShelves = await App.Controller.GetTShelfAsync();

            //Add the shelves to the collection in the list

            ShelfItems = new ObservableCollection<Bookshelf>(TShelves.Bookshelves);
            collectionView.ItemsSource = ShelfItems;

            //Search bar initialization

            sBar.Text = string.Empty;
            searchedBooks = new ObservableCollection<searchs>();
            searchList.ItemsSource = searchedBooks;
        }
        /*
        async void OnButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new BookDetailView(new Book()));
        }
        */

        /// <summary>
        /// When a Bookshlef is selected, this event will navigate to the Bookshelf Detail page
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        async void BookshelfSelected(object sender, System.EventArgs e)
        {
            
            await Navigation.PushAsync(new BookshelfView((Bookshelf)((ListView)sender).SelectedItem));
            collectionView.SelectedItem = null;
            
        }

        /// <summary>
        /// When clicked on add bookshelf, redirect to the add bookshelf page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void OnAddClicked(object sender, EventArgs e)
        {
            
            //Bookshelf creation

            await Navigation.PushAsync(new CreateBookshelfView(this));

        }

        /// <summary>
        /// When pressed on scan photo, go to camera and take photo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void OnPhotoClicked(object sender, EventArgs e)
        {

            await TakePhotoAsync();

            Console.WriteLine(PhotoPath);

        }

        async void OnRequestClicked(object sender, EventArgs e)
        {

            HttpClient serverReq = new HttpClient();

            await serverReq.GetAsync("http://192.168.1.200:8080/process");

        }

        /// <summary>
        /// Search for what is written in the search bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void OnTextChanged(object sender, EventArgs e)
        {   
            //GET search results

            if (((SearchBar)sender).Text != string.Empty)
            {

                //Use the API to search the book

                OpenLibraryClient client = new OpenLibraryClient();
            
                List<searchs> query = await client.GetSearch(((SearchBar)sender).Text);
                

                searchedBooks.Clear();

                foreach (searchs book in query)
                {
                    searchedBooks.Add(book);
                }
            }
        }

        /// <summary>
        /// Display the details of the pressed book
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void OnSearchBookTapped(object sender, EventArgs e)
        {
            //GET search results
            await Navigation.PushAsync(new BookDetailView((searchs)((ListView)sender).SelectedItem));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }


        private async Task TakePhotoAsync()
        {
            try
            {
                //Take the photo and save it (in the future will be sent to the server)

                var photo = await MediaPicker.CapturePhotoAsync();
                await LoadPhotoAsync(photo);
                Console.WriteLine($"CapturePhotoAsync COMPLETED: {PhotoPath}");
            
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature is not supported on the device
                Debug.WriteLine(fnsEx.Message );
            }
            catch (PermissionException pEx)
            {
                // Permissions not granted
                Debug.WriteLine(pEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }

        private async Task LoadPhotoAsync(FileResult photo)
        {
            // canceled
            if (photo == null)
            {
                PhotoPath = null;
                return;
            }
            // save the file into local storage
            var newFile = Path.Combine("/storage/self/primary/DCIM", photo.FileName);   
            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);

            byte[] imageBytes;
            var stream2 = await photo.OpenReadAsync();
            MemoryStream ms = new MemoryStream();
            stream2.CopyTo(ms);
            imageBytes = ms.ToArray();
            Console.WriteLine(ms.ToString());

            HttpClient httpClient = new HttpClient();

            var ufile = new ByteArrayContent(imageBytes);

            ufile.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpg");

            var multipartFormContent = new MultipartFormDataContent();

            multipartFormContent.Add(ufile, name: "ufile", fileName:"ufile.jpg");

            var response = await httpClient.PostAsync("http://192.168.1.200:8080/upload", multipartFormContent);

            Console.WriteLine(await response.Content.ReadAsStringAsync());

            PhotoPath = newFile;
        }
    }
}
