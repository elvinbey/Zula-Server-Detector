using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Game;
using System.Diagnostics;
namespace ServerDetector
{
    class Program
    {
        static string DownloadString(string url)
        {
            WebClient client = new WebClient();
            string data = client.DownloadString(url);
            return data;
        }

        static string ReadXML(string data)
        {
            RichTextBox ric = new RichTextBox();
            ric.Text = data.Replace("<update>", "").Replace("</update>", "");
            string launcher = ric.Lines[2].Replace("<file url=\"zula_launcher.exe\"", "").Replace(" high=\"0\"></file>", "").Replace(" high=\"1\"></file>", "").Replace(" ", "|").Replace("||", "").Replace("|" , " ");
            return launcher;
        }


        static string ReadXML_File(string path)
        {
            string text;
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                text = streamReader.ReadToEnd();
            }
            RichTextBox ric = new RichTextBox();
            ric.Text = text.Replace("<update>", "").Replace("</update>", "");
            string launcher = ric.Lines[2].Replace("<file url=\"zula_launcher.exe\"", "").Replace(" high=\"0\"></file>", "").Replace(" high=\"1\"></file>", "").Replace(" ", "|").Replace("||", "").Replace("|", " ");
            return launcher;
        }

        static bool CheckProcess(string process)
        {
            bool state = false;
            foreach (Process p in Process.GetProcesses())
            {
                if (p.ProcessName == process)
                {
                    state = true;
                }
            }
            return state;
        }

        static string pathxxx;
        static string ProcessPath(string process)
        {
            foreach (Process p in Process.GetProcesses())
            {
                if (p.ProcessName == process)
                {
                    pathxxx = p.MainModule.FileName;
                }
            }
            return pathxxx;
        }

        static void Write(string data)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("["+ DateTime.Now.ToLongTimeString() + "]: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(data + "\n");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
        }
        static string pathx;
        static void Main(string[] args)
        {
            //**************************************************************************************
            Write("Downloading current data");
            try
            {
                Game.GServer.CRL_TR = ReadXML(DownloadString(Game.GServer.Server_TR));
                Game.GServer.CRL_EU = ReadXML(DownloadString(Game.GServer.Server_EU));
                Game.GServer.CRL_BR = ReadXML(DownloadString(Game.GServer.Server_BR));
            }
            catch
            {
                Write("Download failed");
                Console.ReadLine();
            }
            //**************************************************************************************
            Write("Checking process");
            System.Threading.Thread.Sleep(1000);
            Write("Waiting for process");
            bool stop = false;
            while (stop == false)
            {
                if (CheckProcess(Game.GProcess.Game) || CheckProcess(Game.GProcess.Launcher))
                {
                    stop = true;
                }
            }
            //**************************************************************************************
            System.Threading.Thread.Sleep(1000);
            Write("Process found");
            if (CheckProcess(Game.GProcess.Game) == true)
            {
                pathx = ProcessPath(Game.GProcess.Game);
                System.Threading.Thread.Sleep(1000);
                pathx = pathx.Replace("\\Game\\zula.exe", "\\zula_files2.xml");

            }
            else if (CheckProcess(Game.GProcess.Launcher))
            {
                pathx =  ProcessPath(Game.GProcess.Launcher);
                System.Threading.Thread.Sleep(1000);
                pathx = pathx.Replace("\\zula_launcher.exe", "\\zula_files2.xml");
            }
            //**************************************************************************************
            System.Threading.Thread.Sleep(1000);
            Write("Checking Server");
            System.Threading.Thread.Sleep(1000);
            string currnet = ReadXML_File(pathx);
            if (currnet == Game.GServer.CRL_TR) {
                Write("Server: TR");
            }
            if (currnet == Game.GServer.CRL_EU) {
                Write("Server: EU");
            }
            if (currnet == Game.GServer.CRL_BR){
                Write("Server: BR");
            }
            //**************************************************************************************
            Console.ReadLine();
        }
    }
}


namespace Game
{

    class GProcess
    {
        public static string Launcher = "zula_launcher";
        public static string Game = "zula";
    }
  
    class GServer
    {
        public static string Server_TR = "http://cdnpatch.zulaoyun.com/zulalive/1.26f_5/zula_files2.xml";
        public static string Server_EU = "http://update.idcgames.com/zula/zulalive/1.26/zula_files2.xml";
        public static string Server_BR = "http://lokumgameslatam-p.mncdn.com/br/zulalive/1.26a_1/zula_files2.xml";
        public static string CRL_TR, CRL_EU, CRL_BR;
    }

}