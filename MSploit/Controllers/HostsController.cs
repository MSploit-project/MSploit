using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSploit.Objects;

[ApiController]
public class HostsController
{
    [HttpGet("hosts/new")]
    public RedirectResult addHost(String host)
    {
        Notification.add("New host added!", host);
        //TODO: add host
        return new RedirectResult("/");
    }

    [HttpGet("hosts/new/scan")]
    public RedirectResult scanHost(String host, String? scanType, String? fast, String? ver, String? aggr, String? osd)
    {
        Notification.add("Nmap scan started!", host);
        String command = $"nmap {host} {scanType} {fast} {ver} {aggr} {osd}";
        //TODO: scan through nmap
        return new RedirectResult("/");
    }
}