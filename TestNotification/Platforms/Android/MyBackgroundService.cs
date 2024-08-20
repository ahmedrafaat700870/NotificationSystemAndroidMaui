using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using Plugin.LocalNotification;
namespace TestNotification.Platforms.Android;

[Service]
internal class MyBackgroundService : Service
{
    Timer timer = null;
    int myId = (new object()).GetHashCode();
    int BadgeNumber = 0;
    private readonly IBinder binder = new LocalBinder();

    public class LocalBinder : Binder
    {
        public MyBackgroundService GetService()
        {
            return this.GetService();
        }
    }

    public override IBinder OnBind(Intent intent)
    {
        return binder;
    }

    public override StartCommandResult OnStartCommand(Intent intent,
        StartCommandFlags flags, int startId)
    {
        var input = intent.GetStringExtra("inputExtra");

        var notificationIntent = new Intent(this, typeof(MainActivity));
        notificationIntent.SetAction("USER_TAPPED_NOTIFIACTION");

        var pendingIntent = PendingIntent.GetActivity(this, 0, notificationIntent,
            PendingIntentFlags.UpdateCurrent);

        var notification = new NotificationCompat.Builder(this,
                MainApplication.ChannelId)
            .SetContentText(input)
            .SetSmallIcon(Resource.Drawable.AppIcon)
            .SetContentIntent(pendingIntent);

        StartForeground(myId, notification.Build());

        timer = new Timer(Timer_Elapsed, notification, 0, 10000);

        // You can stop the service from inside the service by calling StopSelf();

        return StartCommandResult.Sticky;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="state"></param>
    void Timer_Elapsed(object state)
    {
        AndroidServiceManager.IsRunning = true;
        BadgeNumber++;
        string timeString = $"Time: {DateTime.Now.ToLongTimeString()}";

        var notification = new NotificationRequest
        {
            NotificationId = BadgeNumber,
            Title = "Background Task",
            Description = $"Task executed at {DateTime.Now:T}",
            BadgeNumber = BadgeNumber,
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = DateTime.Now.AddSeconds(1)
            }
        };

        LocalNotificationCenter.Current.Show(notification);
    }

    public override void OnTaskRemoved(Intent? rootIntent)
    {
        //StopSelf();
        base.OnTaskRemoved(rootIntent);
    }

    public override void OnDestroy()
    {
        timer?.Dispose();
        base.OnDestroy();
    }
}