using Foundation;
using UIKit;
using UserNotifications;
using Firebase.CloudMessaging;
using System.Diagnostics;

namespace PushNoticationiOSMAUI;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate, IUNUserNotificationCenterDelegate, IMessagingDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        RegisterRemoteNotifications();

        return base.FinishedLaunching(application, launchOptions);
    }

    private void RegisterRemoteNotifications()
    {
        if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
        {
            var authOption = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;

            UNUserNotificationCenter.Current.RequestAuthorization(authOption, (granted, error) =>
            {
                if (granted && error == null)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, new NSSet());
                        UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);

                        UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();

                        UIApplication.SharedApplication.RegisterForRemoteNotifications();

                        InitFirebase();

                        Messaging.SharedInstance.AutoInitEnabled = true;
                        Messaging.SharedInstance.Delegate = this;
                    });
                }
            });
        }
    }

    private void InitFirebase()
    {

        Firebase.Core.App.Configure();

        // if the code above doesn't work, we can set it manually like code below:
        // you can find this information in GoogleService-Info.plist

        //var options = new Firebase.Core.Options("[GOOGLE_APP_ID]", "[GCM_SENDER_ID]");
        //options.ApiKey = "[API_KEY]";
        //options.ProjectId = "[PROJECT_ID]";
        //options.BundleId = "[BUNDLE_ID]";
        //options.ClientId = "[CLIENT_ID]";

        //Firebase.Core.App.Configure(options);
    }

    [Export("messaging:didReceiveRegistrationToken:")]
    public void DidReceiveRegistrationToken(Messaging messaging, string fcmToken)
    {

        Debug.WriteLine("FCM token:" + fcmToken);
        if (Preferences.ContainsKey("DeviceToken"))
        {
            Preferences.Remove("DeviceToken");
        }
        Preferences.Set("DeviceToken", fcmToken);
    }

    [Export("userNotificationCenter:didReceiveNotificationResponse:withCompletionHandler:")]
    public void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
    {
        // this method is fired when app is closed.
        // Here Handle Notification Navigation WhenNotification Is received

        if (userInfo.ValueForKey(new NSString("aps")) is NSDictionary aps)
        {
            if (aps.ValueForKey(new NSString("alert")) is NSDictionary alert)
            {
                // you can get your data here
                var title = Convert.ToString(alert["title"]?.ToString());
            }
        }
    }
}
