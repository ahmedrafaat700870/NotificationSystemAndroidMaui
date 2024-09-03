using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using Plugin.LocalNotification.iOSOption;
namespace TestNotification
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()

                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                })
                .UseLocalNotification(config =>
                {
                    config.AddCategory(new NotificationCategory(NotificationCategoryType.Status)
                    {
                        ActionList = new HashSet<NotificationAction>(new List<NotificationAction>()
                        {
                            new NotificationAction(100)
                            {
                                    Title = "Hello",
                                    Android =
                                    {
                                        LaunchAppWhenTapped = true,
                                        IconName =
                                        {
                                                ResourceName = "i2"
                                        }
                                    },
                                    IOS =
                                    {
                                            Action = iOSActionType.Foreground
                                    }
                            },
                            new NotificationAction(101)
                            {
                                    Title = "Close",
                                    Android =
                                    {
                                        LaunchAppWhenTapped = false,
                                        IconName =
                                        {
                                                ResourceName = "i3"
                                        }
                                    },
                                    IOS =
                                    {
                                            Action = iOSActionType.Destructive
                                    }
                            }
                        })
                    });
                });

            builder.Services.AddMauiBlazorWebView();

            /*#if ANDROID
                    TestNotification.Platforms.Android.AndroidServiceManager.StopMyService();
            #endif*/


#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
