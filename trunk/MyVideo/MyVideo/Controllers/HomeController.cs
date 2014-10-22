using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.Routing;
using MyVideo.Models;
using WebGrease.Css.Extensions;

namespace MyVideo.Controllers
{
    public class HomeController : Controller
    {
        public static Dictionary<string, Process> processes = new Dictionary<string, Process>();
        public static Dictionary<Process, string> otput = new Dictionary<Process, string>();

        //private string[] folders;

        public HomeController()
        {
            //folders = System.IO.File.ReadAllLines(Server.MapPath("~") + "Folders.txt");
        }

        [ValidateInput(false)]
        public ActionResult Index(string source)
        {
            var folders = System.IO.File.ReadAllLines(Server.MapPath("~") + "Folders.txt");
            var model = new FolderModel();
            model.Folder = new Dictionary<string, string>();

            model.source = "123";

            if (source == null)
            {
                foreach (var folder in folders)
                {
                    var di = new DirectoryInfo(folder);
                    model.Folder.Add(di.FullName, di.Name);
                }
            }
            else
            {
                if (System.IO.File.Exists(Server.MapPath("~") + source))
                {
                    model.JWPlayerSource = source;
                }
                else
                {
                    var di = new DirectoryInfo(source);

                    foreach (var dir in di.GetDirectories())
                    {
                        model.Folder.Add(dir.FullName, dir.Name);
                    }

                    foreach (var file in di.GetFiles())
                    {
                        model.Folder.Add(file.FullName, file.Name);
                    }

                    model.ParentFolder = di.Parent.FullName;
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GetStream(string source, string offset, string fileFormat, string bitrate, bool isEmbed, int soundNumber)
        {
            

            if (Directory.Exists(source))
            {
                var di = new DirectoryInfo(source);

                if (ContainsMain(di))
                {
                    return RedirectToAction("Index", new RouteValueDictionary() { { "source", di.FullName } });
                }

                return RedirectToAction("Index");
            }

            if (System.IO.File.Exists(source))
            {
                

                offset = (int.Parse(offset) * 60).ToString();
                var  myproc = new Process();

                if (processes.ContainsKey(source))
                {
                    try
                    {
                        if (!processes[source].HasExited)
                            processes[source].Kill();
                        
                    }
                    catch
                    {
                    }
                    finally
                    {
                        processes.Remove(source);
                    }
                }
                Server.ScriptTimeout = 90000;
                Thread.Sleep(1000);

                processes.Add(source, myproc);

                var fi = new FileInfo(source);
                string outputFile;
                string line;

                if (isEmbed)
                {
                    if (!Request.Browser.IsMobileDevice)
                        fileFormat = "flv";
                    else
                    {
                        fileFormat = "mp4";
                    }
                }

                if (fileFormat == "flv")
                {
                    outputFile = fi.Name.Replace(fi.Extension, "") + ".flv";
                    line = string.Format(
                        @"-i ""{0}"" -ss {2} -async 1 -c:a libfdk_aac -vbr 3 -b {3}k -vf ""scale=400:trunc(ow/a/2)*2"" -map 0:0  -map 0:{4} -v 0 -f flv -vcodec libx264 ""{1}""",
                        source, Server.MapPath("~") + outputFile, offset, bitrate, soundNumber);
                }
                else if (fileFormat == "mp4")
                {
                    outputFile = fi.Name.Replace(fi.Extension, "") + "." + fileFormat;
                    line = string.Format(
                        @"-i ""{0}"" -ss {2} -b {3}k -acodec mp3 -vf ""scale=400:trunc(ow/a/2)*2"" -map 0:0 -map 0:{4} -vcodec h264 ""{1}""",
                        source, Server.MapPath("~") + outputFile, offset, bitrate, soundNumber);
                    otput.Add(myproc, Server.MapPath("~") + outputFile);
                }
                else
                {
                    outputFile = fi.Name.Replace(fi.Extension, "") + "." + fileFormat;
                    line = string.Format(
                        @"-i ""{0}"" -ss {2} -b {3}k -v 0 -f mpegts ""{1}""",
                        source, Server.MapPath("~") + outputFile, offset, bitrate);
                }

                try
                {
                    System.IO.File.Delete(Server.MapPath("~") + outputFile);
                }
                catch (Exception)
                {


                }

                var myProcessStartInfo = new ProcessStartInfo();
                myProcessStartInfo.FileName = Server.MapPath("~") + "ffmpeg.exe";
                myProcessStartInfo.Arguments = line;
                myProcessStartInfo.UseShellExecute = false;
                myProcessStartInfo.RedirectStandardOutput = true;
                myProcessStartInfo.RedirectStandardError = true;
                myProcessStartInfo.RedirectStandardInput = true;
                myProcessStartInfo.RedirectStandardOutput = true;
                myProcessStartInfo.CreateNoWindow = true;
                myproc.StartInfo = myProcessStartInfo;
                myproc.ErrorDataReceived += myproc_ErrorDataReceived;
                myproc.Start();

                Thread log = new Thread(Log);
                log.Start(new Dictionary<Process, string>()
                {
                    {myproc, Server.MapPath("~") + outputFile}
                });

                while (!myproc.HasExited && (!System.IO.File.Exists(Server.MapPath("~") + outputFile) || (new FileInfo(Server.MapPath("~") + outputFile).Length == 0)))
                {
                    Thread.Sleep(2000);
                }

                if (isEmbed)
                {
                    return RedirectToAction("Index", new RouteValueDictionary() { { "source", outputFile } });
                }
                else
                {
                    
                    Response.Redirect(Url.Content(fi.Directory.FullName), true);
                }
            }

            return RedirectToAction("Index");
        }

        

        //public ActionResult LaunchJWPlayer(string source)
        //{
        //    return RedirectToAction("Index");
        //}

        void myproc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            
        }

        void Log(object proc)
        {
            Dictionary<Process, string> process = (Dictionary<Process, string>)proc;

            while (!process.Keys.First().HasExited)
            {
                for (int i = 0; i < 100; i++)
                {
                    System.IO.File.AppendAllText(process.Values.First() + ".txt", process.Keys.First().StandardError.ReadLine() + Environment.NewLine);
                }
                Thread.Sleep(1000);
            }
        }

        public static string GetProcessPercentage(Process proc)
        {
            var res = "no percentage data";

            try
            {
                string data = System.IO.File.ReadAllText(otput[proc] + ".txt");

                var allTime = GetTime(data, data.IndexOf(":", data.IndexOf("Duration:") + 10));
                var currTime = GetTime(data, data.LastIndexOf(":") - 3);

                var percentage = (currTime.TotalMilliseconds / allTime.TotalMilliseconds) * 100;
                res = Convert.ToInt32(percentage).ToString();
            }
            catch (Exception)
            {
                
                
            }

            return res;
        }

        private static TimeSpan GetTime(string data, int index)
        {
            var hours = data.Substring(index - 2, 2);
            var minutes = data.Substring(index + 1, 2);
            var seconds = data.Substring(index + 4, 2);

            return new TimeSpan(int.Parse(hours), int.Parse(minutes), int.Parse(seconds));
        }

        public bool ContainsMain(DirectoryInfo curDir)
        {
            if (IsMain(curDir))
            {
                return true;
            }

            return curDir.Parent != null && ContainsMain(curDir.Parent);
        }

        public bool IsMain(DirectoryInfo curDir)
        {
            var folders = System.IO.File.ReadAllLines(Server.MapPath("~") + "Folders.txt");

            foreach (var folder in folders.Select(s => new DirectoryInfo(s)))
            {
                if (folder.FullName == curDir.FullName)
                {
                    return true;
                }
            }

            return false;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
