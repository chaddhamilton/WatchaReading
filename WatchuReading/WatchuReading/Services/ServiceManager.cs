using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhatchaReading.Models;
using WatchuReading.Data;
using WatchuReading.Models;
using Plugin.Connectivity;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace WatchuReading.Services
{
    public class ServiceManager : ServiceBase
    {
        AzureDB db = new AzureDB();

        public ServiceManager()
        {

        }

        //MAIN READING ACTIVITY FETCH AND PERSIST
        private async Task<IEnumerable<Activity>> GetBooksByUser(int id, bool refresh = false, bool cacheresults = true)
        {

            //Check to see if this is persisted(save trips back to Azure)
            if (AppSettings.Contains(Constants.BooksKey) && LocalUserBookList.Length > 2 && refresh == false)
            {
                string json = LocalUserBookList;

                var obj = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Activity>>(json));
                return obj;
            }
            else
            {
                ResetLocalBookCache();

                if (CrossConnectivity.Current.IsConnected)
                {
                    db = new AzureDB($"Activity/{id}/all");
                    var books = await db.GetAllItems<Activity>(true);

                    //When returning lists of other users we do not want to 
                    //save this as cached data
                    if (cacheresults)
                    {
                        //Serialize to string and Persist
                        var serJson = JsonConvert.SerializeObject(books);
                        LocalUserBookList = serJson;
                    }
                    return books;
                }
                else
                {

                    //TODO sqllite 
                }
            }

            return null;
        }


        //Get Entire Library
        public async Task<CustomObservableCollection<Book>> GetAllBooksAsync()
        {
            var retList = new CustomObservableCollection<Book>();

            //Check to see if this is persisted
            if (AppSettings.Contains(Constants.LibKey) && LocalArchive.Length > 2)
            {
                string json = LocalArchive;

                var books = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Book>>(json));

                foreach (var item in books)
                {
                    retList.Add(item);
                }
            }

            else if (CrossConnectivity.Current.IsConnected)
            {
                db = new AzureDB("Book");
                var books = await db.GetAllItems<Book>(true);

                foreach (var item in books)
                {
                    retList.Add(item);
                }

                //Serialize to string and Persist Library
                var serJson = JsonConvert.SerializeObject(books);
                LocalArchive = serJson;
                return retList;
            }
            else
            {

                //TODO sqllite 
            }

            return retList;
        }


        //ALL READ BOOKS BY USER(either current user or another app user) 
        public async Task<CustomObservableCollection<Activity>> GetAllBooksByUser(int id, bool refresh = false)
        {
            var retList = new CustomObservableCollection<Activity>();
            var act = await GetBooksByUser(id, refresh, !refresh);

            //Sort active on top
            var soList = act.OrderBy(e => e.IsReading ? 0 : 1).ThenBy(e => e.Book.Title);
           
            foreach (var item in soList)
            {
                item.LineItem = $"{item.Book.Title} - {item.Book.Author}";
           
                retList.Add(item);
            }

            return retList;
        }


       //ALL BOOKS Activity by book id 
        public async Task<CustomObservableCollection<Activity>> GetBookReviews(int id)
        {
            var retList = new CustomObservableCollection<Activity>();

            if (CrossConnectivity.Current.IsConnected)
            {
                db = new AzureDB($"Activity/book/all/{id}");
                var act = await db.GetAllItems<Activity>(true);

                //Order by User on top
                var soList = act.OrderBy(e => e.FullName);

                foreach (var item in soList)
                {
                  var n = item.FullName.ToFriendlyName();
                    item.LineItem = $"{item.FullName} Rated it {item.Rating}";
                    retList.Add(item);  
                }
            }

            return retList;
        }


        //ALL BOOKS BY ALL USERS
        public async Task<CustomObservableCollection<Activity>> GetActiveBooksByAllUsers(int uid)
        {
            var retList = new CustomObservableCollection<Activity>();
          
            if (CrossConnectivity.Current.IsConnected)
            {
                db = new AzureDB($"Activity/{0}/all");
                var books = await db.GetAllItems<Activity>(true);

                //Order by User on top
                var soList = books.OrderBy(e => e.FullName).Where(e => e.IsReading==true);

                foreach (var item in soList)
                {
                    if (item.UserId != uid)
                    {
                        var n = item.FullName.ToFriendlyName();
                        item.B1Text = $"{n} Books";
                        item.B2Text = $"Read {item.Book.Title}";
                        item.LineItem = $"{item.FullName.ToLineItemName()} is reading {item.Book.Title}";
                        retList.Add(item);
                    }
                }
            }

            return retList;
        }



        // BOOKSHELF/FAVORITE - GET
        public async Task<CustomObservableCollection<Favorite>> GetUserFavorites(int id, bool refresh = false)
        {
            var retList = new CustomObservableCollection<Favorite>();

            //Check to see if this is persisted(save trips back to Azure)
            if (AppSettings.Contains(Constants.FaveKey) && LocalFavorites.Length > 2 && refresh == false)
            {
                string json = LocalFavorites;

                var obj = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Favorite>>(json));
                return (CustomObservableCollection<Favorite>)obj;
            }
            else
            {
                ResetFaveCache();
                if (CrossConnectivity.Current.IsConnected)
                {
                    db = new AzureDB($"Favorite/{id}");
                    retList = (CustomObservableCollection<Favorite>)await db.GetAllItems<Favorite>(false);
                }

                return retList;
            }
        }

       
        //ACTIVE BOOK
        public async Task<Activity> GetActiveBookByUser(int id)
        {
            var retList = new List<Activity>();
            var act = await GetBooksByUser(id, false);
            foreach (var item in act.Where(b => b.IsReading == true))
            {
                retList.Add(item);
            }
            if (retList.Count > 0)
                {
                return retList[0]; //THERE *SHOULD* ONLY BE ONE
                }
            return null;
         }

        /// <summary>
        /// Returns a Single Book
        /// </summary>
        public async Task<Book> GetBook(int id)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                db = new AzureDB("Book");
                return await db.GetSingleItem<Book>(id);
            }
            else //return book from cache
            {
                var bl = await GetAllBooksAsync();
                return bl.FirstOrDefault(b => b.Id == id);
            }

        }

        /// <summary>
        /// Start Reading Activity
        /// </summary>
        public async Task<int> AddActivity(Activity act)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                db = new AzureDB("Activity");

                var rez = await db.AddAsync(act);

                    ResetLocalBookCache();
                return rez.Id;
            }

            return 0;
        }

        /// <summary>
        /// UPDATE Reading Activity
        /// </summary>
        public async Task<bool> UpdateActivity(Activity act)
        {

            db = new AzureDB("Activity");
            var ret = await db.UpdateAsync(act);

            //Clear the cached booklist
            if (ret)
            {
                ResetLocalBookCache();
            }

            return ret;
        }



        //USER 
        /// <summary>
        /// Log In Existing User
        /// </summary>
        public async Task<User> LogInUserAsync(string username, string password)
        {

            if (CrossConnectivity.Current.IsConnected)
            {
                db = new AzureDB("Users");
                return await db.LogInUserAsync(username, password);
            }

            return null;

        }


        /// <summary>
        /// Create Account
        /// </summary>
        public async Task<bool> CreateAccountAsync(User user)
        {

            if (CrossConnectivity.Current.IsConnected)
            {
                db = new AzureDB("Users");
                var rez = await db.AddAsync(user);
                return rez.Id != 0;
            }

            return false;

        }

        /// <summary>
        /// Add New Book
        /// </summary>
        public async Task<int> AddBook(Book book)
        {

            if (CrossConnectivity.Current.IsConnected)
            {
                db = new AzureDB("Book");
                var rez = await db.AddAsync(book);
                if(rez.Id !=0)
                {
                    //remove cached booklist for refetch
                    try
                    {
                        ResetLibraryCache();
                    }
                    catch{
                        
                    }
                    return rez.Id;
                }
            }

            return 0;

        }

        //SEND CHADDMIN AN EMAIL REGARDING USERS LOST CREDS
        public async Task<bool> SendPassWordResetEmail(User user)
        {
            if (user == null || !CrossConnectivity.Current.IsConnected)
                return false;
            db = new AzureDB("Users");
            return await db.SendPassWordResetEmail(user);

        }

        public async Task<GoogleBooksDto> SearchGoogleBooks(string crit)
        {
            GoogleDS google = new GoogleDS();
            var searchstring = crit.Replace(" ", "+");
            var json = await google.SearchBooks(searchstring);
            if (!String.IsNullOrEmpty(json))
            {
                return JsonConvert.DeserializeObject<GoogleBooksDto>(json);
            }

            return null;
        }

        public bool BookExists(Book b)
        {
            if(String.IsNullOrEmpty(LocalArchive))
            {
                Task.Run(async () => await GetAllBooksAsync());
            }

            var lib = JsonConvert.DeserializeObject<IEnumerable<Book>>(LocalArchive);
          
            return (lib.Any(x => x.Title == b.Title));
        }

        public async Task ResetAll()
        {
            await Task.Run(() => ResetLocalBookCache());
            await Task.Run(() => ResetLibraryCache());
            await Task.Run(() => ResetFaveCache());
        }

        //CACHED BOOKLIST RESET
        public void ResetLocalBookCache()
        {
          AppSettings.Remove(Constants.BooksKey);
        }

        //CACHED LIBRARY RESET
        public void ResetLibraryCache()
        {
            AppSettings.Remove(Constants.LibKey);
            Task.Run(async () => await GetAllBooksAsync());
        }

        //CACHED FAVORITES RESET
        public void ResetFaveCache()
        {
            AppSettings.Remove(Constants.FaveKey);
        }


    }
}
