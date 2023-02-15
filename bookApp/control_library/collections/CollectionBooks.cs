using control_library.data;
using System;
using System.Collections.Generic;

namespace control_library.collections
{
    public class CollectionBooks
    {
        public List<Book> Books { get; set; }

        public CollectionBooks()
        {
            Books = new List<Book>();
        }

        public CollectionBooks(List<Book> books)
        {
            Books = new List<Book>(books);
        }

        public CollectionBooks(Book[] books)
        {
            Books = new List<Book>(books);
        }

        public bool add(Book book)
        {
            if (Books.Contains(book)) { return false; }
            Books.Add(book);
            return true;
        }

        public bool remove(string isbn)
        {
            foreach (Book element in Books)
            {
                if (element.Isbn.Equals(isbn))
                {
                    return Books.Remove(element);
                }
            }
            return false;
        }

        public Book find(string isbn)
        {
            foreach (Book element in Books)
            {
                if (element.Isbn.Equals(isbn))
                {
                    return element;
                }
            }
            throw new BookNotFoundException(isbn);
        }

        
    }

    public class BookNotFoundException : Exception
    {
        public BookNotFoundException() { }

        public BookNotFoundException(string isbn)
            : base(String.Format("Book not Found: {0}", isbn))
        {

        }
    }
}