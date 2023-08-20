using Foundation;
using UserNotifications;

namespace PushNoticationiOSMAUI
{
    public class UserNotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        // handle users tap notification when app is running or in a foreground
        public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            try
            {
                if (response.Notification.Request.Content.UserInfo.ValueForKey(new NSString("aps")) is NSDictionary aps)
                {
                    if (aps.ValueForKey(new NSString("alert")) is NSDictionary alert)
                    {
                        // you can get your data here
                        var title = Convert.ToString(alert["title"]?.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
               
            }

            completionHandler();
        }

        // go here when app is running
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            // update alert number
            if (notification.Request.Content.UserInfo.ValueForKey(new NSString("aps")) is NSDictionary aps)
            {
                if (aps.ValueForKey(new NSString("badge")) is NSDictionary badge)
                {
                   
                }  
            }
                
            completionHandler(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Badge | UNNotificationPresentationOptions.Sound);
        }
    }
}
