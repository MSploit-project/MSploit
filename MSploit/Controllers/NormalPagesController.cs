using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSploit;

[ApiController]
public class NormalPagesController : ControllerBase
{
    //basic credentials for now
    private string login => "user";
    private string password => "lkJSV@OiHF#OLJ@$#HJBCDVop";
    public static List<string> validAuth = new ();

    [HttpGet("")]
    public ContentResult HomePage()
    {
        var validLogin = util.checkSession(Request);
        try
        {
            var data = System.IO.File.ReadAllText(validLogin?@"C:\Users\milan\RiderProjects\MSploit\MSploit\WebPage\index.html":@"C:\Users\milan\RiderProjects\MSploit\MSploit\WebPage\login.html");//absolute path for now
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
            var data = System.IO.File.OpenRead($@"C:\Users\milan\RiderProjects\MSploit\MSploit\WebPage\Icons\Pcs\{iconUrl}");
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