using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using WatchuReading.Interfaces;
using WhatchaReading.Models;
using WatchuReading.Services;
using WatchuReading.Views;
using Xamarin.Forms;

namespace WatchuReading
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public static ISettings AppSettings => CrossSettings.Current;
        public IDataStore<Book> BookData => DependencyService.Get<IDataStore<Book>>();
        public ServiceManager _manager { get; set; }

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { this.SetProperty(ref title, value); }
        }

      
        private CustomObservableCollection<Activity> activities; //READING ACTIVITIES
        public CustomObservableCollection<Activity> Activities
        {
            get { return activities; }
            set { SetProperty(ref activities, value); }
        }

        private CustomObservableCollection<Book> library; //Library
        public CustomObservableCollection<Book> Library
        {
            get { return library; }
            set { SetProperty(ref library, value); }
        }
      
        //PLUGIN SETTING PROPS
        public static int UserId //ID
        {
            get => AppSettings.GetValueOrDefault(Constants.UserIdKey, 0);
            set => AppSettings.AddOrUpdateValue(Constants.UserIdKey, value);
        }

        public static string Logon //UserName
        {
            get => AppSettings.GetValueOrDefault(Constants.LogonKey, String.Empty);
            set => AppSettings.AddOrUpdateValue(Constants.LogonKey, value);
        }

        public static string FirstName //FirstName
        {
            get => AppSettings.GetValueOrDefault(Constants.FirstNameKey, string.Empty);
            set => AppSettings.AddOrUpdateValue(Constants.FirstNameKey, value);
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        private Color activeRowColor = Color.White;
        public Color ActiveRowColor
        {
            get { return activeRowColor; }
            set { SetProperty(ref activeRowColor, value); }
        }


        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
