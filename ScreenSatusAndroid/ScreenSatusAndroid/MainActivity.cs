using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using AndroidX.AppCompat.App;
using System;

namespace ScreenSatusAndroid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        public static Intent startIntent;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                Android.Util.Log.Error("CRASH", e.ExceptionObject.ToString());
            };

            AndroidEnvironment.UnhandledExceptionRaiser += (s, e) =>
            {
                Log.Error("CRASH", e.Exception.ToString());
                e.Handled = true;
            };

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            // Запуск сервиса
            if (startIntent == null)
            {
                startIntent = new Intent(this, typeof(MyService));
                StartService(startIntent);
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}