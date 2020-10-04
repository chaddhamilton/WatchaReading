using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Connectivity;
using WhatchaReading.Models;
using WhatchaReading.Interface;

namespace WatchuReading.Data
{
    public class AzureDB
    {
        HttpClient client;
        string service;

        /// <summary>
        /// Constructor</summary>
        /// <param name="service"> Name of the service to use(Book, Users, Activity etc</param>
        public AzureDB(string ser = "")
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(string.Format(Constants.AzureAPI, string.Empty));
            service = ser;
        }

        /// <summary>
        /// Add new item
        /// </summary>
        public async Task<T> AddAsync<T>(T item) where T : IBusinessEntity
        {
            
            var serializedItem =  JsonConvert.SerializeObject(item);
            var response = await client.PostAsync($"api/{service}",
                                                  new StringContent(serializedItem, Encoding.UTF8, "application/json"));
            var contents = await response.Content.ReadAsStringAsync();
            var blob = JsonConvert.DeserializeObject<T>(contents);

            if(response.IsSuccessStatusCode){
                return blob;
            }

            return default(T);
        }

        /// <summary>
        /// Updates an item
        /// </summary>
        public async Task<bool> UpdateAsync<T>(T item) where T : IBusinessEntity
        {
            if (!CrossConnectivity.Current.IsConnected)
                return false;
            //throw new Exception("No network available");


            var serializedItem = JsonConvert.SerializeObject(item);
            var buffer = Encoding.UTF8.GetBytes(serializedItem);
            var byteContent = new ByteArrayContent(buffer);

            var response = await client.PutAsync($"api/{service}", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }


        /// <summary>
        /// Gets a list of items 
        /// </summary>
        public async Task<IEnumerable<T>> GetAllItems<T>(bool forceRefresh = false) where T : IBusinessEntity, new()
        {

            try
            {   //Call the GET REQUEST 
                var json = await client.GetStringAsync($"api/{service}");

                return await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<T>>(json));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return null;
        }

        /// <summary>
        /// Returns a single item
        /// </summary>
        public async Task<T> GetSingleItem<T>(int id) where T : IBusinessEntity, new()
        {

            try
            {   //Call the GET REQUEST 
                var json = await client.GetStringAsync($"api/{service}/{id}");

                return await Task.Run(() => JsonConvert.DeserializeObject<T>(json));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return default(T);

        }

        /// <summary>
        /// Deletes an item
        /// </summary>
        public async Task<bool> DeleteItem<T>(T item) where T : IBusinessEntity, new()
        {
            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var response = await client.DeleteAsync($"api/{service}/{item}");

            return response.IsSuccessStatusCode;

        }


        //USER 
        /// <summary>
        /// Log In Existing User
        /// </summary>
        public async Task<User> LogInUserAsync(string username, string password)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var t = client.BaseAddress;
                var json = await client.GetStringAsync($"api/Users/logon/{username}/{password}");
                return await Task.Run(() => JsonConvert.DeserializeObject<User>(json));
            }

            return null;

        }

        //USER 
        /// <summary>
        /// Send "Forgot Password" Email to Admin
        /// ("api/Users/Password")]
        /// </summary>
        public async Task<bool> SendPassWordResetEmail(User user)
        {
          
            if (CrossConnectivity.Current.IsConnected)
            {
                var t = client.BaseAddress;
                var serializedUser = JsonConvert.SerializeObject(user);

                var response = await client.PostAsync($"api/Users/Password",
                                                      new StringContent(serializedUser, Encoding.UTF8, "application/json"));
                var contents = await response.Content.ReadAsStringAsync();
                return response.IsSuccessStatusCode;
            }

            return false;
        }

       
    }

}
