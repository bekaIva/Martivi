using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Martivi.Droid.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(AlertNativeDependency))]
namespace Martivi.Droid.DependencyServices
{
    class AlertNativeDependency : IAlertNative
    {
        public void ShowToast(string message)
        {
            Toast.MakeText(MainActivity.Instance, message, ToastLength.Short).Show();
        }
        public void ShowDialog(string v1, string message, string v2)
        {

            try
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(MainActivity.Instance);
                builder.SetMessage(message).SetPositiveButton(v2, new EventHandler<DialogClickEventArgs>((arg1, arg2) => { })).SetTitle(v1);

                Dialog d = builder.Create();
                d.Show();



            }
            catch (Exception e)
            {

            }
        }

        public void ShowDialog(string title, string message, string positive, string negative, Action<string> action)
        {

            AlertDialog.Builder alert = new AlertDialog.Builder(MainActivity.Instance);
            alert.SetTitle(title);
            alert.SetMessage(message);
            alert.SetPositiveButton(positive, (senderAlert, args) =>
            {
                action("positive");
                //Toast.MakeText(MainActivity.Activity, "Deleted!", ToastLength.Short).Show();
            });
            alert.SetNegativeButton(negative, (senderAlert, args) =>
            {
                action("negative");
                //Toast.MakeText(MainActivity.Activity, "Deleted!", ToastLength.Short).Show();
            });
            Dialog dialog = alert.Create();
            dialog.Show();
        }
    }
}