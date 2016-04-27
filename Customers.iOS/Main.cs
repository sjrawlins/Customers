
using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;

using iFactr.Core;
using iFactr.Touch;

namespace Customers.iOS
{
    public class Application
    {
        static void Main(string[] args)
        {
            UIApplication.Main(args, null, "AppDelegate");
        }
    }

    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            TouchFactory.Initialize();

            //Instantiate your iFactr application and set the Factory App property
            TouchFactory.TheApp = new Customers.MyApp();

            iApp.Navigate(TouchFactory.TheApp.NavigateOnLoad);

            return true;
        }

        public override void OnActivated(UIApplication application)
        {
        }
    }
}
