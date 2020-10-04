using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Windows.Input;
using WatchuReading.Data;
using WatchuReading.Interfaces;
using WhatchaReading.Models;
using WatchuReading.Services;
using WatchuReading.Views;
using Xamarin.Forms;
using static WatchuReading.Enums;

namespace WatchuReading.ViewModels
{
    public class BookReviewsViewModel:BaseViewModel
    {
        public Command CloseCommand { get; }
        public Command GetReviewsCommand { get; }
        public Command GetAllReaderCommand { get; }

        
        public BookReviewsViewModel(Book buk)
        {
            CloseCommand = new Command(async () => await App.Current.MainPage.Navigation.PopModalAsync());
            GetReviewsCommand = new Command(async () => await GetReviews());
            GetAllReaderCommand = new Command(async () => await GetAllReadersOfBook());
            Book = buk;
        }

        private async Task GetAllReadersOfBook()
        {
            IsBusy = true;

            try
            {
                _manager = new ServiceManager();
                Activities = await _manager.GetBookReviews(Book.Id);
            }
            catch { }
            finally { IsBusy = false; }
        }

        private async Task GetReviews()
        {
            IsBusy = true;

            try
            {
                _manager = new ServiceManager();
                Activities = await _manager.GetBookReviews(Book.Id);
            }
            catch { }
            finally { IsBusy = false; }

        }

        private Book book;
        public Book Book
        {
            get { return book; }
            set { SetProperty(ref book, value); }
        }

    }
}
