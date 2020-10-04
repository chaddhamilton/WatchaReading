using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace WatchuReading.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

           // Rg.Plugins.Popup.IOS.Popup.Init();

            return base.FinishedLaunching(app, options);
        }
    }
}
