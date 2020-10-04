using System;
using System.Threading.Tasks;
using WhatchaReading.Models;
using WatchuReading.Views;
using WatchuReading.Services;
using Xamarin.Forms;
using WatchuReading.Interfaces;

namespace WatchuReading.ViewModels
{
    public class UserHomeViewModel : BaseViewModel
    {

        public Command MyBookshelfCommand { get; set; }
        public Command AllBooksCommand { get; set; }
        public Command LoadActiveBooksCommand { get; }
        public Command FinishedCommand { get; }
        public Command ReadingListCommand { get; }
        public Command RefreshCommand { get; }
        private Page _page;

        public UserHomeViewModel(Page page)
        {
            Title = $"Hi {FirstName}";
            MyBookshelfCommand = new Command(async () => await App.Current.MainPage.Navigation.PushAsync(new ActivityListPage()));
            AllBooksCommand = new Command(async () => await App.Current.MainPage.Navigation.PushAsync(new BookListPage()));
            LoadActiveBooksCommand = new Command(async () => await LoadBook());
            FinishedCommand = new Command(async () => await SetBookAsRead());
            ReadingListCommand = new Command(async () => await App.Current.MainPage.Navigation.PushAsync(new AllActivityListPage()));
            RefreshCommand = new Command(async () =>  await RefreshCache());
            _page = page;

#if Debug
            IsDebug = true;
#endif
        }


        private async Task RefreshCache()
        {
            await _manager.ResetAll();
            await LoadBook();

            DependencyService.Get<IMessage>().ShowSnackbar("Cache has been refreshed");

        }

        private async Task SetBookAsRead()
        {
            var item = ActiveBook;
            item.IsReading = false;
            var msg = string.Empty;
            ShowFinished = false;

            var rez = await _page.DisplayAlert("", $"Do you want to review and rate this book?", "Yep", "Nope");

            if (rez)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new ActivityDetailPage(item));
            }
            else
            {
                IsBusy = true;

                try
                {
                    _manager = new ServiceManager();
                    var ret = await _manager.UpdateActivity(item);
                    msg = $"{item.Book.Title} has been marked as read. Be sure to rate and review when you get time.";
                    ActiveBook = null;
                   

                }
                catch (Exception ex)
                {
                    //TODO handle network connection here versus service
                    msg = "Hmm, looks like we hit a problem.  Try again later.";
                    item.IsReading = true;
                }
                finally
                {
                    DependencyService.Get<IMessage>().ShowSnackbar(msg);
                    IsBusy = false;
                    await LoadBook();
                }
            }
        }

        //Load Active Book
        private async Task LoadBook()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                _manager = new ServiceManager();
                ActiveBook = await _manager.GetActiveBookByUser(UserId);

                if (ActiveBook != null)
                {
                    ActivityStatusHeader = "You're currently reading:";
                    ShowFinished = true;
                }
                else
                {
                    ActivityStatusHeader = "You're not reading anything.";
                    ShowFinished = false;

                }

                ShowStartReading = ActiveBook ==null;    
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

        private string _activityStatusHeader;
        public string ActivityStatusHeader
        {
            get { return _activityStatusHeader; }
            set { SetProperty(ref _activityStatusHeader, value); }
        }

        private Activity _activeBook;
        public Activity ActiveBook
        {
            get { return _activeBook; }
            set { SetProperty(ref _activeBook, value); }
        }

        private bool showStartReading = false;
        public bool ShowStartReading{

            get { return showStartReading; }
            set{ SetProperty(ref showStartReading, value);}
        }


        private bool showFinished = false;
        public bool ShowFinished{

            get { return showFinished; }
            set{ SetProperty(ref showFinished, value);}
        }

        private bool isDebug = false;
        public bool IsDebug
        {
            get { return isDebug; }
            set{ SetProperty(ref isDebug, value);}
        }



    }
}
