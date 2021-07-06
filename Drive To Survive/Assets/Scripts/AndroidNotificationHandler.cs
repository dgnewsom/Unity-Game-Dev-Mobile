using System;
#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif
using UnityEngine;

/// <summary>
/// Class to handle android notifications
/// </summary>
public class AndroidNotificationHandler : MonoBehaviour
{
#if UNITY_ANDROID
    private const string ChannelID = "driveToSurviveNotificationChannel";

    /// <summary>
    /// Schedule a notification
    /// </summary>
    /// <param name="dateTime">Date / time to trigger the notification</param>
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
            Text = "Your energy is full!\nGo for the High Score!",
            SmallIcon = "small",
            LargeIcon = "large",
            FireTime = dateTime
        };

        AndroidNotificationCenter.SendNotification(notification, ChannelID);
    }
    #endif
}
