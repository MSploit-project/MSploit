using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSploit;
using MSploit.Objects;

[ApiController]
public class NormalPagesController : ControllerBase
{
    //basic credentials for now
    private string login => Settings.settings.userName;
    private string password => Settings.settings.password;
    public static List<string> validAuth = new ();

    [HttpGet("")]
    public ContentResult HomePage()
    {
        var validLogin = util.checkSession(Request);
        try
        {
            var data = System.IO.File.ReadAllText(validLogin?$@"{Directory.GetCurrentDirectory()}\WebPage\index.html":$@"{Directory.GetCurrentDirectory()}\WebPage\login.html");
            return new ContentResult()
            {
                Content = data,
                ContentType = "text/html",
                StatusCode = (int) HttpStatusCode.OK
            };
        }
        catch
        {
            return new ContentResult()
            {
                Content = "Something went wrong",
                ContentType = "text/html",
                StatusCode = (int) HttpStatusCode.InternalServerError
            };
        }
    }

    [HttpGet("icons/Pcs/{iconUrl}")]
    public IActionResult getIcon(string iconUrl)
    {
        if (!util.checkSession(Request)) return new UnauthorizedResult();
        try
        {
            var data = System.IO.File.OpenRead($@"{Directory.GetCurrentDirectory()}\WebPage\Icons\Pcs\{iconUrl}");
            return File(data, "image/png");
        }
        catch
        {
            return new ContentResult()
            {
                Content = "Error 500 Something went wrong",
                ContentType = "text/html",
                StatusCode = (int) HttpStatusCode.InternalServerError
            };
        }
    }

    [HttpGet("login")]
    public RedirectResult loginAuth(string name, string pass)
    {
        if (name == login && pass == this.password)
        {
            var auth = util.randomString();
            validAuth.Add(auth);
            Response.Cookies.Append("auth", auth);
        }
        return new RedirectResult("/");
    }
}