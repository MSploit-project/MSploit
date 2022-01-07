using System;

namespace MSploit.Objects
{
    public class Settings
    {
        public static Settings settings = new();

        public String pyInterp { get; set; } = "python";
        public String nmap { get; set; } = "nmap";
        public String userName { get; set; } = "user";
        public String password { get; set; } = "lkJSV@OiHF#OLJ@$#HJBCDVop";
        public Settings()
        {
            
        }
    }
}