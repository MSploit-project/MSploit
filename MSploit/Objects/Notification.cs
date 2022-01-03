using System;
using System.Collections.Generic;

namespace MSploit.Objects
{
    public class Notification
    {
        public static List<Notification> notifications = new List<Notification>(){};

        public static void add(String name, String description) => notifications.Add(new Notification(name, description));
        
        private static int highestId = 0;
        public int id { get; }
        public string title { get; }
        public String content { get; }
        private DateTime creationTime { get; }
        public int minsSinceShown { get => (int) DateTime.Now.Subtract(creationTime).TotalMinutes; }

        public Notification(String title, String content)
        {
            id = highestId++;
            creationTime = DateTime.Now;
            this.title = title;
            this.content = content;
        }
    }
}