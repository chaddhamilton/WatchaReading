using System;
using Xamarin.Forms;

namespace WatchuReading
{
    public class Constants
    {
        // URL of REST service
        public static string AzureAPI = "http://whatchareading.azurewebsites.net/";
        public static string OpenLibraryUrl = "http://openlibrary.org/";

        // Credentials that are hard coded into the REST service
        public static string LogonKey = "logonkey";
        public static string UserIdKey = "userid_key";
        public static string FirstNameKey = "fname_key";
        public static string BooksKey = "books_key";
        public static string LibKey = "lib_key";
        public static string FaveKey = "fave_key";
       
        //Color for Active Book 
        public static Color ActiveBookColor = Color.Orange;
        public static string UpdateErrorMsg = "Hmm, we couldn't save your details...try again";

        //Google Books Api
        public static string GoogleAPIkey = "AIzaSyDIvW44DeRKZcdl7JOV4qhyNBwuzuGKTcw";
        public static string GoogleBooksAPI = "https://www.googleapis.com/books/v1/volumes?q=";
    }
}
