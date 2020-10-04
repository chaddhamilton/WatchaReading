using System;
using System.Collections.Generic;
using WatchuReading.ViewModels;

using Xamarin.Forms;

namespace WatchuReading.Views
{
    public partial class AddBookPage : ContentPage
    {
        AddBookViewModel vm;
        public AddBookPage()
        {
            BindingContext =vm = new AddBookViewModel(this);

            InitializeComponent();
        }



        void TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}
