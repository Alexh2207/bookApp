using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using control_library.collections;

namespace control_library.data
{
#pragma warning disable CS0659 // El tipo reemplaza a Object.Equals(object o), pero no reemplaza a Object.GetHashCode()
    public class Bookshelf: CollectionBooks
#pragma warning restore CS0659 // El tipo reemplaza a Object.Equals(object o), pero no reemplaza a Object.GetHashCode()
    {
        public double BookshelfID { get; set; }
        public string ShelfName { get; set; }
        public string Concept { get; set; }

        public CollectionBookshelves Shelves { get; set; }

        public Bookshelf(string name, string concept)
            :base()
        {
            byte[] asciiBytes = Encoding.ASCII.GetBytes(String.Concat(name, concept));
            int total = 0;
            Array.ForEach(asciiBytes, delegate (byte i) { total += i; });
            BookshelfID = total+DateTime.Now.Millisecond;
            ShelfName = name;
            Concept = concept;
            Shelves = new CollectionBookshelves();
        }

        public Bookshelf(string name, string concept, Book[] books1)
            : base(books1)
        {
            byte[] asciiBytes = Encoding.ASCII.GetBytes(String.Concat(name, concept));
            int total = 0;
            Array.ForEach(asciiBytes, delegate (byte i) { total += i; });
            BookshelfID = total + DateTime.Now.Millisecond;
            ShelfName = name;
            Concept = concept;
            Shelves = new CollectionBookshelves();
        }

        public Bookshelf() : base() 
        {
            BookshelfID = 0.0;
            ShelfName = "";
            Concept = "";
            Shelves = new CollectionBookshelves();
        }

        //LIMPIAR ESTO, MAL CÓDIGO

        public bool addBook(Book book)
        {
            return add(book);
        }

        public bool addShelf(Bookshelf bookshelf)
        {
            Shelves.add(bookshelf);
            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is Bookshelf bookshelf &&
                   EqualityComparer<List<Book>>.Default.Equals(Books, bookshelf.Books) &&
                   ShelfName == bookshelf.ShelfName &&
                   Concept == bookshelf.Concept &&
                   BookshelfID == bookshelf.BookshelfID &&
                   EqualityComparer<CollectionBookshelves>.Default.Equals(Shelves, bookshelf.Shelves);
        }

    }
}
