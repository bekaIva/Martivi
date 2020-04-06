using Martivi.Pages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Martivi
{
    public partial class App : Application
    {
        public static string BaseImageUrl { get; } = "https://cdn.syncfusion.com/essential-ui-kit-for-xamarin.forms/common/uikitimages/";
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjM0MzYxQDMxMzgyZTMxMmUzMGhib01pcWd3QnJzSWJvcThKYUhVejJQOG1xbUpQSmp2eTg5UEY2Yml3UXc9");
            InitializeComponent();
            MainPage = new MainShell();
            //MainPage = new NavigationPage(new SignInPage());
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
