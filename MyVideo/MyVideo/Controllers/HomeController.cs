using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.Routing;
using log4net;
using MyVideo.Models;
using WebGrease.Css.Extensions;

namespace MyVideo.Controllers
{
    public class HomeController : Controller
    {
        private static readonly log4net.ILog log1 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
            try
            {
                UrlExtensions.Prepend(Server.MapPath(@"~\TV\log.txt"), Environment.NewLine + DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString() + ": " + source + ": " + Request.UserHostAddress);
            }
            catch{}
            

            var folders = System.IO.File.ReadAllLines(Server.MapPath(@"~\Folders.txt"));
            var model = new FolderModel();
            model.Folder = new Dictionary<string, string>();
            model.SRC = Source;
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
                    else if (System.IO.File.Exists(Source + source))
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
                        if (source.Contains("игореша") || source.Contains("Igorosa"))
                        {
                            var filesException =
                                di.GetFiles()
                                    .Where(
                                        w =>
                                            (new DirectoryInfo(Source)).GetFiles().Select(s => s.Name).Contains(w.Name) ==
                                            false);

                            
                            foreach (var file in (new DirectoryInfo(Source)).GetFiles().Where(w=>w.Extension == ".mp4"))
                            {
                                model.Folder.Add(file.FullName, file.Name);
                            }
                            foreach (var file in filesException.OrderBy(o => o.CreationTime))
                            {
                                model.Folder.Add(file.FullName, file.Name);
                            }
                        }

                        
                        else
                        {
                            foreach (var file in di.GetFiles())
                            {
                                model.Folder.Add(file.FullName, file.Name);
                            }
                        }
                        

