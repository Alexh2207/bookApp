using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using control_library.collections;
using control_library.data;

namespace control_library
{
    /// <summary>
    /// Class that allows for a higher level of abstraction from the rest of the control logic of the app
    /// </summary>
    public class DataController
    {

        /// <summary>
        /// Bookshelves located in the top layer of the app
        /// </summary>
        CollectionBookshelves TopLayerBookshelves { get; set; }

        /// <summary>
        /// Every bookoshelf present in the app
        /// </summary>
        CollectionBookshelves AllBookshelves { get; set; }

        /// <summary>
        /// All Books present in the app
        /// </summary>
        Bookshelf AllBooks { get; set; }

        /// <summary>
        /// List of books in the wishlist
        /// </summary>
        Bookshelf Wishlist { get; set; }

        /// <summary>
        /// All Authors present in the app
        /// </summary>
        CollectionAuthors AllAuthors { get; set; }

        /// <summary>
        /// Initializes the Data Controller
        /// </summary>
        public DataController()
        {
            //IMPLEMENT DATA RETRIEVAL

            TopLayerBookshelves = new CollectionBookshelves();
            AllBookshelves = new CollectionBookshelves();
            AllBooks = new Bookshelf();
            AllAuthors = new CollectionAuthors();
            Wishlist = new Bookshelf();

            addBook("1234", "Fernando", "Pescador", "Hola", "Terror");
            addBook("123345", "Fernando", "Pescador", "Adios", "Comedia");

            Bookshelf books = new Bookshelf("All Books", "All the books stored in the app", AllBooks.Books.ToArray());

            AllBookshelves.add(books);
            TopLayerBookshelves.add(books);

            Bookshelf wish = new Bookshelf("Wishlist", "The books you want to read", Wishlist.Books.ToArray());

            AllBookshelves.add(wish);
            TopLayerBookshelves.add(wish);
        }
        
        /// <summary>
        /// Creates a Bookshelf at the top layer
        /// </summary>
        /// <param name="name"> Name of the Bookshelf</param>
        /// <param name="concept"> Concept of the Bookshelf</param>
        /// <returns>True if successful, False if failure</returns>
        public bool createTopBookshelf(string name, string concept)
        {
            Bookshelf bookshelf = new Bookshelf(name, concept);
            Console.WriteLine(bookshelf.BookshelfID);
            return TopLayerBookshelves.add(bookshelf) && AllBookshelves.add(bookshelf);
        }

        /// <summary>
        /// Creates a Bookshelf in another Bookshelf
        /// </summary>
        /// <param name="name">Name of the bookshelf</param>
        /// <param name="concept">Concept of the Bookshelf</param>
        /// <param name="bookshelfID">ID of the Parent of the Bookshelf</param>
        /// <returns>True if successful, False if failure</returns>
        public bool createSubBookshelf(string name, string concept, double bookshelfID)
        {
            Bookshelf bookshelf = new Bookshelf(name, concept);
#pragma warning disable CS0168 // La variable está declarada pero nunca se usa
            try
            {
                foreach(Bookshelf element in AllBookshelves.find(bookshelfID).Shelves.Bookshelves)
                {
                    if (element.ShelfName.Equals(name))
                    {
                        Console.WriteLine("nombre ya existente");
                        return false;
                    }
                }

                if (!AllBookshelves.find(bookshelfID).addShelf(bookshelf))
                {
                    return false;
                }
            }
            catch(BookshelfNotFoundException e)
            {
                return false;
            }
#pragma warning restore CS0168 // La variable está declarada pero nunca se usa
            return AllBookshelves.add(bookshelf);
        }

        /// <summary>
        /// Adds a Book to the application.
        /// </summary>
        /// <param name="isbn">ISBN of the book</param>
        /// <param name="authorName">Name of the author</param>
        /// <param name="authorSurname">Surname of the author</param>
        /// <param name="title">Title of the book</param>
        /// <param name="genre">Genre of the book</param>
        /// <returns>True if successful, False if failure</returns>
        public bool addBook(string isbn, string authorName, string authorSurname, string title, string genre)
        {
            Author author = new Author(authorName, authorSurname);
            Book newBook;
            //AÑADIR LIBRO A PUBLICADOS DEL AUTOR
            //Search for author
            try
#pragma warning disable CS0168 // La variable está declarada pero nunca se usa
            {
                if (AllAuthors.add(author))
                {
                    newBook = new Book(isbn, author, title, genre);
                    AllAuthors.find(author.AuthorID).addBook(newBook);
                }
                else
                {
                    newBook = new Book(isbn, AllAuthors.find(author.AuthorID), title, genre);
                    AllAuthors.find(author.AuthorID).addBook(newBook);
                } 
            }catch(AuthorNotFoundException e)
            {
                return false;
            }
#pragma warning restore CS0168 // La variable está declarada pero nunca se usa
            return AllBooks.add(newBook);
        }

