using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MSploit;
using MSploit.Objects;

[ApiController]
public class NotificationsController : ControllerBase
{
    [HttpGet("server/notifications")]
    public ActionResult<List<Notification>> getNotifications()
    {
        if (!util.checkSession(Request)) return new UnauthorizedResult();
        return Notification.notifications;
    }

    [HttpPost("server/notifications/{id}/delete")]
    public ActionResult<bool> deleteNotification(int id)
    {
        if (!util.checkSession(Request)) return new UnauthorizedResult();
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
