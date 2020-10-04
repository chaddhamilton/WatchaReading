using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WatchuReading.Data
{
    
    public class GoogleDS
    {
        HttpClient client;
        string service = Constants.GoogleBooksAPI;

        public GoogleDS()
        {
        }

        public async Task<string> SearchBooks(string title)
        {
            try
            {
                client = new HttpClient();
                client.BaseAddress = new Uri($"{service}{title}+intitle:{title}");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                return await client.GetStringAsync(string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

            return null;
        }

        // https://www.googleapis.com/books/v1/volumes?q=cold+print+intitle:cold+print&key=AIzaSyDIvW44DeRKZcdl7JOV4qhyNBwuzuGKTcw
    }
}

