using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;

namespace Martivi.Droid
{
    [Activity(Label ="Martivi", Theme = "@style/splashscreen", Icon = "@mipmap/icon", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
        }

        // Launches the startup task

        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(typeof(MainActivity));
        }

        // Prevent the back button from canceling the startup process
        public override void OnBackPressed() { }

        // Simulates background work that happens behind the splash screen
      
    }
}