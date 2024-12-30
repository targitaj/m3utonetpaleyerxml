using Android.Content;
using Android.App;
using Android.OS;
using AndroidX.Core.Content;

[BroadcastReceiver(Enabled = true, Exported = true)]
[IntentFilter(new[] { Intent.ActionBootCompleted })]
public class BootCompletedReceiver : BroadcastReceiver
{
    public override void OnReceive(Context context, Intent intent)
    {
        if (intent.Action == Intent.ActionBootCompleted)
        {
            Intent startServiceIntent = new Intent(context, typeof(MyService));
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                ContextCompat.StartForegroundService(context, startServiceIntent);
            }
            else
            {
                context.StartService(startServiceIntent);
            }
        }
    }
}