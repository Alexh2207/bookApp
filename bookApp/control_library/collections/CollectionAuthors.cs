using System;
using System.Collections.Generic;
using control_library.data;

namespace control_library.collections
{
#pragma warning disable CS0659 // El tipo reemplaza a Object.Equals(object o), pero no reemplaza a Object.GetHashCode()
    public class CollectionAuthors
#pragma warning restore CS0659 // El tipo reemplaza a Object.Equals(object o), pero no reemplaza a Object.GetHashCode()
    {
        public List<Author> Authors { get; set; }

        public CollectionAuthors()
        {
            Authors = new List<Author>();
        }

        public CollectionAuthors(Author[] authors1)
        {
            Authors = new List<Author>(authors1);
        }

        public CollectionAuthors(List<Author> authors)
        {
            Authors = new List<Author>(authors);
        }

        public bool add(Author Author)
        {
            if (Authors.Contains(Author)) { return false; }
                Authors.Add(Author);
            return true;
        }

        public bool add(CollectionAuthors Authors)
        {
            foreach (Author element in Authors.Authors)
            {
                if (this.Authors.Contains(element)) { return false; }
                this.Authors.Add(element);
            }
            return true;
        }

        public bool remove(string name)
        {
            foreach (Author element in Authors)
            {
                if (element.AuthorID.Equals(name))
                {
                    return Authors.Remove(element);
                }
            }
            return false;
        }

        public Author find(double AuthorID)
        {
            foreach (Author element in Authors)
            {
                if (element.AuthorID.Equals(AuthorID))
                {
                    return element;
                }
            }
            throw new AuthorNotFoundException(AuthorID);
        }

        public override bool Equals(object obj)
        {
            return obj is CollectionAuthors authors &&
                   EqualityComparer<List<Author>>.Default.Equals(Authors, authors.Authors);
        }

        public override string ToString()
        {
            string names = string.Empty;
            foreach (Author element in Authors)
                names += element.Name + ", ";
            return names;
        }

    }

    internal class AuthorNotFoundException : Exception
    {
        public AuthorNotFoundException() { }

        public AuthorNotFoundException(double AuthorID) 
            : base(String.Format("Author not Found: {0}", AuthorID))
        {

        }
    }
}
