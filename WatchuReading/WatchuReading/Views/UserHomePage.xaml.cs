using System;
using System.Collections.Generic;
using WatchuReading.ViewModels;
using Xamarin.Forms;

namespace WatchuReading.Views
{
   
    public partial class UserHomePage : ContentPage
    {
        UserHomeViewModel vm;

        public UserHomePage()
        {
            vm = new UserHomeViewModel(this);
            BindingContext = vm;
            InitializeComponent();
        }

       // public new UserHomeViewModel CurViewModel => BindingContext as UserHomeViewModel;

        protected override void OnAppearing()
        {
            
            base.OnAppearing();
            vm.LoadActiveBooksCommand.Execute(null);

        }
    }
}
