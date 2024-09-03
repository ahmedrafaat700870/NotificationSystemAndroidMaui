
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using AndroidX.Core.App;

namespace TestNotification.Platforms.Android
{
    public class Colling
    {
        public const string CHANNEL_ID = "incoming_calls_channel";
        private readonly Context _context;

        public Colling(Context context)
        {
            _context = context;
        }

        public void ShowIncomingCallNotification(string title, string content, Type activityType)
        {
            // Create the notification channel (if needed)
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var ringtoneUri = RingtoneManager.GetDefaultUri(RingtoneType.Ringtone);
                var channel = new NotificationChannel(CHANNEL_ID, "Incoming Calls", NotificationImportance.High)
                {
                    Description = "Incoming Call Notifications"
                };
                channel.SetSound(ringtoneUri, new AudioAttributes.Builder()
                    .SetUsage(AudioUsageKind.NotificationRingtone)
                    .SetContentType(AudioContentType.Sonification)
                    .Build());

                var notificationManager = (NotificationManager)_context.GetSystemService(Context.NotificationService);
                notificationManager.CreateNotificationChannel(channel);
            }

            // Create the intent that will start the incoming call activity
            var intent = new Intent(_context, activityType);
            intent.SetFlags(ActivityFlags.NoUserAction | ActivityFlags.NewTask);

            var pendingIntent = PendingIntent.GetActivity(_context, 0, intent, PendingIntentFlags.Mutable);

            // Create intents for the accept and reject actions
            var acceptIntent = new Intent(_context, typeof(CallActionReceiver));
            acceptIntent.SetAction("ACCEPT_CALL");
            var acceptPendingIntent = PendingIntent.GetBroadcast(_context, 1, acceptIntent, PendingIntentFlags.Mutable);

            var rejectIntent = new Intent(_context, typeof(CallActionReceiver));
            rejectIntent.SetAction("REJECT_CALL");
            var rejectPendingIntent = PendingIntent.GetBroadcast(_context, 2, rejectIntent, PendingIntentFlags.Mutable);

            // Build the notification
            var builder = new NotificationCompat.Builder(_context, CHANNEL_ID)
                .SetOngoing(true)
                .SetPriority((int)NotificationPriority.High)
                .SetSmallIcon(Resource.Drawable.abc_action_bar_item_background_material) // Replace with your app's icon resource
                .SetContentTitle(title)
                .SetContentText(content)
                .SetContentIntent(pendingIntent)
                .SetFullScreenIntent(pendingIntent, true)
                .AddAction(Resource.Drawable.abc_ab_share_pack_mtrl_alpha, "Accept", acceptPendingIntent) // Replace with your icon
                .AddAction(Resource.Drawable.abc_ab_share_pack_mtrl_alpha, "Reject", rejectPendingIntent); // Replace with your icon

            var notification = builder.Build();
            notification.Flags |= NotificationFlags.Insistent;

            // Show the notification
            var notificationManagerCompat = NotificationManagerCompat.From(_context);
            notificationManagerCompat.Notify(1, notification);
        }
    }
}

