using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WatchuReading.Interfaces;
using WhatchaReading.Models;
using WatchuReading.Services;
using WatchuReading.Views;
using Xamarin.Forms;
using static WatchuReading.Enums;

namespace WatchuReading.ViewModels
{
    public class ActivityListViewModel : BaseViewModel
    {
        public Command LoadItemsCommand;
        public Command SelectedActivityCommand { get; }
        public Command MarkAsReadCommand { get; }
        public Command SortCommand { get; }
        public Command FavoriteCommand { get; }
        public Command FilterCommand { get; }
        public Command ViewBookInfoCommand { get;  }
        public Command OtheReadersCommand { get; } 

        private Page _page;
        private CustomObservableCollection<Activity> memList;

        public ActivityListViewModel(Page page)
        {
            Title = $"{DispName} Bookshelf";
            Activities = new CustomObservableCollection<Activity>();
            LoadItemsCommand = new Command(async () => await LoadCollectionForUser());
            SelectedActivityCommand = new Command<Activity>(ShowMenu);
            MarkAsReadCommand = new Command<Activity>(async (Activity obj) => await SetBookAsRead(obj));
            SortCommand = new Command((obj) => DoSort(obj));
            FavoriteCommand = new Command<Activity>(async (Activity obj) => await SetAsFavorite(obj));
            FilterCommand = new Command(async () => await FilterFavorites());
            ViewBookInfoCommand = new Command<Activity>((Activity obj) => GoToBookDetails(obj));
            OtheReadersCommand = new Command<Activity>(async (Activity obj) => await ShowWhoIsReading(obj));
            _page = page;

        }

        private async Task ShowWhoIsReading(Activity act)
        {
            //Show Modal
            await _page.Navigation.PushModalAsync(new BookReviewsPage(BookAction.Comments, act.Book));
        }

        private async Task FilterFavorites()
        {
            if(memList==null){
                memList = Activities;
            }
            
            ShowFavorites = !ShowFavorites;
            if (ShowFavorites)
            {
                Activities = new CustomObservableCollection<Activity>(memList.Where(e => e.IsFavorite == true));
            }
            else
            {
                await LoadCollectionForUser();
                memList = null;
            }

            Title = Title.Swap();
        }

        private void DoSort(object obj)
        {

            var sort = (ActivitySort)Enum.Parse(typeof(ActivitySort), obj.ToString());
            switch (sort)
            {
                case ActivitySort.Title:
                    Activities = new CustomObservableCollection<Activity>(Activities.OrderBy(e => e.IsReading ? 0 : 1).ThenBy(e => e.Book.Title));
                    break;
                case ActivitySort.Author:
                    Activities = new CustomObservableCollection<Activity>(Activities.OrderBy(e => e.IsReading ? 0 : 1).ThenBy(e => e.Book.Author));
                    break;
                case ActivitySort.AddDate:
                    Activities = new CustomObservableCollection<Activity>(Activities.OrderBy(e => e.IsReading ? 0 : 1).ThenBy(e => e.AddDate)); break;

            }
        }

        #region COMMANDS
        //EDIT
        public ICommand EditActivityCommand
        {
            get
            {
                return new Command((e) =>
                {
                    var item = (e as Activity);
                    Application.Current.MainPage.Navigation.PushAsync(new ActivityDetailPage(item));
                });
            }
        }

        //WHO IS READING BOOK -maybe show a modal? TODO
        public ICommand WhoIsReadingCommand
        {
            get
            {
                return new Command((e) =>
                {
                    var item = (e as Activity);
                    // Application.Current.MainPage.Navigation.PushAsync(new BookListPage(item));
                });
            }
        }
    #endregion

        private void ShowMenu(Activity act)
        {
            if (ShowLineItemMenu)
            {
                //RESET ANY OTHER ITEMS
                foreach (Activity a in Activities)
                {
                    a.ShowMenu = false;
                    Activities.ReportItemChange(a);
                }

                act.ShowMenu = true;
                Activities.ReportItemChange(act);
            }
            else
            {
                GoToBookDetails(act);
            }

        }

        private void GoToBookDetails(Activity act)
        {
              
                Application.Current.MainPage.Navigation.PushAsync(new BookDetailsPage(act.Book));
        }

        //LIST LOAD
        public async Task LoadCollectionForUser()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            ServiceManager _man = new ServiceManager();

            try
            {
                Activities.Clear();
                //CURRENT USER
                if(AltUserId==0){
                    Activities = await _man.GetAllBooksByUser(UserId);
                }
                else//Another App User
                {
                    Activities = await _man.GetAllBooksByUser(AltUserId, true);
                    Title = $"{DispName} Bookshelf";
                    ShowLineItemMenu = false; //disable the submenu

                }

            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                if(Activities?.Count==0)
                {
                
                }
                IsBusy = false;


            }
        }

        //Set Book As a Favorite
        public async Task SetAsFavorite(object e)
        {
            IsBusy = true;

            var item = (e as Activity);
            var yn = item.IsFavorite;

            var msg = string.Empty;
            try
            {
                item.IsFavorite = !yn;
                var axun = item.IsFavorite ? "Added to" : "Removed from";
                _manager = new ServiceManager();
                var ret = await _manager.UpdateActivity(item);
                msg = $"{item.Book.Title} has been {axun} your Favorites.";

            }
            catch (Exception ex)
            {
                //TODO handle network connection here versus service
                msg = "Hmm, looks like we hit a problem.  Try again later.";

            }
            finally
            {
                DependencyService.Get<IMessage>().ShowSnackbar(msg);
                IsBusy = false;
                ShowMenu(item);
            }

        }

        //Set Book As being Read
        public async Task SetBookAsRead(object e)
        {
            var item = (e as Activity);
            item.IsReading = false;
            var msg =string.Empty;

            var rez = await _page.DisplayAlert("", $"Do you want to review and rate this?", "Yep", "Nope");

            if (rez)
            {
                await Application.Current.MainPage.Navigation.PushModalAsync(new ActivityDetailPage(item));
            }
            else
            {
                IsBusy = true;
               
                try
                {
                    _manager = new ServiceManager();
                    var ret = await  _manager.UpdateActivity(item);

                    msg = $"{item.Book.Title} has been marked as read. Be sure to rate and review when you get time.";

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
                    ShowMenu(item);
                }

            }
        }

        #region Properties 
        //For use when viewing another user's reading history list
        private int altUserId = 0;
        public int AltUserId
        {
            get { return altUserId; }
            set {SetProperty(ref altUserId, value);}
        }

        //For use when viewing another user's reading history list
        private string dispName = "Your";
        public string DispName
        {
            get { return dispName; }
            set { SetProperty(ref dispName, value); }
        }

        //For use when viewing another user's reading history list
        private bool showLineItemMenu=true;
        public bool ShowLineItemMenu
        {
            get { return showLineItemMenu; }
            set { SetProperty(ref showLineItemMenu, value); }
        }

        //Favorites Filtering
        private bool showFavorites = false;
        public bool ShowFavorites
        {
            get { return showFavorites; }
            set { SetProperty(ref showFavorites, value); }
        }


       
        #endregion
    }
}
