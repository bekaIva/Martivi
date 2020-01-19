using Martivi.Pages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Martivi
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTk3MTE5QDMxMzcyZTM0MmUzMG1IdW5xQ3E1L2xxeFl5c1hVMlZ2T2p4UktmdDFkT3RKNWxxZnl1bkNiSGs9");
            InitializeComponent();

            MainPage = new NavigationPage(new HomePage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
