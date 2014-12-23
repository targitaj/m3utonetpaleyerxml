using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
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
        public string Source
        {
            get { return Server.MapPath(@"~\Videos\"); }
        }

        public static Dictionary<string, Process> processes = new Dictionary<string, Process>();
        public static Dictionary<Process, string> otput = new Dictionary<Process, string>();

        //private string[] folders;

        public HomeController()
        {
            //folders = System.IO.File.ReadAllLines(Source + "Folders.txt");
        }

        [ValidateInput(false)]
        public ActionResult Index(string source)
        {
            var folders = System.IO.File.ReadAllLines(Server.MapPath(@"~\Folders.txt"));
            var model = new FolderModel();
            model.Folder = new Dictionary<string, string>();

            model.source = source;

            if (source == null)
            {
                foreach (var folder in folders)
                {
                    var di = new DirectoryInfo(folder);
                    model.Folder.Add(di.FullName, di.Name);
                }

                model.Folder.Add("TV", "TV");
            }
            else
            {
                if (source == "TV")
                {
                    var urls = System.IO.File.ReadLines(Server.MapPath(@"~\TV\tv.m3u")).ToList();
                    Uri res;

                    foreach (var url in urls)
                    {
                        bool isUri = Uri.TryCreate(url, UriKind.Absolute, out res) && res.Scheme == Uri.UriSchemeHttp;

                        if (isUri)
                        {
                            var name = urls[urls.IndexOf(url)-2].Replace("#EXTINF:-1, ", "");
                            model.Folder.Add(url, name);
                        }
                    }
                }
                else
                {
                    Uri res;

                    bool isUri = source.Contains("rtmp");
                    bool isVlc = source.Contains("tcp");

                    if (isVlc)
                    {
                        model.VLCStreamUrl = source;
                    }
                    else
                    if (isUri)
                    {
                        model.Url = source;
                    }
                    else
                    if (System.IO.File.Exists(Source + source))
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
            }

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GetStream(string source, string offset, string fileFormat, string bitrate, bool isEmbed, int soundNumber, bool isVlc)
        {
            if (source == "TV")
            {
                return RedirectToAction("Index", new RouteValueDictionary() { { "source", "TV" } });
            }

            if (Directory.Exists(source))
            {
                var di = new DirectoryInfo(source);

                if (ContainsMain(di))
                {
                    return RedirectToAction("Index", new RouteValueDictionary() { { "source", di.FullName } });
                }

                return RedirectToAction("Index");
            }

            Uri res;
            bool isUri = Uri.TryCreate(source, UriKind.Absolute, out res) && res.Scheme == Uri.UriSchemeHttp;

            if (System.IO.File.Exists(source) || isUri)
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

                if (isUri)
                {
                    var prcs = GetProcessIds();

                    foreach (var pr in prcs)
                    {
                        if (pr.Key == Request.UserHostAddress)
                        {
                            try
                            {
                                Process prc = Process.GetProcessById(int.Parse(pr.Value));
                                prc.Kill();
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                }

                Server.ScriptTimeout = 90000;
                Thread.Sleep(1000);

                processes.Add(source, myproc);

                FileInfo fi = null;

                if (!isUri)
                {
                    fi = new FileInfo(source);
                }
                string outputFile = null;
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

                if (isUri)
                {
                    if (isVlc)
                    {
                        outputFile = "TV.txt";
                        line = string.Format(
                            @"-i ""{0}"" -b {1}k -vf ""scale=400:trunc(ow/a/2)*2"" -loglevel quiet -f mpegts tcp://a.mosalsky.com:2042?listen",
                            source, bitrate);
                    }
                    else
                    {
                        outputFile = "TV.txt";

                        line = string.Format(
                            @"-i ""{0}"" -b {1}k -c:a libfdk_aac -vbr 3 -vf ""scale=400:trunc(ow/a/2)*2"" -loglevel quiet -f flv -vcodec libx264 rtmp://a.mosalsky.com:1935/live/" + Request.UserHostAddress,
                            source, bitrate);
                    }
                }
                else
                if (fileFormat == "flv")
                {
                    outputFile = fi.Name.Replace(fi.Extension, "") + ".flv";
                    line = string.Format(
                        @"-i ""{0}"" -ss {2} -async 1 -c:a libfdk_aac -vbr 3 -b {3}k -vf ""scale=400:trunc(ow/a/2)*2"" -map 0:0  -map 0:{4} -v 0 -f flv -vcodec libx264 ""{1}""",
                        source, Source + outputFile, offset, bitrate, soundNumber);
                }
                else if (fileFormat == "mp4")
                {
                    outputFile = fi.Name.Replace(fi.Extension, "") + "." + fileFormat;
                    line = string.Format(
                        @"-i ""{0}"" -ss {2} -b {3}k -acodec mp3 -vf ""scale=400:trunc(ow/a/2)*2"" -map 0:0 -map 0:{4} -vcodec h264 ""{1}""",
                        source, Source + outputFile, offset, bitrate, soundNumber);
                    otput.Add(myproc, Source + outputFile);
                }
                else
                {
                    outputFile = fi.Name.Replace(fi.Extension, "") + "." + fileFormat;
                    line = string.Format(
                        @"-i ""{0}"" -ss {2} -b {3}k -v 0 -f mpegts ""{1}""",
                        source, Source + outputFile, offset, bitrate);
                }

                try
                {
                    if (outputFile != null)
                    {
                        System.IO.File.Delete(Source + outputFile);
                    }
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

                if (!isUri)
                {
                    Thread log = new Thread(Log);
                    log.Start(new Dictionary<Process, string>()
                    {
                        {myproc, Source + outputFile}
                    });

                    while (!myproc.HasExited &&
                           (!System.IO.File.Exists(Source + outputFile) ||
                            (new FileInfo(Source + outputFile).Length == 0)))
                    {
                        Thread.Sleep(2000);
                    }
                }
                else
                {
                    AddProcessId(myproc.Id);
                    Thread.Sleep(2000);
                }

                if (isEmbed)
                {
                    return RedirectToAction("Index", new RouteValueDictionary() { { "source", outputFile } });
                }

                if (isVlc)
                {
                    return RedirectToAction("Index", new RouteValueDictionary() { { "source", @"tcp://a.mosalsky.com:2042?listen" } });
                }

                if (isUri)
                {
                    return RedirectToAction("Index", new RouteValueDictionary() { { "source", @"rtmp://a.mosalsky.com:1936/live/" + Request.UserHostAddress } });
                }

                var di = new DirectoryInfo(source);
                return RedirectToAction("Index", new RouteValueDictionary() { { "source", di.Parent.FullName } });
            }

            return RedirectToAction("Index");
        }

        public Dictionary<string, string> GetProcessIds()
        {
            var formatter = new BinaryFormatter();
            FileStream fs = null;
            var channels = new Dictionary<string, string>();
            try
            {
                fs = System.IO.File.Open(Server.MapPath(@"~\TV\streamIds.txt"), FileMode.Open);
                channels = (Dictionary<string, string>)formatter.Deserialize(fs);
            }
            catch
            {
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }

            return channels;
        }


        public void SaveProcessIds(Dictionary<string, string> ids)
        {
            BinaryFormatter formatter = new BinaryFormatter();

           FileStream writer = null;
            try
            {
                writer = System.IO.File.Create(Server.MapPath(@"~\TV\streamIds.txt"));
                formatter.Serialize(writer, ids);
            }
            catch (Exception e)
            {
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                    writer.Dispose();
                }
            }
        }

        public void AddProcessId(int id)
        {
            var prc = GetProcessIds();

            if (!prc.ContainsKey(Request.UserHostAddress))
            {
                prc.Add(Request.UserHostAddress, id.ToString());
            }
            else
            {
                prc[Request.UserHostAddress] = id.ToString();
            }

            SaveProcessIds(prc);
        }

        public void RemoveProcessId()
        {
            var prc = GetProcessIds();

            if (prc.ContainsKey(Request.UserHostAddress))
            {
                prc.Remove(Request.UserHostAddress);
            }

            SaveProcessIds(prc);
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
            var folders = System.IO.File.ReadAllLines(Server.MapPath(@"~\Folders.txt"));

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

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UploadFile(HttpPostedFileBase[] files, string source)
        {
            ViewBag.Message = "Your contact page.";
            source = "";
            var folders = System.IO.File.ReadAllLines(Server.MapPath(@"~\Folders.txt"));

            if (string.IsNullOrWhiteSpace(source))
            {
                var folder = folders.Last();
                SaveFiles(files, folder);
            }

            foreach (var folder in folders)
            {
                if (source.Contains(folder))
                {
                    SaveFiles(files, folder);
                }
            }

            return View("Contact");
        }

        public void SaveFiles(HttpPostedFileBase[] files, string folder)
        {
            foreach (HttpPostedFileBase file in files)
            {
                var fi = new FileInfo(folder + "\\" + file.FileName);

                if (fi.Extension == ".torrent")
                {
                    file.SaveAs(HttpContext.Server.MapPath("~") + "1.torrent");
                }
                else
                {
                    file.SaveAs(folder + "\\" + file.FileName);
                }
            }
        }
    }
}
