using System;
using System.Collections.Generic;
using System.Linq;
using WatchuReading.ViewModels;
using Xamarin.Forms;

namespace WatchuReading.Views
{
    public partial class BookListPage : ContentPage
    {
        private bool IsRowEven;
       
        void Cell_Appearing(object sender, System.EventArgs e)
        {
           //if (this.IsRowEven)
            //{
            //    var c = (ViewCell)sender;
            //    if (c.View != null)
            //    {
            //        c.View.BackgroundColor = Color.Silver;
            //    }
            //}
            //this.IsRowEven = !this.IsRowEven;
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            LibraryList.BeginRefresh();

            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                LibraryList.ItemsSource = vm.Library;
            else
                LibraryList.ItemsSource = vm.Library.Where(i => i.Title.ToLower().Contains(e.NewTextValue.ToLower()));

            LibraryList.EndRefresh();
        }

        BookListViewModel vm;

        public BookListPage()
        {
            BindingContext = vm = new BookListViewModel();
           
            InitializeComponent();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

                vm.LoadItemsCommand.Execute(null);
        }
    }
}
