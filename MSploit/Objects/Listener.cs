using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace MSploit.Objects
{
    public class Listener : IDisposable
    {
        public static List<Listener> Listeners = new List<Listener>();
        public int port { get; }
        public TcpListener listener;
        private bool running = true;
        public List<Client> clients = new List<Client>();

        public Listener(int port)
        {
            this.port = port;
            new Thread(() =>
            {
                try
                {
                    listener = new TcpListener(IPAddress.Any, port);
                    TcpClient client;
                    listener.Start();
                    while (running)
                    {
                        client = listener.AcceptTcpClient();
                        ThreadPool.QueueUserWorkItem(newClient, client);
                    }
                }
                catch
                {
                    
                }
            }).Start();
        }

        private void newClient(object? obj)
        {
            var client = obj as TcpClient;
            if (client != null)
            {
                client.GetStream().ReadTimeout = -1;
                Client CClient = new Client(client);
                clients.Add(CClient);
                bool found = false;
                foreach (var host in Hosts.List) if (host.ip.ToLower().Equals(CClient.getIp.ToLower())) found = true;
                if (!found) Hosts.add(CClient.getIp, true);
                Notification.add("Reverse shell obtained!", CClient.getIp);
                new Thread(() =>
                {
                    try
                    {
                        var networkStream = client.GetStream();
                        // Server Reply
                        if (networkStream.CanRead) {
                            do
                            {
                                var buf = new byte[client.ReceiveBufferSize];
                                var n = networkStream.Read(buf, 0, buf.Length);
                                if(n == 0)
                                {
                                    return; // connection is lost
                                }
                                string fullServerReply = new string(Encoding.ASCII.GetChars(buf)).Replace("\0", "");//we don't want empty bytes
                                CClient.addData(fullServerReply);
                            }
                            while(true);
                        }
                        clients.Remove(CClient);
                        Notification.add("Lost shell", CClient.getIp);
                    }
                    catch
                    {
                        clients.Remove(CClient);
                        Notification.add("Lost shell", CClient.getIp);
                    }
                }).Start();
            }
        }

        public void Dispose()
        {
            running = false;
            //TODO remove tcp connection
        }
    }

    public class Client
    {
        private static int maxId = 0;
        public int id;
        private TcpClient client;
        private string dataRechieved;

        public string getData()
        {
            return dataRechieved;
        }

        public void addData(string data)
        {
            dataRechieved += data;
        }

        public void setData(string data)
        {
            dataRechieved = data;
        }
        public Client(TcpClient client)
        {
            id = maxId++;
            this.client = client;
            dataRechieved = "";
        }

        public void sendData(byte[] data)
        {
            client.GetStream().Write(data, 0, data.Length);
            client.GetStream().Flush();
        }

        public string getIp
        {
            get {
                return ((IPEndPoint) client.Client.RemoteEndPoint).Address.ToString();
            }
        }
    }
}