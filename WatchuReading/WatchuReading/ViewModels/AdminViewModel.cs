using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace WatchuReading
{
    public class AdminViewModel : BaseViewModel
    {
        public AdminViewModel()
        {
            Title = "About";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));
        }

        public ICommand OpenWebCommand { get; }
    }
}
