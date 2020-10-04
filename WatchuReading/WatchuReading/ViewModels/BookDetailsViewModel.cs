using System;
using System.Threading.Tasks;
using WatchuReading.Interfaces;
using WhatchaReading.Models;
using WatchuReading.Services;
using WatchuReading.Views;
using Xamarin.Forms;
using static WatchuReading.Enums;

namespace WatchuReading.ViewModels
{
    public class BookDetailsViewModel : BaseViewModel
    {

        public Command StartReadingCommand { get; }
        public Command CheckBookStatusCommand { get; }
        public Command WhoIsReadingCommand { get; }
        public Command ShowCommentsCommand { get; }

        public BookDetailsViewModel(Book book)
        {
            Title = "Book Details";
            CurBook = book;
            CurBook.PublishedDate = CurBook.PublishedDate.Substring(0, 4);
            StartReadingCommand = new Command(async () => await StartReading());
            CheckBookStatusCommand = new Command(async () => await CheckReadingStatus());
            WhoIsReadingCommand = new Command(async () => await ShowWhoIsReading());
            //GET USER REVIEWS FROM COMMENTS TODO
            _manager = new ServiceManager();

        }

        private async Task ShowWhoIsReading()
        {
            //Show Modal
            await  App.Current.MainPage.Navigation.PushModalAsync(new BookReviewsPage(BookAction.Comments, CurBook));
        }

        private async Task CheckReadingStatus()
        {
            var act = await _manager.GetActiveBookByUser(UserId);
            if (act == null)
                return;

            CanShowRead = (act.Book.Id != CurBook.Id);

        }

        private async Task StartReading()
        {
            _manager = new ServiceManager();
            var act = new Activity()
            {
                Book = CurBook,
                IsReading = true,
                UserId = UserId

            };
            try
            {
                await _manager.AddActivity(act);
                DependencyService.Get<IMessage>().ShowSnackbar($"You are now reading {CurBook.Title}. ");

            }
            catch
            {
                DependencyService.Get<IMessage>().ShowSnackbar("Unable to mark this as your active book");
                //todo send email
            }
            finally
            {
               await Application.Current.MainPage.Navigation.PopAsync();
            }
        }

        private Book curBook;
        public Book CurBook
        { 
            get { return curBook; }
            set { SetProperty(ref curBook, value); }
        }

        //Is the user currently reading this
        private bool canShowRead = true;
        public bool CanShowRead {
            get { return canShowRead; }
            set { SetProperty(ref canShowRead, value); }
        }
    }
}
