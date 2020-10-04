using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WhatchaReading.Models;
using WatchuReading.Services;
using WatchuReading.Views;
using Xamarin.Forms;


namespace WatchuReading.ViewModels
{
    public class BookListViewModel : BaseViewModel
    {
        public Command LoadItemsCommand { get; }
        public Command SelectedBookCommand { get; }
        public Command AddBookCommand { get; }
        public Command BookDetailsCommand { get; }
        public Command StartReadingCommand { get; }

        public BookListViewModel()
        {
            Library = new CustomObservableCollection<Book>();
            Title = "Book List";
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            // NOT USING THIS FOR RIGHT NOW 1/22/2019
            // SelectedBookCommand = new Command<Book>(ShowMenu);
            StartReadingCommand = new Command(async (obj) => await StartReading(obj));
            AddBookCommand = new Command(async () => await Application.Current.MainPage.Navigation.PushAsync(new AddBookPage()));
            BookDetailsCommand = new Command(async (obj) => await ShowDetails(obj));
                                              }

        private async Task ShowDetails(object obj)
        {
            //Since we are viwing a cached list of books, we need to 
            //call the api and get a fresh version with
            //any new comments or rating changes.

            var b = obj as Book;
            var freshBook = await _manager.GetBook(b.Id);
            await Application.Current.MainPage.Navigation.PushAsync(new BookDetailsPage(freshBook));
        
        }

        private async Task StartReading(object obj)
        {
            var book = (Book)obj;
            _manager = new ServiceManager();
            var act = new Activity()
            {
                IsReading = true,
                UserId = UserId,
                Book = book
                             
            };

            act.IsReading = true;
            await _manager.AddActivity(act);
            await Application.Current.MainPage.Navigation.PopAsync();
        }

       async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                _manager = new ServiceManager();
                var ret = await _manager.GetAllBooksAsync();
                Library?.Clear();
               
                //Oder by Title
                var sorted = ret.OrderBy(t => t.Title);
                foreach (var item in sorted)
                {
                    item.LineItem = $"{item.Title} - {item.Author}";
                    Library.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }

        }

        private void ShowMenu(Book book)
        {
            //RESET ANY OTHER ITEMS
            foreach (Book b in Library)
            {
                b.ShowMenu = false;
                Library.ReportItemChange(b);
            }

            book.ShowMenu = true;
            Library.ReportItemChange(book);
        }

        private Color activeRowColor = Color.White;
        public Color ActiveRowColor
        {
            get { return activeRowColor; }
            set { SetProperty(ref activeRowColor, value); }
        }


        #region PROPERTIES


        #endregion  
    }
}

