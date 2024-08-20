using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;
using TestNotification.Platforms.Android;

namespace TestNotification;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true,
    LaunchMode = LaunchMode.SingleTop,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation
    | ConfigChanges.UiMode | ConfigChanges.ScreenLayout
    | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    public static MainActivity lastActivty;
    protected override bool AllowFragmentRestore => true;
    public MainActivity()
    {
        AndroidServiceManager.MainActivity = this;
    }


    protected override void OnDestroy()
    {
        
        bool isServicesRunning = IsServiceRunning(typeof(MyBackgroundService));
        if (isServicesRunning)
            lastActivty = this;
        base.OnDestroy();
    }

    // Method to clear all application data
    private void ClearAppData()
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
        {
            // Clear all user data
            (GetSystemService(Context.ActivityService) as ActivityManager)?.ClearApplicationUserData();
        }
        else
        {
            // Handle older versions if necessary
            // Alternatively, clear specific data (SharedPreferences, files, etc.)
        }
    }
    public void RestartApp()
    {
        // Get the context
        // Get the application context
        var context = Android.App.Application.Context;

        // Get the package manager and launch intent
        var packageManager = context.PackageManager;
        var intent = packageManager.GetLaunchIntentForPackage(context.PackageName);

        if (intent != null)
        {
            // Create a pending intent to restart the app
            var pendingIntent = PendingIntent.GetActivity(
                context,
                0,
                intent,
                PendingIntentFlags.Immutable | PendingIntentFlags.CancelCurrent
            );

            // Schedule the restart using SystemClock.ElapsedRealtime() for accurate timing
            var alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);
            alarmManager.Set(
                AlarmType.ElapsedRealtime,
                SystemClock.ElapsedRealtime() + 1000, // Reopen after 1 second
                pendingIntent
            );

            // Close the current app
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        try
        {
            bool isServicesRunning = IsServiceRunning(typeof(MyBackgroundService));
            if (isServicesRunning)
            {
                //ClearAppData();

                // Optionally restart the app after clearing data
                RestartApp();

                /* 
                 StopService();
                 StopApp();
                 StartNewInstance();
                 */
            } else
            {
                StartService();
            }

            LocalNotificationCenter.Current.NotificationActionTapped += OnNotificationActionTapped;
            base.OnCreate(savedInstanceState);
        }
        catch (Exception e)
        {

        }
    }

    private void OnNotificationActionTapped(NotificationActionEventArgs e)
    {
        // Handle the notification action, e.g., navigate to a specific page
        if (e.IsTapped)
        {
            // Navigate to chat or any specific page
            NavigateToChatPage();
        }
        else if(e.IsDismissed)
        {
           // hendel dismissed
        }
    }
    private void NavigateToChatPage()
    {
        Device.BeginInvokeOnMainThread(async () =>
        {
            // naivgate to chat page .
            //await ;
        });
    }
    protected override void OnNewIntent(Intent intent)
  {
        base.OnNewIntent(intent);
        // Handle the intent that you received
        ProcessIntent(intent);
  }

    private void ProcessIntent(Intent intent)
    {
        // Extract data from the intent and use it
        // For example, you can check for a specific action or extract extras
        if (intent != null)
        {
            // Example: checking for a specific action
            var action = intent.Action;
            if (action == "USER_TAPPED_NOTIFIACTION")
            {
                // Handle the specific action
            }
        }
    }

    public void StartService()
    {
        bool isServicesRunning = IsServiceRunning(typeof(MyBackgroundService));
        if (!isServicesRunning)
        {
            var serviceIntent = new Intent(this, typeof(MyBackgroundService));
            serviceIntent.PutExtra("inputExtra", "Background Service");

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                StartForegroundService(serviceIntent); // For API level 26 and above
            }
            else
            {
                StartService(serviceIntent); // For older versions
            }
        }
      
    }
    private bool IsServiceRunning(Type serviceClass)
    {
        ActivityManager manager = (ActivityManager)GetSystemService(ActivityService);
        var servicesRunning = manager.GetRunningServices(int.MaxValue);
        foreach (var service in servicesRunning)
        {
            string packgeName = "com.companyname.testnotification";
            string currentServices = service.Service.PackageName;
            if (packgeName == currentServices)
                return true;
           /* if (serviceClass.Name.Contains(service.Service.ClassName))
            {
                return true;
            }*/
        }
        return false;
    }
    public void StopService()
    {
        var serviceIntent = new Intent(this, typeof(MyBackgroundService));
        StopService(serviceIntent);
        //RestartApp();
    }
    private void StopApp()
    {
        // Create an intent to stop the application
        var packageManager = PackageManager;
        var intent = packageManager.GetLaunchIntentForPackage(PackageName);

        if (intent != null)
        {
            // Use a pending intent to restart the app after stopping it
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.CancelCurrent);

            // Use the AlarmManager to schedule the restart
            var alarmManager = (AlarmManager)GetSystemService(AlarmService);
            alarmManager.Set(AlarmType.Rtc, Java.Lang.JavaSystem.CurrentTimeMillis() + 1000, pendingIntent);

            // Finish the current activity
            FinishAffinity();
        }
    }
    private void StartNewInstance()
    {
        // Create an intent to start the new instance
        var intent = new Intent(this, typeof(MainActivity));
        intent.AddFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
        StartActivity(intent);
    }
}
