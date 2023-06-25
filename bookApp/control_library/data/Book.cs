using control_library.collections;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace control_library.data
{
#pragma warning disable CS0659 // El tipo reemplaza a Object.Equals(object o), pero no reemplaza a Object.GetHashCode()
    public class Book
#pragma warning restore CS0659 // El tipo reemplaza a Object.Equals(object o), pero no reemplaza a Object.GetHashCode()
    {
        /// <summary>
        /// ISBN of book. unique identifier
        /// </summary>
        public string Isbn { get; set; }

        public string OLID { get; set; }

        /// <summary>
        /// Collection of Authors of the book
        /// </summary>
        public CollectionAuthors Author { get; set; }
        public string Title { get; set; }
        public List<string> Genre { get; set; }
        public bool Read { get; set; }
        public bool Wishlist { get; set; }
        public double Valoration { get; set; }
        public string Description { get; set; }
        public string cover_url { get; set; }
        public CollectionBooks RelevantBooks { get; set; }
        public List<double> Shelves { get; set; }
        public DateTime EndDate { get; set; }

        public Book()
        {
            Isbn = "";
            Author = new CollectionAuthors();
            Title = "";
            Genre = new List<string>();
            Read = false;
            Wishlist = false;
            Description = "";
            Shelves = new List<double>();
            RelevantBooks = new CollectionBooks();
            EndDate = new DateTime();
            cover_url= "Bookdefault.jpg";
            OLID = "";
        }

        public Book(string isbn, CollectionAuthors author, string title)
        {
            Isbn = isbn;
            this.Author = author;
            Title = title;
            Genre = new List<string>();
            Read = false;
            Wishlist = false;
            Description = "";
            Shelves = new List<double>();
            RelevantBooks = new CollectionBooks();
            EndDate = new DateTime();
            cover_url = "Bookdefault.jpg";
            OLID = "";
        }

        public Book(string isbn, CollectionAuthors author, string title, List<string> genre)
        {
            Isbn = isbn;
            this.Author = author;
            Title = title;
            Genre = genre;
            Read = false;
            Wishlist = false;
            Description = "";
            Shelves = new List<double>();
            RelevantBooks = new CollectionBooks();
            EndDate = new DateTime();
            cover_url = "Bookdefault.jpg";
            OLID = "";
        }

        public Book(string isbn, CollectionAuthors author, string title, List<string> genre, string cover)
        {
            Isbn = isbn;
            this.Author = author;
            Title = title;
            Genre = genre;
            Read = false;
            Wishlist = false;
            Description = "";
            Shelves = new List<double>();
            RelevantBooks = new CollectionBooks();
            EndDate = new DateTime();
            cover_url = cover;
            OLID = "";
        }

        public bool addToShelf(double shelfID)
        {
            if (!Shelves.Contains(shelfID))
            {
                Shelves.Add(shelfID);
                return true;
            }else { return false; }
        }

        public bool finish(DateTime end)
        {
            if (Read || end.Equals(null)) { return false; }
            Read = true;
            EndDate = end;
            return true;
        }

        public bool addToWishlist()
        {
            if (Wishlist) { return false; }
            Wishlist = true;
            return true;
        }

        public bool addValoration(double points, string description)
        {
            Valoration = points;
            Description = description;
            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is Book book &&
                   Isbn == book.Isbn &&
                   EqualityComparer<CollectionAuthors>.Default.Equals(Author, book.Author) &&
                   Title == book.Title &&
                   Genre == book.Genre;
        }

        public string serializeBook()
        {

            return JsonSerializer.Serialize(this);

        }

    }
}