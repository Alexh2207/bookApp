using control_library.collections;
using control_library.data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace control_library.data_retrieval
{
    public class OpenLibraryClient
    {
        HttpClient client;
        public OpenLibraryClient() 
        {
            client = new HttpClient();
        }

        public async Task<List<searchs>> GetSearch(string terms)
        {
            String url = "https://openlibrary.org/search.json?q=";

            terms = terms.Replace(" ", "+");

            url = url + terms;

            Stream json = await client.GetStreamAsync(url);

            var search = JsonSerializer.DeserializeAsync<search>(json);

            List<search_book> results;

            if(search.Result.numFound >= 20)
            {
                results = search.Result.docs.Take(20).ToList();
            }else if(search.Result.numFound == 0)
            {
                return null;
            }
            else
            {
                results = search.Result.docs;
            }

            List<searchs> transformedResults = new List<searchs>();

            foreach (search_book book in results)
            {
                List<Author> authorList = new List<Author>();
                if (book.author_name != null)
                {
                    foreach (string element in book.author_name)
                    {
                        authorList.Add(new Author(element));
                    }
                }
                if (book.cover_edition_key != null)
                {
                    transformedResults.Add(new searchs(book.title, book.edition_key, new CollectionAuthors(authorList), "https://covers.openlibrary.org/b/olid/" + book.cover_edition_key + "-L.jpg", book.key));
                }
                else
                {
                    transformedResults.Add(new searchs(book.title, book.edition_key, new CollectionAuthors(authorList), "Bookdefault.jpg", book.key));
                }
            }

            return transformedResults;
        }

        public async Task<searched_book> GetWorkData(searchs book)
        {

            String base_url = "https://openlibrary.org/books/";

            string url;


            Stream json;

            List<string> editions= new List<string>();

            List<edition_book> edList = new List<edition_book>();

            if (book.edition_key.Count > 20) 
            {
                editions = book.edition_key.Take(20).ToList();
            }
            else
            {
                editions = book.edition_key;
            }

            foreach (string element in editions)
            {
                string[] subid = element.Split('/');
                url = base_url + subid[0] + ".json"; 
                Console.WriteLine(url);
                json = await client.GetStreamAsync(url);
                var search = (JsonSerializer.DeserializeAsync<edition_book>(json));
                if (search != null)
                {
                    edList.Add(search.Result);
                }
            }

            url = "https://openlibrary.org/" + book.key + ".json";

            json = await client.GetStreamAsync(url);
            var search_key = JsonSerializer.DeserializeAsync<work>(json);

            string cover_url = "https://covers.openlibrary.org/b/olid/" + book.cover_edition_key + "-M.jpg";

            searched_book searched = new searched_book(book.title, edList, book.author_name, cover_url, book.key, search_key.Result.subjects);

            return searched;
        }

    }


    public record class search(int numFound, List<search_book> docs, String q);

    /// <summary>
    /// Work record to deserialize JSON.
    /// </summary>
    /// <param name="title">Common title to editions</param>
    /// <param name="edition_key">Keys of all editions</param>
    /// <param name="author_name">Author name</param>
    /// <param name="cover_edition_key">Cover key</param>
    /// <param name="key">Work key</param>
    public record class search_book(String title, List<String> edition_key, List<String> author_name, String cover_edition_key, String key);

    /// <summary>
    /// Translation to CollectionAuthors.
    /// </summary>
    /// <param name="title">Common title to editions</param>
    /// <param name="edition_key">Keys of all editions</param>
    /// <param name="author_name">Author name</param>
    /// <param name="cover_edition_key">Cover key</param>
    /// <param name="key">Work key</param>
    public record class searchs(String title, List<String> edition_key, CollectionAuthors author_name, String cover_edition_key, String key);

    /// <summary>
    /// Work specific data
    /// </summary>
    /// <param name="title">Title</param>
    /// <param name="subjects">Subjects of the book</param>
    public record class work(String title, List<String> subjects);

    /// <summary>
    /// Record to store most information of a concrete searched book
    /// </summary>
    /// <param name="title"></param>
    /// <param name="edition_key"></param>
    /// <param name="author_name"></param>
    /// <param name="cover_url"></param>
    /// <param name="key"></param>
    /// <param name="subjects"></param>
    public record class searched_book(String title, List<edition_book> edition_key, CollectionAuthors author_name, String cover_url, String key, List<String> subjects);

    /// <summary>
    /// Edition specific record to store information of an edition
    /// </summary>
    /// <param name="title"></param>
    /// <param name="number_of_pages"></param>
    /// <param name="isbn_13"></param>
    /// <param name="covers"></param>
    /// <param name="authors"></param>
    public record class edition_book(String title, int number_of_pages, List<String> isbn_13, List<int> covers, List<authors_keys> authors);

    /// <summary>
    /// Translation of data to be understood by mobile application. Please clean.
    /// </summary>
    /// <param name="title"></param>
    /// <param name="number_of_pages"></param>
    /// <param name="isbn_13"></param>
    /// <param name="covers"></param>
    /// <param name="authors"></param>
    public record class edition_transform(String title, int number_of_pages, String isbn_13, string covers, CollectionAuthors authors);

    public record class authors_keys(String key);

    public record class authors(String personal_name);
}



namespace System.Runtime.CompilerServices
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static class IsExternalInit { }
}