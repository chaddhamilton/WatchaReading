using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using WatchuReading.Models;
using Xamarin.Forms;

namespace WatchuReading
{
    public class ItemsViewModel : BaseViewModel
    {
        //public ObservableCollection<TodoItem> Items { get; set; }
        //public Command LoadItemsCommand { get; set; }

        //public ItemsViewModel()
        //{
        //    Title = "Browse";
        //    Items = new ObservableCollection<TodoItem>();
        //    LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

        //    MessagingCenter.Subscribe<NewItemPage, TodoItem>(this, "AddItem", async (obj, item) =>
        //    {
        //        var _item = item as TodoItem;
        //        Items.Add(_item);
        //       // await DataStore.AddAsync(_item);
        //    });


        //}

        //async Task ExecuteLoadItemsCommand()
        //{
        //    if (IsBusy)
        //        return;

        //    IsBusy = true;

        //    try
        //    {
        //        Items.Clear();

        //        var items = await DataStore.GetAllAsync(true);
        //        foreach (var item in items)
        //        {
        //            Items.Add(item);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex);
        //    }
        //    finally
        //    {
        //        IsBusy = false;
        //    }
        //}
    }
}
