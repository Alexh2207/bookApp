using control_library.data;
using control_library.data_retrieval;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace bookApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookDetailView : ContentPage
    {

        Xamarin.Forms.Button addBook;

        Book ThisBook { get; set; }

        searchs book { get; set; }

        bool local_book;

        searched_book editionCollect { get; set; }

        ObservableCollection <edition_transform> Editions { get; set; }
        ObservableCollection<simple_book> recommended { get; set; }

        /// <summary>
        /// When arriving from a book already stored in the app you are not able to modify it
        /// </summary>
        /// <param name="book"></param>
        public BookDetailView(Book book)
        {
            InitializeComponent();
            ThisBook = book;
            Title1.Text = book.Title;
            Author.Text = book.Author.ToString();
            cover.Source = book.cover_url;
            Editions = new ObservableCollection<edition_transform>();
            local_book = true;
            subjectList.ItemsSource = book.Genre;
        }

        /// <summary>
        /// When coming from a searched book you can choose the edition
        /// </summary>
        /// <param name="book"></param>
        public BookDetailView(searchs book)
        {
            InitializeComponent();
            Title1.Text = book.title;
            Author.Text = book.author_name.ToString();
            cover.Source = book.cover_edition_key;
            addBook = new Xamarin.Forms.Button();
            addBook.Text = "Add";
            addBook.Clicked += new EventHandler(OnAddClicked);
            grid.Children.Add(addBook,1,2,3,4);
            this.book = book;
            Editions = new ObservableCollection<edition_transform>();
            local_book = false;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //If not a local book, search for all the editions and display them

            if (!local_book)
            {
                OpenLibraryClient openLibraryClient = new OpenLibraryClient();

                editionCollect = await openLibraryClient.GetWorkData(book);

                string title, isbn = string.Empty;

                List<int> covers = new List<int>();

                foreach (edition_book element in editionCollect.edition_key)
                {

                    if (element.isbn_13 != null && element.isbn_13.ElementAt(0) != null)
                    {
                        if (element.title == null)
                        {
                            title = book.title;
                        }
                        else
                        {
                            title = element.title;
                        }

                        if (element.covers == null)
                        {
                            covers.Add(0);
                        }
                        else
                        {
                            covers = element.covers;
                        }

                        Editions.Add(new edition_transform(title, element.number_of_pages, element.isbn_13.ElementAt(0), "https://covers.openlibrary.org/b/id/" + covers.ElementAt(0) + "-M.jpg", book.author_name));
                    }else if(element.isbn_10 != null && element.isbn_10.ElementAt(0) != null)
                    {
                        if (element.title == null)
                        {
                            title = book.title;
                        }
                        else
                        {
                            title = element.title;
                        }

                        if (element.covers == null)
                        {
                            covers.Add(0);
                        }
                        else
                        {
                            covers = element.covers;
                        }

                        Editions.Add(new edition_transform(title, element.number_of_pages, element.isbn_10.ElementAt(0), "https://covers.openlibrary.org/b/id/" + covers.ElementAt(0) + "-M.jpg", book.author_name));
                    }

                }

                ThisBook = new Book(Editions.ElementAt(0).isbn_13,book.author_name, book.title, editionCollect.subjects, book.cover_edition_key);

                editionList.ItemsSource = Editions;

                recommended = new ObservableCollection<simple_book>((await GetRecommend()).Simple_Books);
                recommendList.ItemsSource = recommended;
            }
            else
            {
                recommended = new ObservableCollection<simple_book>((await GetRecommend()).Simple_Books);
                recommendList.ItemsSource = recommended;
            }
        }

        /// <summary>
        /// Add the default edition of the book to the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void OnAddClicked(object sender, EventArgs e)
        {

            OpenLibraryClient openLibraryClient = new OpenLibraryClient();

            searched_book editions = await openLibraryClient.GetWorkData(book);

            App.Controller.addBook(ThisBook.Isbn,ThisBook.Author,ThisBook.Title, editions.subjects, ThisBook.cover_url);
            App.Controller.addBookToBookshelf(ThisBook.Isbn, App.Controller.AllBooks.BookshelfID);

            Console.WriteLine("Book");

            await Navigation.PopAsync();

        }

        async void OnValueChanged(object sender, EventArgs e)
        {
            App.Controller.addValoration(ThisBook.Isbn,((Slider)sender).Value,"");
            val.Text = ((Slider)sender).Value.ToString();
        }

        /// <summary>
        /// Add a concrete edition to the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void OnEditionTapped(object sender, EventArgs e)
        {
            //ADD BOOKSHELF SELECTION

            edition_transform edition = (edition_transform)((Xamarin.Forms.ListView)sender).SelectedItem;

            App.Controller.addBook(edition.isbn_13, book.author_name, edition.title, editionCollect.subjects, edition.covers.Replace("-M","-L"));
            App.Controller.addBookToBookshelf(edition.isbn_13, App.Controller.AllBooks.BookshelfID);

            ((Xamarin.Forms.ListView)sender).SelectedItem = null;

            Console.WriteLine("Book");

            await Navigation.PopAsync();
        }

        async void OnRecommendTapped(object sender, EventArgs e)
        {
            OpenLibraryClient client = new OpenLibraryClient();

            simple_book selectedBook = (simple_book)((Xamarin.Forms.ListView)sender).SelectedItem;

            searchs work = (await client.GetSearch(selectedBook.ID)).ElementAt(0);

            await Navigation.PushAsync(new BookDetailView(work));
        }

        async Task<recommendations> GetRecommend()
        {
            recommendations recomList = new recommendations(new List<simple_book>());
            await Task.Run(async () =>
            {
                HttpClient client = new HttpClient();
                string url = "http://192.168.1.200:8080/recommend?isbn=" + ThisBook.Isbn;
                Stream resp = await client.GetStreamAsync(url);
                var recommends = JsonSerializer.DeserializeAsync<recommendations>(resp);
                Console.WriteLine((new StreamReader(resp)).ReadToEnd());
                recomList = recommends.Result;
                Console.WriteLine(recomList.Simple_Books.ElementAt(0).ID);
            });
            return recomList;
        }

        public record class recommendations(List<simple_book> Simple_Books);

        public record class simple_book(string ID, string Title, string coverl_url);

    }
}