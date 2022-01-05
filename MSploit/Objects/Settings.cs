using System;

namespace MSploit.Objects
{
    public class Settings
    {
        public static Settings settings = new();

        public String pyInterp { get; set; } = "python";
        public Settings()
        {
            
        }
    }
}