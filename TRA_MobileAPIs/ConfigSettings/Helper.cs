using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace TRA_MobileAPIs.ConfigSettings
{
    public class Helper
    {
        private static void LogWrite(string logMessage, StreamWriter w)
        {
            w.WriteLine("{0}", logMessage);
            w.WriteLine("--------------------------------------------------------");
        }
        public static void writeLog(string strValue)
        {
            try
            {
                string logFile = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";

                //Logfile//C:\inetpub\Log2\EkeyMobileAPILogs
                string path = "C:/inetpub/Log2/EkeyMobileAPILogs/" + logFile;
                StreamWriter sw;
                if (!File.Exists(path))
                { 
                    sw = File.CreateText(path); 
                }
                else
                {
                    sw = File.AppendText(path); 
                }

                LogWrite(strValue, sw);

                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                throw ex;
                

            }
        }

    }
}