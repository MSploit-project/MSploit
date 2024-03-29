﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http;
using MSploit.Objects;
using Newtonsoft.Json;
using NmapXmlParser;

namespace MSploit
{
    public class util
    {
        public static void runCmd(string program, string? command)
        {
            
            ProcessStartInfo cmdsi = new ProcessStartInfo(program);
            if (command != null) cmdsi.Arguments = command;
            cmdsi.UseShellExecute = false;
            Process? cmd = Process.Start(cmdsi);
            if (cmd != null) cmd.WaitForExit();
        }
        
        public static string randomString()
        {
            Random r = new Random();
            int lenght = r.Next(5,25);
            string outString = "";
            for (int i = 0; i < lenght; i++)
            {
                outString += randomChar();
            }
            return outString;
        }

        public static char randomChar()
        {
            Random r = new Random();
            char[] chars = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            return chars[r.Next(chars.Length)];
        }
        
        public static bool checkSession(HttpRequest Request)
        {
            var cookie = Request.Cookies["auth"];
            var validLogin = cookie != null && NormalPagesController.validAuth.Contains(cookie);
            return validLogin;
        }

        public static void save()
        {
            savedData data = new(Hosts.List, NormalPagesController.validAuth, Settings.settings);
            data.write();
        }

        public static void load()
        {
            savedData.read();
        }

        public static void updateHostFromScan(string host, string resultFile)
        {
            var xmlSerializer = new XmlSerializer(typeof(nmaprun));
            var result = default(nmaprun);
            using (var xmlStream = new StreamReader(resultFile))
            {
                result = xmlSerializer.Deserialize(xmlStream) as nmaprun;
                foreach (var host1var in result.Items)
                {
                    if (host1var != null && host1var.GetType() == typeof(host))
                    {
                        NmapXmlParser.host host1 = (host) host1var;
                        Hosts found = new Hosts(host, true);
                    bool alreadyIn = false;
                    foreach (var listHost in Hosts.List)
                    {
                        if (listHost.ip == host)
                        {
                            found = listHost;
                            alreadyIn = true;
                            break;
                        }
                    }

                    found.ip = host;
                    found.up = host1.status.state == statusState.up;
                    foreach (var item in host1.Items)
                    {
                        Type type = item.GetType();
                        switch (item)
                        {
                            case os osInfo:
                                if (osInfo.osmatch != null)
                                {
                                    foreach (var match in osInfo.osmatch)
                                    {
                                        String s = "";
                                        foreach (osclass osClass in match.osclass)
                                        {
                                            found.Os = osClass.osfamily.ToLower() switch
                                            {
                                                "linux" => Hosts.OS.linux,
                                                "windows" => Hosts.OS.win,
                                                "macos" => Hosts.OS.osx,
                                                _ => Hosts.OS.other_unknown
                                            };
                                            found.OsVer = osClass.osgen;
                                            break;
                                        }
                                        break;
                                    }
                                }
                                break;
                            case ports portInfo:
                                found.ports = new();
                                if (portInfo.port != null)
                                {
                                    foreach (port portFound in portInfo.port)
                                    {
                                        if (portFound.state.state1.ToLower().StartsWith("open"))
                                        {
                                            Port addport = new Port();
                                            addport.portNum = portFound.portid;
                                            addport.protocol = portFound.protocol.ToString();
                                            addport.service = portFound.service.name;
                                            addport.serviceProduct = portFound.service.product;
                                            found.ports.Add(addport);
                                        }
                                    }
                                }
                                break;
                            case address addr:
                                if (addr.addrtype == addressAddrtype.ipv4)
                                {
                                    found.ip = addr.addr;
                                }
                                break;
                        }
                    }

                    if (!alreadyIn) Hosts.List.Add(found);
                    }
                }
            }
            Notification.add("Nmap done!", $"Nmap for {host} completed!");
        }
    }

    public class savedData
    {
        public List<Hosts> hosts { get; set; }
        public List<string> tokens { get; set; }
        public Settings settings { get; set; }
        public savedData(List<Hosts> hosts, List<string> tokens, Settings settings)
        {
            this.hosts = hosts;
            this.tokens = tokens;
            this.settings = settings;
        }

        public savedData()
        {
            
        }

        public static void read()//load
        {
            checkSavePaths();
            if (File.Exists("saved/data.msploit"))
            {
                savedData? read = JsonConvert.DeserializeObject<savedData>(File.ReadAllText("saved/data.msploit"));
                if (read != null)
                {
                    Hosts.List = read.hosts;
                    NormalPagesController.validAuth = read.tokens;
                    Settings.settings = read.settings;
                }
            }
        }

        public void write()//save
        {
            checkSavePaths();
            String jsonData = JsonConvert.SerializeObject(this);
            File.WriteAllText("saved/data.msploit", jsonData);
        }

        private static void checkSavePaths()
        {
            if (!Directory.Exists("saved")) Directory.CreateDirectory("saved");
        }
    }
}