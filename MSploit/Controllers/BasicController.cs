using Microsoft.AspNetCore.Mvc;
using MSploit.Objects;

namespace MSploit.Controllers
{
    [ApiController]
    public class BasicController : ControllerBase
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

        [HttpGet("server/settings")]
        public ActionResult<Settings> settings()
        {
            if (!util.checkSession(Request)) return new UnauthorizedResult();
            return Settings.settings;
        }

        [HttpGet("server/settings/set")]
        public ActionResult setSettings(string pyInterp)
        {
            if (!util.checkSession(Request)) return new UnauthorizedResult();
            Settings.settings.pyInterp = pyInterp;
            return new RedirectResult("/");
        }
    }
}