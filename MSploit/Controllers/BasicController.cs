using Microsoft.AspNetCore.Mvc;
using MSploit.Objects;

namespace MSploit.Controllers
{
    [ApiController]
    public class BasicController
    {
        [HttpGet("server/ping")]
        public ActionResult<bool> pingServer()
        {
            return true;
        }
        
        [HttpGet("server/status")]
        public ActionResult<bool> serverStatus()
        {
            Notification.notifications.Add(new Notification("Server status", "Server is online!"));
            return true;
        }
    }
}