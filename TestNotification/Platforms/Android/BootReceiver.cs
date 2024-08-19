using Android.App;
using Android.Content;
using Android.OS;


namespace TestNotification.Platforms.Android;
[BroadcastReceiver(Enabled = true, Exported = true, DirectBootAware = true)]
[IntentFilter(new[] { Intent.ActionBootCompleted })]
public class BootReceiver : BroadcastReceiver
{
    public override void OnReceive(Context context, Intent intent)
    {
        /* if (intent.Action == Intent.ActionBootCompleted)
         {
             Toast.MakeText(context, "Boot completed event received",
                 ToastLength.Short).Show();

             var serviceIntent = new Intent(context,
                 typeof(MyBackgroundService));

             ContextCompat.StartForegroundService(context,
                 serviceIntent);
         }*/
        if (intent.Action.Equals(Intent.ActionBootCompleted))
        {
            var serviceIntent = new Intent(context, typeof(MyBackgroundService));
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                context.StartForegroundService(serviceIntent);
            }
            else
            {
                context.StartService(serviceIntent);
            }
        }
    }
}