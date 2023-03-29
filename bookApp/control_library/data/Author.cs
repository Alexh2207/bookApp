using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System;
using control_library.collections;

namespace control_library.data
{
#pragma warning disable CS0659 // El tipo reemplaza a Object.Equals(object o), pero no reemplaza a Object.GetHashCode()
    public class Author
#pragma warning restore CS0659 // El tipo reemplaza a Object.Equals(object o), pero no reemplaza a Object.GetHashCode()
    {
        public double AuthorID { get; set; }
        public string Name { get; set; }
        public List<string> PubBooks { get; set; }

        public Author() 
        { 
            Name = "";
            PubBooks = new List<string>();
        }
        public Author(double authorID, string name, List<string> pubBooks)
        {
            byte[] asciiBytes = Encoding.ASCII.GetBytes(name);
            int total = 0;
            Array.ForEach(asciiBytes, delegate (byte i) { total += i; });
            AuthorID = total;
            Name = name;
            PubBooks = pubBooks;
        }
        public Author(string name)
        {
            byte[] asciiBytes = Encoding.ASCII.GetBytes(name);
            int total = 0;
            Array.ForEach(asciiBytes, delegate (byte i) { total += i; });
            AuthorID = total;
            Name = name;
            PubBooks = new List<string>();
        }

        public void addBook(Book book)
        {
            PubBooks.Add(book.Isbn);
        }

        public override bool Equals(object obj)
        {
            return obj is Author author &&
                   AuthorID == author.AuthorID &&
                   Name == author.Name;
        }

        public override string ToString()
        {
            return Name;
        }

        public string serializeAuthor()
        {

            return JsonSerializer.Serialize(this);

        }

    }
}