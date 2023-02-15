using control_library.data;
using System;
using System.Collections.Generic;

namespace control_library.collections
{
#pragma warning disable CS0659 // El tipo reemplaza a Object.Equals(object o), pero no reemplaza a Object.GetHashCode()
    public class CollectionBookshelves
#pragma warning restore CS0659 // El tipo reemplaza a Object.Equals(object o), pero no reemplaza a Object.GetHashCode()
    {
        public List<Bookshelf> Bookshelves { get; set; }
        public CollectionBookshelves()
        {
            Bookshelves = new List<Bookshelf>();
        }

        public CollectionBookshelves(List<Bookshelf> bookshelves)
        {
            Bookshelves = bookshelves;
        }

        public bool add(Bookshelf Bookshelf)
        {
            if (Bookshelves.Contains(Bookshelf)) { return false; }
            Bookshelves.Add(Bookshelf);
            return true;
        }

        public bool remove(double BookshelfID)
        {
            foreach (Bookshelf element in Bookshelves)
            {
                if (element.BookshelfID.Equals(BookshelfID))
                {
                    return Bookshelves.Remove(element);
                }
            }
            return false;
        }

        public Bookshelf find(double BookshelfID)
        {
            foreach (Bookshelf element in Bookshelves)
            {
                if (element.BookshelfID.Equals(BookshelfID))
                {
                    return element;
                }
            }
            throw new BookshelfNotFoundException(BookshelfID);
        }

        public override bool Equals(object obj)
        {
            return obj is CollectionBookshelves bookshelves &&
                   EqualityComparer<List<Bookshelf>>.Default.Equals(Bookshelves, bookshelves.Bookshelves);
        }
    }

    internal class BookshelfNotFoundException : Exception
    {
        public BookshelfNotFoundException() { }

        public BookshelfNotFoundException(double BookshelfID)
            : base(String.Format("Bookshelf not Found: {0}", BookshelfID))
        {

        }
    }
}