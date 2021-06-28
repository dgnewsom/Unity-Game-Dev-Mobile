using System;
#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif
using UnityEngine;

public class AndroidNotificationHandler : MonoBehaviour
{
#if UNITY_ANDROID
    private const string ChannelID = "driveToSurviveNotificationChannel";

    public void ScheduleNotification(DateTime dateTime)
    {
        
        AndroidNotificationChannel notificationChannel = new AndroidNotificationChannel
        {
            Id = ChannelID,
            Name = "Drive to Survive Notification Channel",
            Description = "Recharge complete",
            Importance = Importance.Default
        };

        AndroidNotificationCenter.RegisterNotificationChannel(notificationChannel);

        AndroidNotification notification = new AndroidNotification
        {
            Title = "Energy Recharged!",
            Text = "Your energy is full, come back and try to beat your HighScore",
            SmallIcon = "small",
            LargeIcon = "large",
            FireTime = dateTime
        };

        AndroidNotificationCenter.SendNotification(notification, ChannelID);
    }
    #endif
}
