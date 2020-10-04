using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using WatchuReading.Interfaces;
using WhatchaReading.Models;
using WatchuReading.Services;
using WatchuReading.Views;
using Xamarin.Forms;

namespace WatchuReading.ViewModels
{
    public class ActivityDetailViewModel : BaseViewModel
    {
        public Command SaveBookCommand { get; set; }
        public Command StartReadingCommand { get; }

        #region Constructors

        public ActivityDetailViewModel(Activity act)
        {
            Title = "Your Review Details";
            Activity = act;
            SaveBookCommand = new Command(async () => await SaveActivityDetails()); 
            StartReadingCommand = new Command(async (obj) => await StartReading(obj));

        }

        #endregion


        #region PRIVATE METHODS

        private async Task SaveActivityDetails()
        {
            IsBusy = true;
            var msg = "";

            _manager = new ServiceManager();
            try
            {
                await _manager.UpdateActivity(Activity);
                msg = "Details Saved!";

            }
            catch (Exception ex)
            {
                //TODO handle network connection here versus service
                msg = Constants.UpdateErrorMsg;
            }
            finally
            {
                DependencyService.Get<IMessage>().ShowSnackbar(msg);
                IsBusy = false;
                await Application.Current.MainPage.Navigation.PopAsync();
             }
        }

        protected internal async Task StartReading(object obj)
        {
            var book = (Book)obj;
            _manager = new ServiceManager();
            var act = new Activity()
            {
                Book = book,
                IsReading = true,
                UserId = UserId

            };
            act.IsReading = true;
            await _manager.AddActivity(act);
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        #endregion


        #region PROPERTIES

        private Activity activity;
        public Activity Activity
        {
            get { return activity; }
            set { SetProperty(ref activity, value); }
        }


        #endregion  
    }
}





