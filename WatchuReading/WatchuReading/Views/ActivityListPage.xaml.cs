using System;
using System.Collections.Generic;
using WhatchaReading.Models;
using WatchuReading.ViewModels;

using Xamarin.Forms;

namespace WatchuReading.Views
{
    public partial class ActivityListPage : ContentPage
    {
        ActivityListViewModel vm;

        public ActivityListPage(Activity activity =null)
        {
            
            vm = new ActivityListViewModel(this);
            if (activity != null)
            {
                vm.AltUserId = activity.UserId;
                vm.DispName = activity.FullName.ToFriendlyName();
            }
          
            BindingContext = vm;
            InitializeComponent();

        }

        public new ActivityListViewModel ViewModel
        {
            get { return BindingContext as ActivityListViewModel; }
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
           // await Navigation.PushAsync(new NewItemPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //if (vm.Activities.Count == 0)
               vm.LoadItemsCommand.Execute(null);
        }
    }
}
