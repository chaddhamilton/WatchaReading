using System;
using System.Collections.Generic;
using System.Net.Http;
using WhatchaReading.Models;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace WatchuReading.Services
{
    public class ServiceBase
    {
        private HttpClient client;
        IEnumerable<Book> book;
        IEnumerable<Activity> activity;
        IEnumerable<Favorite> favorite;
        public static ISettings AppSettings => CrossSettings.Current;

        public ServiceBase()
        {
           
        }

        public IEnumerable<Book> Book { get => book; set => book = value; }
        public IEnumerable<Activity> Activity { get => activity; set => activity = value; }
        public IEnumerable<Favorite> Favorite { get => favorite; set => favorite = value; }
       
        public static string LocalUserBookList //User's Book Reading History
        {
            get => AppSettings.GetValueOrDefault(Constants.BooksKey, string.Empty);
            set => AppSettings.AddOrUpdateValue(Constants.BooksKey, value);
        }

        public static string LocalFavorites //User's favorites
        {
            get => AppSettings.GetValueOrDefault(Constants.FaveKey, string.Empty);
            set => AppSettings.AddOrUpdateValue(Constants.FaveKey, value);
        }

        public static string LocalArchive //Entire Library
        {
            get => AppSettings.GetValueOrDefault(Constants.LibKey, string.Empty);
            set => AppSettings.AddOrUpdateValue(Constants.LibKey, value);
        }
    }
}

