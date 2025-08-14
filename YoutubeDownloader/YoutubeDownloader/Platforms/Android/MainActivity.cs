using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Provider;

namespace YoutubeDownloader
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        public const int RequestCodeDownloadFolder = 1001;

        public static bool HasAllFilesAccess() =>
            Build.VERSION.SdkInt >= BuildVersionCodes.R &&
            Android.OS.Environment.IsExternalStorageManager;

        public static void RequestAllFilesAccess()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.R) return;
            var ctx = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity!;
            try
            {
                var intent = new Intent(Settings.ActionManageAllFilesAccessPermission);
                intent.SetData(Android.Net.Uri.Parse($"package:{ctx.PackageName}"));
                ctx.StartActivity(intent);
            }
            catch
            {
                ctx.StartActivity(new Intent(Settings.ActionManageAllFilesAccessPermission));
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (!HasAllFilesAccess())
            {
                RequestAllFilesAccess();
            }

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel(
                    "downloader_channel",
                    "Downloads",
                    NotificationImportance.Low
                );
                var mgr = (NotificationManager?)GetSystemService(NotificationService);
                mgr?.CreateNotificationChannel(channel);
            }
            // Получаем список сохранённых (persisted) разрешений через ContentResolver
            //var persistedUriPermissions = ContentResolver.PersistedUriPermissions;
            //// Проверяем, есть ли разрешение на запись
            //if (!persistedUriPermissions.Any(permission => permission.IsWritePermission))
            //{
            //    // Если разрешение отсутствует, запускаем диалог выбора папки
            //    Intent intent = new Intent(Intent.ActionOpenDocumentTree);
            //    intent.AddFlags(ActivityFlags.GrantReadUriPermission |
            //                    ActivityFlags.GrantWriteUriPermission |
            //                    ActivityFlags.GrantPersistableUriPermission);
            //    StartActivityForResult(intent, RequestCodeDownloadFolder);
            //}
            //else
            //{
            //    System.Diagnostics.Debug.WriteLine("Доступ к папке Downloads уже предоставлен");
            //    // Здесь можно сразу использовать ранее полученный URI
            //}
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == RequestCodeDownloadFolder && resultCode == Result.Ok)
            {
                var treeUri = data?.Data;
                if (treeUri != null)
                {
                    // Сохраняем персистентное разрешение для доступа к выбранной папке
                    ContentResolver.TakePersistableUriPermission(treeUri,
                        data.Flags & (ActivityFlags.GrantReadUriPermission | ActivityFlags.GrantWriteUriPermission));
                    System.Diagnostics.Debug.WriteLine("Доступ к папке Downloads получен и сохранён");
                }
            }
        }
    }
}
