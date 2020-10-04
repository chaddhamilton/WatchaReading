using System;
using System.Linq;
using System.Threading.Tasks;
using Plugin.FirebasePushNotification;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using WatchuReading.Interfaces;
using WatchuReading.Services;
using WatchuReading.Views;
using WhatchaReading.Models;
using Xamarin.Forms;



namespace WatchuReading
{
    public partial class App : Application
    {
        public static bool UseMockDataStore = false;
       // public static string BackendUrl = "https://localhost:5000";

        public App()
        {
            
            InitializeComponent();
            var messanger = DependencyService.Get<IMessage>();

           //AppSettings.Clear(); // CAREFUL WITH THIS AX, Eugene
         
            //New User
            MainPage= new NavigationPage(new UserAccountPage())
            {
                BarBackgroundColor = Color.Black,
                BarTextColor = Color.White

            };
                      
            //Existing user
            if (UserId != 0)
            {
                //clear the cached booklist to force a refecth from the service
                AppSettings.Remove(Constants.LibKey);

                messanger.ShowSnackbar($"Welcome Back, {FirstName}!");
                MainPage = new NavigationPage(new UserHomePage())
                { 
                    BarBackgroundColor = Color.Black,
                    BarTextColor = Color.White
           
                };
            }

         //   Task.Run(async () => await PreloadCollections());

        }

        private static ISettings AppSettings =>  CrossSettings.Current;
        public static string FirstName //FirstName
        {
            get => AppSettings.GetValueOrDefault(Constants.FirstNameKey, string.Empty);
            set => AppSettings.AddOrUpdateValue(Constants.FirstNameKey, value);
        }

        public static int UserId
        {
            get => AppSettings.GetValueOrDefault(Constants.UserIdKey, 0);
            set => AppSettings.AddOrUpdateValue(Constants.UserIdKey, value);
        }

        private async Task PreloadCollections()
        {
            try
            {
                var _manager = new ServiceManager();
                var ret = await _manager.GetAllBooksAsync();
            }
            catch
            {
            } 
        }

    }

}
