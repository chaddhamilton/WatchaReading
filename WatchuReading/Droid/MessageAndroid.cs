using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.Design.Widget;
using Android.OS;
using Xamarin.Forms;
using Plugin.CurrentActivity;
using WatchuReading.Interfaces;
using WatchuReading.Droid;


[assembly :Xamarin.Forms.Dependency(typeof(MessageAndroid))]

namespace WatchuReading.Droid
{
    public class MessageAndroid:IMessage
    {

        public void LongAlert(string message)
        {
            Activity activity = CrossCurrentActivity.Current.Activity;
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long);
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short);
        }

        public void ShowSnackbar(string message)
        {
            Activity activity = CrossCurrentActivity.Current.Activity;
            Android.Views.View activityRootView = activity.FindViewById(Android.Resource.Id.Content);
            Snackbar.Make(activityRootView,message,  Snackbar.LengthLong).Show();

        }
    }
}
