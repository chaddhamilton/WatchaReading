using System;
using System.Collections.Generic;
using WatchuReading.ViewModels;
using Xamarin.Forms;

namespace WatchuReading.Views
{
    public partial class AllActivityListPage : ContentPage
    {
        AllActivityViewModel vm;

        public AllActivityListPage()
        {
            BindingContext = vm = new AllActivityViewModel();
            InitializeComponent();

        }

        //call up the load
        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.LoadListCommand.Execute(null);
        }
    }
}
