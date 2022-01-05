using System;
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
        public static void runCmd(string command)
        {
            
            ProcessStartInfo cmdsi = new ProcessStartInfo("nmap.exe");
            cmdsi.Arguments = command;
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
            savedData data = new(Hosts.List, NormalPagesController.validAuth);
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
                foreach (host host1 in result.Items)
                {
                    Hosts found = new Hosts(host, true);
                    foreach (var listHost in Hosts.List)
                    {
                        if (listHost.ip == host)
                        {
                            found = listHost;
                            break;
                        }
                    }

                    found.up = host1.status.state == statusState.up;
                    foreach (var item in host1.Items)
                    {
                        Type type = item.GetType();
                        switch (item)
                        {
                            case tcptssequence tcpts:
                                break;
                            case ipidsequence ipid:
                                break;
                            case tcpsequence tcp:
                                break;
                            case distance dist:
                                break;
                            case uptime up:
                                break;
                            case os osInfo:
                                break;
                            case ports portInfo:
                                break;
                            case hostnames hostsInfo:
                                break;
                            case address addr:
                                break;
                        }
                    }
                }
            }
        }
    }

    public class savedData
    {
        public List<Hosts> hosts { get; set; }
        public List<string> tokens { get; set; }
        public savedData(List<Hosts> hosts, List<string> tokens)
        {
            this.hosts = hosts;
            this.tokens = tokens;
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