using System;
using System.Collections.Generic;
using Xamarin.Forms;
using WatchuReading.ViewModels;


namespace WatchuReading.Views
{
    public partial class UserAccountPage : ContentPage
    {
        UserAccountViewModel vw = new UserAccountViewModel();
        public UserAccountPage()
        {
            BindingContext = vw;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            base.OnAppearing();
           // vw.OnRelandingCommand.Execute(null);
        }
    }
}
