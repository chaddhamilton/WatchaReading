using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using WatchuReading.Models;
using Xamarin.Forms;

namespace WatchuReading.Views
{
    public partial class BookView : ContentView
    {


        public BookView()
        {
            InitializeComponent();
        }


        //async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        //{
        //    var item = args.SelectedItem as TodoItem;
        //    if (item == null)
        //        return;

        ////    await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

        //    // Manually deselect item
        //    //ItemsListView.SelectedItem = null;
        //}

        //async void AddItem_Clicked(object sender, EventArgs e)
        //{
        //  //  await Navigation.PushAsync(new NewItemPage());
        //}
    }
}
