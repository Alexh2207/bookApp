using control_library.collections;
using System;
using System.Collections.Generic;

namespace control_library.data
{
#pragma warning disable CS0659 // El tipo reemplaza a Object.Equals(object o), pero no reemplaza a Object.GetHashCode()
    internal class Book
#pragma warning restore CS0659 // El tipo reemplaza a Object.Equals(object o), pero no reemplaza a Object.GetHashCode()
    {
        public string Isbn { get; set; }
        public Author Author { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public bool Read { get; set; }
        public bool Wishlist { get; set; }
        public double Valoration { get; set; }
        public string Description { get; set; }
        public CollectionBooks RelevantBooks { get; set; }
        public List<double> Shelves { get; set; }
        public DateTime EndDate { get; set; }

        public Book()
        {
            Isbn = "";
            Author = new Author();
            Title = "";
            Genre = "";
            Read = false;
            Wishlist = false;
            Description = "";
            Shelves = new List<double>();
            RelevantBooks = new CollectionBooks();
            EndDate = new DateTime();
        }

        public Book(string isbn, Author author, string title)
        {
            Isbn = isbn;
            this.Author = author;
            Title = title;
            Genre = "";
            Read = false;
            Wishlist = false;
            Description = "";
            Shelves = new List<double>();
            RelevantBooks = new CollectionBooks();
            EndDate = new DateTime();
        }

        public Book(string isbn, Author author, string title, string genre)
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
                   EqualityComparer<Author>.Default.Equals(Author, book.Author) &&
                   Title == book.Title &&
                   Genre == book.Genre;
        }

    }
}