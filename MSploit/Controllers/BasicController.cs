using Microsoft.AspNetCore.Mvc;

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
    }
}