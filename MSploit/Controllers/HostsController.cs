using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class HostsController
{
    [HttpGet("hosts/new")]
    public RedirectResult addHost(String host)
    {
        Console.WriteLine($"add host: {host}");
        return new RedirectResult("/");
    }

    [HttpGet("hosts/new/scan")]
    public RedirectResult scanHost(String host, String? scanType, String? fast, String? ver, String? aggr, String? osd)
    {
        Console.WriteLine($"Would run nmap scan on {host}\n\tnmap {host} {scanType} {fast} {ver} {aggr} {osd}");
        return new RedirectResult("/");
    }
}