using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSploit;
using MSploit.Objects;

[ApiController]
public class HostsController : ControllerBase
{
    [HttpGet("hosts/new")]
    public IActionResult addHost(String? host)
    {
        if (!util.checkSession(Request)) return new UnauthorizedResult();
        if (host != null)
        {
            Hosts found = null;
            foreach (var hosts in Hosts.List) if (hosts.ip == host) found = hosts;

            if (found == null)
            {
                Notification.add("New host added!", host);
                Hosts.add(host, false);
            }
        }
        return new RedirectResult("/");
    }

    [HttpGet("hosts/new/scan")]
    public IActionResult scanHost(String host, String? scanType, String? fast, String? ver, String? aggr, String? osd)
    {
        if (!util.checkSession(Request)) return new UnauthorizedResult();
        string dir = $"{Directory.GetCurrentDirectory()}\\scans\\{host}.xml";
        Notification.add("Nmap scan started!", $"Target: {host}");
        String command = $"{host} {scanType} {fast} {ver} {aggr} {osd} -oX \"{dir}\"";
        Console.WriteLine($"Running nmap.exe {command}");
        //TODO: scan through nmap
        new Thread(() =>
        {
            Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}\\scans");
            util.runCmd("nmap.exe", command);
            util.updateHostFromScan(host, dir);
        }).Start();
        return new RedirectResult("/");
    }
    
    [HttpGet("hosts")]
    public ActionResult<List<Hosts>> getHosts()
    {
        if (!util.checkSession(Request)) return new UnauthorizedResult();
        return Hosts.List;
    }
    
    [HttpPost("hosts/{id:int}/delete")]
    public ActionResult<bool> deleteHost(int id)
    {
        if (!util.checkSession(Request)) return new UnauthorizedResult();
        int exists = -1;
        Hosts found = null;
        foreach (var host in Hosts.List)
        {
            if (host.id == id)
            {
                exists = host.id;
                found = host;
            }
        }

        if (exists != -1)
        {
            Hosts.List.Remove(found);
            Notification.add("Host removed", $"Removed host {found.ip}");
        }
        return exists != -1;
    }

    [HttpGet("hosts/{id:int}")]
    public ActionResult<Hosts> getHost(int id)
    {
        if (!util.checkSession(Request)) return new UnauthorizedResult();
        Hosts found = null;
        foreach (var host in Hosts.List) if (host.id == id) found = host;

        if (found != null)
        {
            return found;
        }

        return new Hosts("Invalid", false);
    }
}