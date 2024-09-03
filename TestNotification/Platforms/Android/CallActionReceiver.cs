
using Android.Content;
using AndroidX.Core.App;

namespace TestNotification.Platforms.Android;
[BroadcastReceiver(Enabled = true, Exported = false)]
public class CallActionReceiver : BroadcastReceiver
{
    public override void OnReceive(Context context, Intent intent)
    {
        string action = intent.Action;
        var notificationManagerCompat = NotificationManagerCompat.From(context);
        if (action == "ACCEPT_CALL")
        {
            // Handle accepting the call
            notificationManagerCompat.Cancel(1);
        }
        else if (action == "REJECT_CALL")
        {
            // Handle rejecting the call
            notificationManagerCompat.Cancel(1);
        }
    }
}
