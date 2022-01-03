using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MSploit.Objects;

[ApiController]
public class NotificationsController
{
    [HttpGet("server/notifications")]
    public ActionResult<List<Notification>> getNotifications()
    {
        return Notification.notifications;
    }

    [HttpPost("server/notifications/{id}/delete")]
    public ActionResult<bool> deleteNotification(int id)
    {
        int exists = -1;
        Notification found = null;
        foreach (var notification in Notification.notifications)
        {
            if (notification.id == id)
            {
                exists = notification.id;
                found = notification;
            }
        }
        if (exists != -1) Notification.notifications.Remove(found);
        return exists != -1;
    }
}
