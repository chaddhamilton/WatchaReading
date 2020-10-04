using System;
using System.Collections.Generic;
using WhatchaReading.Models;
using WatchuReading.ViewModels;
using WatchuReading.Views;
using Xamarin.Forms;

namespace WatchuReading.Views
{
    public partial class ActivityDetailPage : ContentPage
    {
        ActivityDetailViewModel vm;

        public ActivityDetailPage(Activity act)
        {
            BindingContext = vm = new ActivityDetailViewModel(act);
            InitializeComponent();
        }

        public new ActivityDetailViewModel ViewModel
        {
            get { return BindingContext as ActivityDetailViewModel; }
        }


    }
}
