using System;
namespace WatchuReading.Droid
{
    public class AndroidDialog
    {
       Dialog  
        public AndroidDialog()
        {
        }

        public void StartLoading()
        {
            dialog = new Dialog(Xamarin.Forms.Forms.Context);
            dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
            dialog.SetCancelable(false);
            dialog.SetCanceledOnTouchOutside(false);
            Window window = dialog.Window;
            window.SetLayout(WindowManagerLayoutParams.MatchParent, WindowManagerLayoutParams.MatchParent);
            window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));

            var progress = new ProgressBar(Xamarin.Forms.Forms.Context);
            progress.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            progress.Indeterminate = true;
            dialog.AddContentView(progress, progress.LayoutParameters);

            dialog.Show();
        }

        public void StopLoading()
        {
            dialog.Dismiss();
        }
    }
}


