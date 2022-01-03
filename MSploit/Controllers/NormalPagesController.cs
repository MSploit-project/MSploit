using System;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class NormalPagesController : ControllerBase
{
    [HttpGet("")]
    public ContentResult HomePage()
    {
        try
        {
            var data = System.IO.File.ReadAllText(@"C:\Users\milan\RiderProjects\MSploit\MSploit\WebPage\index.html");//absolute path for now
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
    public IActionResult getIcon(String iconUrl)
    {
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
}