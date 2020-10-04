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


namespace WatchuReading.ViewModels
{
    public class AllActivityViewModel : BaseViewModel
    {
        public Command LoadListCommand { get; }
        public Command SelectedActivityCommand { get; }
        public Command ViewUserBooksCommand { get; set; }
        public Command StartReadingCommand { get; }


        public AllActivityViewModel()
        {
            LoadListCommand = new Command(async () => await LoadUsersActivityList());
            SelectedActivityCommand = new Command<Activity>(ShowMenu);
            Title = "Who's Reading What";
            ViewUserBooksCommand = new Command(async (obj) => await Application.Current.MainPage.Navigation.PushAsync(new ActivityListPage(((Activity)obj))));
            StartReadingCommand = new Command(async (obj) => await StartReading(obj));
        }

        //load up our list
        private async Task LoadUsersActivityList()
        {
            IsBusy = true;
            _manager = new ServiceManager();
            Activities = await _manager.GetActiveBooksByAllUsers(UserId);
            IsBusy = false;

        }

        private async Task StartReading(object o)
        {
            _manager = new ServiceManager();
            var act = o as Activity;
            act.UserId = UserId;
            var rez = await App.Current.MainPage.DisplayAlert("Hold Up", $"This will now be your active book. Are you sure?", "Yep", "Nope");
            if (rez)
            {
                act.IsReading = true;
                try
                {
                    await _manager.AddActivity(act);
                    DependencyService.Get<IMessage>().ShowSnackbar($"You are now reading {act.Book.Title}. ");

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
        }

        private void ShowMenu(Activity act)
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


        #region Properties 


        #endregion
    }
}
