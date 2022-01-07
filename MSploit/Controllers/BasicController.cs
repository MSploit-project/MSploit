using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using MSploit.Objects;

namespace MSploit.Controllers
{
    [ApiController]
    public class BasicController : ControllerBase
    {
        [HttpGet("server/ping")]
        public ActionResult<bool> PingServer()
        {
            return true;
        }
        
        [HttpGet("server/status")]
        public ActionResult<bool> ServerStatus()
        {
            Notification.notifications.Add(new Notification("Server status", "Server is online!"));
            return true;
        }
        
        [HttpGet("server/ports")]
        public ActionResult<List<Listener>> GetPorts()
        {
            if (!util.checkSession(Request)) return new UnauthorizedResult();
            return Listener.Listeners; 
        }
        
        [HttpGet("server/ports/add")]
        public ActionResult<bool> AddPort(int port)
        {
            if (!util.checkSession(Request)) return new UnauthorizedResult();
            foreach (var listener in Listener.Listeners) if (listener.port == port) return false;
            Listener.Listeners.Add(new Listener(port));
            Notification.add("Port opened", port.ToString());
            return true; 
        }
        
        [HttpGet("server/ports/{port:int}/remove")]
        public ActionResult<bool> ClosePort(int port)
        {
            if (!util.checkSession(Request)) return new UnauthorizedResult();
            Listener found = null;
            foreach (var listener in Listener.Listeners) if (listener.port == port) found = listener;
            if (found != null)
            {
                Listener.Listeners.Remove(found);
                Notification.add("Port Removed", port.ToString());
                return true;
            }
            return false; 
        }

        [HttpGet("server/settings")]
        public ActionResult<Settings> settings()
        {
            if (!util.checkSession(Request)) return new UnauthorizedResult();
            return Settings.settings;
        }

        [HttpGet("server/settings/set")]
        public ActionResult setSettings(string pyInterp, string nmapPath, string userName, string password)
        {
            if (!util.checkSession(Request)) return new UnauthorizedResult();
            Settings.settings.pyInterp = pyInterp;
            Settings.settings.nmap = nmapPath;
            Settings.settings.userName = userName;
            Settings.settings.password = password;
            return new RedirectResult("/");
        }

        [HttpGet("server/clients/{id:int}")]
        public ActionResult<Client> getClient(int id)
        {
            foreach (var listener in Listener.Listeners)
            {
                foreach (var client in listener.clients)
                {
                    if (client.id == id)
                    {
                        return client;
                    }
                }
            }

            return null;
        }
        
        [HttpGet("server/clients/{id:int}/run/{command}")]
        public ActionResult getClient(int id, string command)
        {
            foreach (var listener in Listener.Listeners)
            {
                foreach (var client in listener.clients)
                {
                    if (client.id == id)
                    {
                        if (command == "cls" || command == "clear")//fake clear
                        {
                            client.setData("#>");
                        }
                        client.sendData(Encoding.ASCII.GetBytes(command + "\n"));
                    }
                }
            }

            return new RedirectResult("/");
        }
        
        [HttpGet("server/clients/{id:int}/getContent")]
        public ActionResult<string> getContent(int id)
        {
            foreach (var listener in Listener.Listeners)
            {
                foreach (var client in listener.clients)
                {
                    if (client.id == id)
                    {
                        return client.getData();
                    }
                }
            }

            return "Shell Disconnected...";
        }
    }
}