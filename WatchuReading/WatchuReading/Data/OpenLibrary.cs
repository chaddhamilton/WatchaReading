using System;
using System.Net;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Connectivity;
using WhatchaReading.Models;

namespace WatchuReading.Data
{
    public class OpenLibrary
    {
        HttpClient client;
        string service = Constants.OpenLibraryUrl;

        /// <summary>
        /// Constructor</summary>
        /// <param name="service"> </param>
        public OpenLibrary()
        {
            client = new HttpClient();
           
        }

        public async Task<string> SearchBooks(string title)
        {
            try
            {
                client.BaseAddress = new Uri($"{service}search.json?title={title}");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                return await client.GetStringAsync(string.Empty);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

            return null;
        }



     
    }
}
