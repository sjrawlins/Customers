using Android.App;
using Android.OS;
using Android.Views;
using iFactr.Core;
using iFactr.Droid;

namespace Customers.Droid
{
    [Activity(MainLauncher = true, WindowSoftInputMode = SoftInput.AdjustPan)]
    public class MainActivity : iFactrActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            if (!DroidFactory.IsInitialized)
            {
                DroidFactory.MainActivity = this;
                iApp.OnLayerLoadComplete += layer => DroidFactory.Instance.OutputLayer(layer);

                //Instantiate your iFactr application and set the Factory App property
                DroidFactory.TheApp = new Customers.MyApp();
                iApp.Navigate(iApp.Instance.NavigateOnLoad);
            }
            else DroidFactory.MainActivity = this;
            base.OnCreate(bundle);
        }
    }
}