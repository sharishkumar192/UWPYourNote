using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.WebUI;

namespace YourNoteUWP
{
    internal class Logger
    {
        public static void WriteLog(string exception)
        {


     
            // flush every 20 seconds as you do it


            StorageFolder localFolder = ApplicationData.Current.LocalFolder;


            using (StreamWriter writer = new StreamWriter(localFolder.Path + "//log.txt", true))
            {
                writer.WriteLine($" Crash Time : {DateTime.Now} ");
                writer.WriteLine($" Exception  : {exception}");
                writer.WriteLine("------------------------------");
            }
         
        }

    }
}
