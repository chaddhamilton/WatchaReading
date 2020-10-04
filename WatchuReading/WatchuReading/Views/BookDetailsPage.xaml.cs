using System;
using System.Collections.Generic;
using WhatchaReading.Models;
using WatchuReading.ViewModels;
using Xamarin.Forms;

namespace WatchuReading.Views
{
    public partial class BookDetailsPage : ContentPage
    {
        BookDetailsViewModel vm;

        public BookDetailsPage(Book book)
        {
      
            BindingContext = vm = new BookDetailsViewModel(book);
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.CheckBookStatusCommand.Execute(null);
        }
    }
}
