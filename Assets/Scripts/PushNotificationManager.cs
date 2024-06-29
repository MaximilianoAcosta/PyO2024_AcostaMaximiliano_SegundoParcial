using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif
public class PushNotificationManager : MonoBehaviour
{
#if UNITY_ANDROID
    private static string CHANNEL_ID = "notis01";
    private void Start()
    {
        //Creo los Notification Channels, una única vez.
        string NotiChannels_Created_Key = "NotiChannels_Created";
        if (!PlayerPrefs.HasKey(NotiChannels_Created_Key))
        {
            var group = new AndroidNotificationChannelGroup()
            {
                Id = "Main",
                Name = "Main notifications",
            };
            AndroidNotificationCenter.RegisterNotificationChannelGroup(group);
            var channel = new AndroidNotificationChannel()
            {
                Id = CHANNEL_ID,
                Name = "Default Channel",
                Importance = Importance.Default,
                Description = "Generic notifications",
                Group = "Main",  // Tiene que ser el mismo Id del grupo que creé antes
            };
            AndroidNotificationCenter.RegisterNotificationChannel(channel);

            StartCoroutine(RequestPermission());

            PlayerPrefs.SetString(NotiChannels_Created_Key, "y");
            PlayerPrefs.Save();
        }
        else
        {
            ScheduleNotis();
        }
    }

    private IEnumerator RequestPermission()
    {
        var request = new PermissionRequest();
        while (request.Status == PermissionStatus.RequestPending)
            yield return null;

        ScheduleNotis();
    }

    private void ScheduleNotis()
    {
        AndroidNotificationCenter.CancelAllScheduledNotifications();

        var notification3days = new AndroidNotification();
        notification3days.Title = "Acosta Maximiliano";
        notification3days.Text = "El juego no se abrió hace 10 minutos";
        notification3days.FireTime = System.DateTime.Now.AddMinutes(10);

        AndroidNotificationCenter.SendNotification(notification3days, CHANNEL_ID);
    }
#endif
}