using System;
using System.Collections.Generic;
using WhatchaReading.Models;
using WatchuReading.ViewModels;
using Xamarin.Forms;

namespace WatchuReading.Views
{
    public partial class MyBookShelfPage : ContentPage
    {
        BookShelfVewModel vm;

        public MyBookShelfPage()
        {
            BindingContext = vm = new BookShelfVewModel(this);
            InitializeComponent();
        }

        //Load the users books
        protected override void OnAppearing()
        {
            base.OnAppearing();

            //if (vm.Activities.Count == 0)
            vm.LoadBooksCommand.Execute(null);
        }
    }
}
