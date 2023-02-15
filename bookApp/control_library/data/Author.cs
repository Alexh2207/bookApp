using System.Collections.Generic;
using System.Text;
using System;
using control_library.collections;

namespace control_library.data
{
#pragma warning disable CS0659 // El tipo reemplaza a Object.Equals(object o), pero no reemplaza a Object.GetHashCode()
    internal class Author
#pragma warning restore CS0659 // El tipo reemplaza a Object.Equals(object o), pero no reemplaza a Object.GetHashCode()
    {
        public double AuthorID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public CollectionBooks PubBooks { get; set; }

        public Author() 
        { 
            Name = "";
            Surname = "";
            PubBooks = new CollectionBooks();
        }
        public Author(double authorID, string name, string surname, CollectionBooks pubBooks)
        {
            byte[] asciiBytes = Encoding.ASCII.GetBytes(String.Concat(name, surname));
            int total = 0;
            Array.ForEach(asciiBytes, delegate (byte i) { total += i; });
            AuthorID = total;
            Name = name;
            Surname = surname;
            PubBooks = pubBooks;
        }
        public Author(string name, string surname)
        {
            byte[] asciiBytes = Encoding.ASCII.GetBytes(String.Concat(name, surname));
            int total = 0;
            Array.ForEach(asciiBytes, delegate (byte i) { total += i; });
            AuthorID = total;
            Name = name;
            Surname = surname;
            PubBooks = new CollectionBooks();
        }

        public bool addBook(Book book)
        {
            return PubBooks.add(book);
        }

        public override bool Equals(object obj)
        {
            return obj is Author author &&
                   AuthorID == author.AuthorID &&
                   Name == author.Name &&
                   Surname == author.Surname &&
                   EqualityComparer<CollectionBooks>.Default.Equals(PubBooks, author.PubBooks);
        }

    }
}