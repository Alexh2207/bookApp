using control_library.collections;
using control_library.data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using System.Runtime.CompilerServices;

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
                results = search.Result.docs.Take(50).ToList();
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
                foreach(string element in book.author_name)
                {
                    authorList.Add(new Author(element));
                }
                transformedResults.Add(new searchs(book.title, book.edition_key, new CollectionAuthors(authorList), book.cover_edition_key, book.key));
            }

            return transformedResults;
        }

        public async Task<searched_book> GetWorkData(search_book book)
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
                if(search != null) 
                    edList.Add(search.Result);
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

    public record class search_book(String title, List<String> edition_key, List<String> author_name, String cover_edition_key, String key);

    public record class searchs(String title, List<String> edition_key, CollectionAuthors author_name, String cover_edition_key, String key);

    public record class work(String title, List<String> subjects);

    public record class searched_book(String title, List<edition_book> edition_key, List<String> author_name, String cover_url, String key, List<String> subjects);

    public record class edition_book(String title, int number_of_pages, List<String> isbn_13, List<int> covers, List<authors_keys> authors);

    public record class authors_keys(String key);

    public record class authors(String personal_name);
}



namespace System.Runtime.CompilerServices
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static class IsExternalInit { }
}