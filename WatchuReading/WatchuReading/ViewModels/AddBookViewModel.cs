using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WatchuReading.Data;
using WatchuReading.Interfaces;
using WatchuReading.Services;
using WatchuReading.Views;
using WhatchaReading.Models;

using Xamarin.Forms;


namespace WatchuReading.ViewModels
{
    public class AddBookViewModel : BaseViewModel
    {
        //COMMANDS
        public Command DoSearchCommand { get; }
        public Command AddToLibraryCommand { get; }
        private Page _page;

        public AddBookViewModel(Page page)
        {
            Title = "Add Book To Library";
            DoSearchCommand = new Command(async() => await DoBookSearch());
            SearchResults = new CustomObservableCollection<Book>();
            AddToLibraryCommand = new Command(async (obj) => await AddBook(obj));
            _page = page;

         }

        private async Task AddBook(object obj)
        {
            var b = (Book)obj;
            b.AddedBy = UserId;
            var msg = string.Empty;
            var manager = new ServiceManager();
            //Check to make sure this isn't a duplicate
            if (manager.BookExists(b))
                return;
            
            if(await _page.DisplayAlert("", $"Add {b.Title} to library?", "Yep", "Nope"))
            {
                IsBusy = true;

                //Save 
                var newBookId = await manager.AddBook(b);

                if (newBookId > 0)
                {
                    b.Id = newBookId;

                    DependencyService.Get<IMessage>().ShowSnackbar($"{b.Title} has been added to your Bookshelf");
                   
                   // SearchResults.Remove(b);
                    var userBooks = await manager.GetAllBooksByUser(UserId);

                    var act= new Activity()
                        {
                            Id = 0,
                            Book = b,
                            UserId = UserId,
                            IsReading = false
                        };

                    //See if the user has an active book.
                    if (await _page.DisplayAlert("", $"Set this as your current book?", "Yep", "Nope"))
                    {
                        act.IsReading = true;
                    }

                    await _manager.AddActivity(act);
                    await Application.Current.MainPage.Navigation.PopAsync();
                }

                else
                {
                    DependencyService.Get<IMessage>().ShowSnackbar($"! Could Not Add Book !");

                }

                searchResults.Clear();
                searchText = string.Empty;
            }

            IsBusy = false;
        }

        //Main Search
        private async Task DoBookSearch()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                _manager = new ServiceManager();
                var rez = await _manager.SearchGoogleBooks(SearchText);

                if(SearchResults?.Count>0){
                    SearchResults.Clear();
                }

                foreach (var b in rez.items)
                {
                    Book item = new Book()
                    {
                        Title = b.volumeInfo.title,
                        Author = string.Join(",", b.volumeInfo.authors),
                        ImageUrl = b.volumeInfo.imageLinks.smallThumbnail,
                        Description = b.volumeInfo.description,
                        PublishedDate = b.volumeInfo.publishedDate
                     };

                    SearchResults.Add(item);
                }
            }
            catch{
                
            }
            finally{
                IsBusy = false;
            }
        }

        string helpText = "To add a suggested book to the library, search by  the book's title";
        public string HelpText
        {
            get { return helpText; }
            set { this.SetProperty(ref helpText, value); }
        }

        string searchText;
        public string SearchText
        {
            get { return searchText; }
            set { this.SetProperty(ref searchText, value); }
        }




        private CustomObservableCollection<Book> searchResults; 
        public CustomObservableCollection<Book> SearchResults
        {
            get { return searchResults; }
            set { SetProperty(ref searchResults, value); }
        }



    }
}

