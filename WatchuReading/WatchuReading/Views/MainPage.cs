using System;

using Xamarin.Forms;

namespace WatchuReading
{
    public class MainPage : NavigationPage
    {
        public MainPage()
        {

            Page itemsPage, aboutPage = null;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:

                    break;
                default:
                   
                    break;
            }

            Title = "y";
        }


       
    }
}
