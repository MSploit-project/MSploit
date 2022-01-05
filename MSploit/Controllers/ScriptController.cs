using System;
using System.IO;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using MSploit.Objects;

namespace MSploit.Controllers
{
    [ApiController]
    public class ScriptController : ControllerBase
    {
        [HttpGet("script/run")]
        public ActionResult runScript(string script)
        {
            if (!util.checkSession(Request)) return new UnauthorizedResult();
            new Thread(() =>
            {
                string fileName = util.randomString()+".py";
                string filePath = $@"{Directory.GetCurrentDirectory()}\scriptrun\{fileName}";
                Directory.CreateDirectory($@"{Directory.GetCurrentDirectory()}\scriptrun");
                System.IO.File.WriteAllText(filePath, script);
                util.runCmd(Settings.settings.pyInterp, $"\"{filePath}\"");
                System.IO.File.Delete(filePath);
            }).Start();
            return new RedirectResult("/");
        }
    }
}