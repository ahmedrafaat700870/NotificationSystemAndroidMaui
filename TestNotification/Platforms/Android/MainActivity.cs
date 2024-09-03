using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;
using System.Runtime.CompilerServices;
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
    private Bundle _bundel;
    protected override bool AllowFragmentRestore => true;
    public MainActivity()
    {
        AndroidServiceManager.MainActivity = this;
    }

    public void ShowIncomingCallNotification(string title , string content)
    {
        Colling colling = new Colling(this);
        colling.ShowIncomingCallNotification(title, content, typeof(MainActivity));
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

  
    protected override void OnCreate(Bundle? savedInstanceState)
    {

        _bundel = savedInstanceState;
        try
        {
            bool isServicesRunning = IsServiceRunning(typeof(MyBackgroundService));
            if (isServicesRunning)
            {
                ClearAppData();
            }
            else
            {
                _StartService();
            }

            LocalNotificationCenter.Current.NotificationActionTapped += OnNotificationActionTapped;

        }
        catch (Exception e)
        {

        }
        base.OnCreate(savedInstanceState);
    }

    private void OnNotificationActionTapped(NotificationActionEventArgs e)
    {
        if (e.IsTapped)
        {
        }
        else if(e.IsDismissed)
        {
        }
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

    public void _StartService()
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

}