                        model.ParentFolder = di.Parent.FullName;
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GetStream(string source, string offset, string fileFormat, string bitrate, bool isEmbed, int soundNumber, bool isVlc, bool isStream)
        {
            try
            {
                UrlExtensions.Prepend(Server.MapPath(@"~\TV\log.txt"), Environment.NewLine + DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString() + ": " + source + ": " + Request.UserHostAddress);
            }
            catch{}
            

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
                

                offset = (Int32.Parse(offset) * 60).ToString();
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
                                Process prc = Process.GetProcessById(Int32.Parse(pr.Value));
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
                        line = String.Format(
                            @"-i ""{0}"" -b {1}k -vf ""scale=400:trunc(ow/a/2)*2"" -loglevel quiet -f mpegts tcp://a.mosalsky.com:2042?listen",
                            source, bitrate);
                    }
                    else
                    {
                        outputFile = "TV.txt";

                        line = String.Format(
                            @"-i ""{0}"" -b {1}k -c:a libfdk_aac -vbr 3 -vf ""scale=400:trunc(ow/a/2)*2"" -loglevel quiet -f flv -vcodec libx264 rtmp://a.mosalsky.com:1935/live/" + Request.UserHostAddress,
                            source, bitrate);
                    }
                }
                else
                if (fileFormat == "flv")
                {
                    if (isStream)
                    {
                        isUri = true;
                        line = String.Format(
                            @"-i ""{0}"" -b {1}k -c:a libfdk_aac -vbr 3 -vf ""scale=400:trunc(ow/a/2)*2"" -loglevel quiet -f flv -vcodec libx264 rtmp://a.mosalsky.com:1935/live/" + Request.UserHostAddress,
                            source, bitrate);
                        outputFile = "TV.txt";
                    }
                    else
                    {
                        outputFile = fi.Name.Replace(fi.Extension, "") + ".flv";
                        line = String.Format(
                            @"-i ""{0}"" -ss {2} -async 1 -c:a libfdk_aac -vbr 3 -b {3}k -vf ""scale=400:trunc(ow/a/2)*2"" -map 0:0  -map 0:{4} -v 0 -f flv -vcodec libx264 ""{1}""",
                            source, Source + outputFile, offset, bitrate, soundNumber);
                    }
                }
                else if (fileFormat == "mp4")
                {
                    if (isStream)
                    {
                        isUri = true;
                        line = String.Format(
                            @"-re -i ""{0}"" -ss {2} -b {1}k -c:a libfdk_aac -vbr 3 -vf ""scale=400:trunc(ow/a/2)*2"" -loglevel quiet -f flv -vcodec libx264 rtmp://a.mosalsky.com:1935/live/" + Request.UserHostAddress,
                            source, bitrate, offset);
                        outputFile = "TV.txt";
                    }
                    else
                    {
                        outputFile = fi.Name.Replace(fi.Extension, "." + fileFormat);
                        //outputFile = fi.Name.Remove(fi.Name.Length - fi.Extension.Length, fi.Name.Length) + "." + fileFormat;
                        line = String.Format(
                            @"-i ""{0}"" -ss {2} -b {3}k -acodec mp3 -vf ""scale=400:trunc(ow/a/2)*2"" -map 0:0 -map 0:{4} -vcodec h264 ""{1}""",
                            source, Source + outputFile, offset, bitrate, soundNumber);
                        otput.Add(myproc, Source + outputFile);
                    }
                }
                else
                {
                    outputFile = fi.Name.Replace(fi.Extension, "") + "." + fileFormat;
                    line = String.Format(
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

                log1.Debug("tut");

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

                myproc.Exited += myproc_Exited;
                log1.Debug("myproc.HasExited: " + myproc.HasExited);
                log1.Debug("myProcessStartInfo.Arguments: " + myProcessStartInfo.Arguments);
                

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

        private void GetOutput(FileInfo fileInfo)
        {
            
        }

        void myproc_Exited(object sender, EventArgs e)
        {
            log1.Debug("exited");
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
            log1.Debug("error");
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

        public static string GetProcessPercentage(string file)
        {
            var res = "no percentage data";

            try
            {
                FileInfo fi = new FileInfo(file);

                string data = System.IO.File.ReadAllText(file);
                if (data.Contains("final ratefactor") || data.Contains("Chapter") || (DateTime.Now - fi.LastWriteTime).TotalMinutes > 2)
                {
                    res = "100";
                }
                else
                {
                    var allTime = GetTime(data, data.IndexOf(":", data.IndexOf("Duration:") + 10));

                    
                    var currTime = GetTime(data, data.LastIndexOf(":") - 3);

                    var percentage = (currTime.TotalMilliseconds / allTime.TotalMilliseconds) * 100;
                    res = Convert.ToInt32(percentage).ToString();

                    //if (data.Split(new[] {"frame=  "}, StringSplitOptions.None).Length - 1 > 20)
                    //{
                    //    var cnt = 0;
                    //    int i = data.Length - 3;
                    //    for (; i >= 0; i--)
                    //    {
                    //        if (data[i] == '\n')
                    //        {
                    //            cnt++;
                    //        }

                    //        if (cnt == 21)
                    //        {
                    //            break;
                    //        }
                    //    }

                    //    for (; i >= 0; i--)
                    //    {
                    //        if (data[i] == ':')
                    //        {
                    //            break;
                    //        }
                    //    }

                    //    var minus20SecTime = GetTime(data, i - 3);


                    //}
                }
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

            return new TimeSpan(Int32.Parse(hours), Int32.Parse(minutes), Int32.Parse(seconds));
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

            if (String.IsNullOrWhiteSpace(source))
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

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Mp3File(string source, string bitrate)
        {
            var files = Directory.GetFiles(source);

            if (!Directory.Exists(source + "/Converted"))
            {
                Directory.CreateDirectory(source + "/Converted");
            }

            foreach (var file in files)
            {
                var fi = new FileInfo(file);

                if (fi.Extension.ToLower() == ".mp3")
                {
                    var outputFile = String.Format(@"{0}/Converted/{1}", source,
                        fi.Name.Replace(fi.Extension, ".aac"));

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

                    StartMp3Convert(String.Format(@"-i ""{0}"" -c:a libfdk_aac -b:a {2}k -loglevel quiet ""{1}""", file, outputFile, bitrate));
                }
            }

            return RedirectToAction("Index", new RouteValueDictionary() { { "source", source } });
        }

        public void StartMp3Convert(string line)
        {
            var myproc = new Process();

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