        /// <summary>
        /// Adds a book to a given bookshelf
        /// </summary>
        /// <param name="Isbn">ISBN of the book</param>
        /// <param name="bookshelfID">ID of the bookshelf</param>
        /// <returns>True if successful, False if failure</returns>
        public bool addBookToBookshelf(string Isbn, double bookshelfID)
        {
#pragma warning disable CS0168 // La variable está declarada pero nunca se usa
            try
            {
                AllBookshelves.find(bookshelfID).addBook(AllBooks.find(Isbn));
                AllBooks.find(Isbn).addToShelf(bookshelfID);
            }
            catch(BookshelfNotFoundException e)
            {
                return false;
            }
            catch(BookNotFoundException e)
            {
                return false;
            }
#pragma warning restore CS0168 // La variable está declarada pero nunca se usa
            return true;
        }

        /// <summary>
        /// Add a valoration to a book, consisting of a rating and a description
        /// </summary>
        /// <param name="isbn">ISBN of the book</param>
        /// <param name="score">Score over 10 given to the book</param>
        /// <param name="valoration">Description of the valoration</param>
        /// <returns>True if successful, False if failure</returns>
        public bool addValoration(string isbn, double score, string valoration)
        {
            try
#pragma warning disable CS0168 // La variable está declarada pero nunca se usa
            {
                if(score <= 10)
                    AllBooks.find(isbn).addValoration(score, valoration);
                else
                {
                    return false;
                }
            }catch(BookNotFoundException e)
            {
                return false;
            }
#pragma warning restore CS0168 // La variable está declarada pero nunca se usa
            return true;
        }

        /// <summary>
        /// Remove a book from the application
        /// </summary>
        /// <param name="isbn">ISBN of the book</param>
        /// <returns>True if successful, False if failure</returns>
        public bool removeBook(string isbn)
        {
            foreach(Bookshelf bookshelf in AllBookshelves.Bookshelves)
            {
                bookshelf.remove(isbn);
            }
            return AllBooks.remove(isbn);
        }

        /// <summary>
        /// Remove a bookshelf from the application
        /// </summary>
        /// <param name="ID">ID of the bookshelf</param>
        /// <returns>True if successful, False if failure</returns>
        public bool removeBookshelf(double ID)
        {
            foreach(Bookshelf booksh in AllBookshelves.find(ID).Shelves.Bookshelves)
            {
                AllBookshelves.remove(booksh.BookshelfID);
            }

            foreach(Bookshelf bookshelf in AllBookshelves.Bookshelves)
            {
                bookshelf.Shelves.remove(ID);
            }
            foreach(Book book in AllBooks.Books)
            {
                book.Shelves.Remove(ID);
            }
            return AllBookshelves.remove(ID);
        }

        /// <summary>
        /// Edit the parameters of a bookshelf
        /// </summary>
        /// <param name="BookshelfID">ID of the bookshelf</param>
        /// <param name="newName">New name to add</param>
        /// <param name="newConcept">New concept to add</param>
        /// <returns>True if successful, False if failure</returns>
        public bool editBookshelf(double BookshelfID, string newName, string newConcept)
        {
            Bookshelf targetBs = new Bookshelf();
            try
            {
                targetBs = AllBookshelves.find(BookshelfID);
            }
            catch
            {
                return false;
            }
            finally
            {
                if (newName != null)
                {
                    targetBs.ShelfName = newName;
                }

                if (newConcept != null)
                {
                    targetBs.Concept = newConcept;
                }
                
            }
            return true;
        }

        /// <summary>
        /// Edit the parameters of a book
        /// </summary>
        /// <param name="isbn">ISBN of the book</param>
        /// <param name="newtitle">New title of the book</param>
        /// <param name="newGenre">New genre of the book</param>
        /// <param name="newIsbn">New ISBN of the book</param>
        /// <param name="newAuthorName">New author name of the book</param>
        /// <param name="newAuthorSurname">New author surname of the book</param>
        /// <returns>True if successful, False if failure</returns>
        public bool editBook(string isbn, string newtitle, string newGenre, string newIsbn, string newAuthorName, string newAuthorSurname)
        {
            Book targetB = new Book();
            try
            {
                targetB = AllBooks.find(isbn);
            }
            catch
            {
                return false;
            }
            finally
            {
                if (newtitle != null)
                {
                    targetB.Title = newtitle;
                }

                if (newAuthorName != null)
                {
                    targetB.Author.Name = newAuthorName;
                }

                if (newAuthorSurname != null)
                {
                    targetB.Author.Surname = newAuthorSurname;
                }

                if (newGenre != null)
                {
                    targetB.Genre = newGenre;
                }

                if (newIsbn != null)
                {
                    targetB.Isbn = newIsbn;
                }

            }
            return true;
        }

        /// <summary>
        /// Remove a Book ONLY from a Bookshelf
        /// </summary>
        /// <param name="isbn">ISBN of the book</param>
        /// <param name="bookhelfID">ID of the bookshelf</param>
        /// <returns>True if successful, False if failure</returns>
        public bool removeBookFromBS(string isbn, double bookhelfID)
        {
            Book target = AllBooks.find(isbn);
            target.Shelves.Remove(bookhelfID);
            return AllBookshelves.find(bookhelfID).remove(isbn);
        }

        public Task<Bookshelf> GetBooksAsync()
        {
            return Task.FromResult(AllBooks);
        }

        public Task<CollectionBookshelves> GetTShelfAsync()
        {
            return Task.FromResult(TopLayerBookshelves);
        }
    }
}
