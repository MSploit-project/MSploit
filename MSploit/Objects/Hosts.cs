using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MSploit.Objects
{
    public class Hosts
    {
        public enum OS
        {
            win, osx, linux, other_unknown
        }

        public static int highestId = 0;
        
        public static List<Hosts> List = new();
        public static void add(String ip, bool online) => List.Add(new Hosts(ip, online));
        public static void add(String ip, bool online, OS Os) => List.Add(new Hosts(ip, online, Os));
        public static void add(String ip, bool online, String lanIp, OS Os) => List.Add(new Hosts(ip, online, lanIp, Os));

        public int id { get; }
        public String ip { get; set; }
        public bool up { get; set; }
        public List<Port> ports { get; set; }

        public bool pwned
        {
            get
            {
                foreach (var listener in Listener.Listeners) foreach (var client in listener.clients) if (client.getIp.ToLower().Equals(ip.ToLower())) return true;
                return false;
            }
        }

        public String lanIp { get; set; }
        public OS Os { get; set; }
        public String OsVer { get; set; }
        public String HostName { get; set; }

        public List<int> shells
        {
            get
            {
                List<int> outList = new List<int>();
                foreach (var listener in Listener.Listeners)
                {
                    foreach (var client in listener.clients)
                    {
                        if (client.getIp.ToLower().Equals(ip.ToLower()))
                        {
                            outList.Add(client.id);
                        }
                    }
                }
                return outList;
            }
        }

        public String OsString => Os switch
        {
            OS.linux => "Linux",
            OS.osx => "Mac Os",
            OS.win => "Windows",
            OS.other_unknown => "Other/Unknown",
            _ => ""
        } + " " + OsVer;
        
        public String imageName => Os switch
        {
            OS.linux => "linux",
            OS.osx => "mac",
            OS.win => "win",
            OS.other_unknown => "unix",
            _ => "unix"
        } + (pwned?"-e":"");

        
        public Hosts(String ip, bool up, String lanIp, OS Os)
        {
            id = highestId++;
            this.ip = ip;
            this.up = up;
            this.lanIp = lanIp;
            this.Os = Os;
            ports = new();
        }
        
        public Hosts(String ip, bool up, OS Os)
        {
            id = highestId++;
            this.ip = ip;
            this.up = up;
            lanIp = "xxx.xxx.xxx.xxx";
            this.Os = Os;
            ports = new();
        }
        
        public Hosts(String ip, bool up, String lanIp)
        {
            id = highestId++;
            this.ip = ip;
            this.up = up;
            this.lanIp = lanIp;
            Os = OS.other_unknown;
            ports = new();
        }
        
        public Hosts(String ip, bool up)
        {
            id = highestId++;
            this.ip = ip;
            this.up = up;
            lanIp = "xxx.xxx.xxx.xxx";
            Os = OS.other_unknown;
            ports = new();
        }

        public Hosts()
        {
            id = highestId++;
            ports = new();
        }
    }

    public class Port
    {
        public string portNum { get; set; }
        public string? service { get; set; }
        public string? serviceProduct { get; set; }
        public string protocol { get; set; }
    }
}