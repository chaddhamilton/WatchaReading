using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
 
[assembly: ExportRenderer(typeof(Button), typeof(WatchuReading.Droid.FlatButtonRenderer))]
namespace WatchuReading.Droid
{
    public class FlatButtonRenderer : ButtonRenderer
    {
        protected override void OnDraw(Android.Graphics.Canvas canvas)
        {
            base.OnDraw(canvas);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
        }
    }
}