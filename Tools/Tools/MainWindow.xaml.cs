using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DateTimeExtensions.TimeOfDay;
using ProcessPrivileges;
using Tools;

namespace Deleter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Thread th;

        public MainWindow()
        {
            InitializeComponent();
            th = new Thread(DeleteFolder);

        }

        private void BtnSelDirectory_OnClick(object sender, RoutedEventArgs e)
        {
            var diag = new System.Windows.Forms.FolderBrowserDialog();
            if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tbPath.Text = diag.SelectedPath;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!IsRunAsAdmin)
            {
                ProcessStartInfo proc = new ProcessStartInfo();
                proc.UseShellExecute = true;
                proc.WorkingDirectory = Environment.CurrentDirectory;
                proc.FileName = System.Reflection.Assembly.GetEntryAssembly().Location;
                proc.Verb = "runas";

                try
                {
                    Process.Start(proc);
                }
                catch
                {
                    // The user refused to allow privileges elevation.
                    // Do nothing and return directly ...
                    return;
                }

                Application.Current.Shutdown();
            }
            else
            {
                DirectoryInfo di = new DirectoryInfo(tbPath.Text);
                th = new Thread(DeleteFolder);
                th.Start(di);
            }
        }

        public bool IsRunAsAdmin
        {
            get
            {
                WindowsIdentity id = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(id);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        private void DeleteFolder(object dir)
        {
            using (new ProcessPrivileges.PrivilegeEnabler(Process.GetCurrentProcess(), Privilege.TakeOwnership))
            {
                var directoryInfo = (DirectoryInfo) dir;

                GrantAccess(directoryInfo.FullName);
                try
                {
                    foreach (DirectoryInfo d in directoryInfo.GetDirectories())
                    {
                        this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background,
                            new System.Windows.Threading.DispatcherOperationCallback(delegate
                            {

                                tbLog.Text = "Deleting " + d.FullName + Environment.NewLine;

                                return null;
                            }), null);
                        DeleteFolder(d);
                    }
                }
                catch
                {
                    this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background,
                        new System.Windows.Threading.DispatcherOperationCallback(delegate
                        {

                            //tbLog.Text = "ERROR " + Environment.NewLine;

                            return null;
                        }), null);
                }

                try
                {
                    foreach (FileInfo f in directoryInfo.GetFiles())
                    {
                        GrantAccess(f.FullName);
                        try
                        {
                            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background,
                                new System.Windows.Threading.DispatcherOperationCallback(delegate
                                {

                                    tbLog.Text = "Deleting " + f.FullName + Environment.NewLine;


                                    return null;
                                }), null);
                            f.Delete();
                        }
                        catch
                        {
                            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background,
                                new System.Windows.Threading.DispatcherOperationCallback(delegate
                                {

                                    //tbLog.Text = "ERROR " + Environment.NewLine;

                                    return null;
                                }), null);
                        }
                    }
                }
                catch
                {
                }

                try
                {
                    this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background,
                        new System.Windows.Threading.DispatcherOperationCallback(delegate
                        {

                            tbLog.Text = "Deleting " + directoryInfo.FullName + Environment.NewLine;

                            return null;
                        }), null);
                    directoryInfo.Delete(true);
                }
                catch
                {
                    this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background,
                        new System.Windows.Threading.DispatcherOperationCallback(delegate
                        {

                            //tbLog.Text = "ERROR " + directoryInfo.FullName + Environment.NewLine;

                            return null;
                        }), null);
                }
            }
        }

        private static void GrantAccess(string filepath)
        {
            FileSecurity fs;

            try
            {
                fs = File.GetAccessControl(filepath);
            }
            catch
            {
                fs = new FileSecurity();
            }

            //var sid = fs.GetOwner(typeof(SecurityIdentifier));
            var ntAccount = new NTAccount(Environment.UserDomainName, Environment.UserName);
            try
            {
                fs.SetOwner(ntAccount);
                File.SetAccessControl(filepath, fs);

                var currentRules = fs.GetAccessRules(true, false, typeof (NTAccount));
                var newRule = new FileSystemAccessRule(
                    ntAccount, FileSystemRights.FullControl,
                    AccessControlType.Allow);
                fs.AddAccessRule(newRule);
                File.SetAccessControl(filepath, fs);

                File.SetAttributes(filepath, FileAttributes.Normal);
            }
            catch
            {
            }
            finally
            {
                fs = null;
                ntAccount = null;
            }
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (th.IsAlive)
                th.Abort();
        }

        private void BtnStop_OnClick(object sender, RoutedEventArgs e)
        {
            if (th.IsAlive)
                th.Abort();
        }

        private void TbSalary_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                List<SalaryPerMonth> salPerMonths = new List<SalaryPerMonth>();


                var salar = double.Parse(tbSalary.Text)*8;
                var dependants = int.Parse(tbDependants.Text);
                double result = 0;

                var currentYear = new DateTime(int.Parse(tbYear.Text), 1, 1);
                var nextYear = currentYear.AddYears(1);
                var curMonth = currentYear.Month;
                SalaryPerMonth curSalPer = new SalaryPerMonth()
                {
                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(1) + ": ",
                    DependentCount = dependants
                };

                var holidays = 0;
                var weekEnds = 0;
                var hours = 0;
                var workingDays = 0;

                while (currentYear < nextYear)
                {
                    if (!IsHoliday(currentYear))
                    {
                        workingDays++;
                        hours += 8;
                        result += salar;
                        curSalPer.Salary += salar;
                    }

                    if (currentYear.DayOfWeek == DayOfWeek.Sunday || currentYear.DayOfWeek == DayOfWeek.Saturday)
                    {
                        weekEnds++;
                    }
                    else if (ci.IsHoliday(currentYear))
                    {
                        holidays++;
                    }

                    currentYear = currentYear.AddDays(1);
                    if (curMonth != currentYear.Month)
                    {
                        curSalPer.Holidays = holidays;
                        curSalPer.WeekEnds = weekEnds;
                        curSalPer.Hours = hours;
                        curSalPer.WorkingDays = workingDays;
                        workingDays = hours = weekEnds = holidays = 0;
                        curMonth = currentYear.Month;
                        salPerMonths.Add(curSalPer);
                        curSalPer = new SalaryPerMonth()
                        {
                            Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(curMonth) + ": ",
                            DependentCount = dependants
                        };
                    }
                }

                double sal = result;
                sal /= 12;

                salPerMonths.Add(new SalaryPerMonth()
                {
                    AutoCalculateTaxSalary = false,
                    Month = "Total: ",
                    Salary = salPerMonths.Sum(s => s.Salary),
                    Holidays = salPerMonths.Sum(s => s.Holidays),
                    Hours = salPerMonths.Sum(s => s.Hours),
                    TaxSalary = salPerMonths.Sum(s => s.TaxSalary),
                    WeekEnds = salPerMonths.Sum(s => s.WeekEnds),
                    WorkingDays = salPerMonths.Sum(s => s.WorkingDays)
                });

                tbSalaryResult.Text = ((int) sal).ToString();
                lbMonths.ItemsSource = salPerMonths;

                tbTaxSalaryResult.Text = ((int) MinusTax(sal, dependants)).ToString();

                tbSalaryForWorkedDays.Text =
                    MinusTax(double.Parse(tbWorkedHours.Text)*double.Parse(tbSalary.Text), dependants).ToString();
            }
            catch (Exception)
            {
                try
                {
                    tbSalaryResult.Text = "Wrong number";
                }
                catch (Exception)
                {
                }
            }
        }

        DateTimeExtensions.WorkingDays.WorkingDayCultureInfo ci =
            new DateTimeExtensions.WorkingDays.WorkingDayCultureInfo("lv-LV");

        public bool IsHoliday(DateTime dt)
        {
            return !ci.IsWorkingDay(dt) || ci.IsHoliday(dt);
        }

        public double MinusTax(double salary, int dependentCount)
        {
            var notTaxed = 75 + (165*dependentCount);
            var obligate = (salary*0.105);
            var livTax = (salary - obligate - notTaxed)*0.23;

            return salary - obligate - livTax;
        }

        private void BtnSelDirectoryRenamer_OnClick(object sender, RoutedEventArgs e)
        {
            var diag = new System.Windows.Forms.FolderBrowserDialog();
            if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tbPathRenamer.Text = diag.SelectedPath;
            }
        }

        private void btnRename_Click(object sender, RoutedEventArgs e)
        {
            var di = new DirectoryInfo(tbPathRenamer.Text);
            var counter = 0;

            foreach (var file in di.GetFiles())
            {
                File.Move(file.FullName,
                    file.Directory.FullName + "\\" +
                    string.Concat(Enumerable.Repeat("0", 5 - counter.ToString().Length).ToArray()) + counter +
                    file.Extension);

                counter++;
            }
        }

        private CultureInfo _enCulture = new CultureInfo("en-US");

        private void TestBase_OnClick(object sender, RoutedEventArgs e)
        {
            var res = "<table>";

            var endYear = 2100;
            var currYear = 0;
            var currDate = new DateTime(2016, 1, 1);
            //CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(1)
            var curMonth = string.Empty;
            var hasMonth = false;

            while (currDate.Year != endYear)
            {
                if (currYear != currDate.Year)
                {
                    currYear = currDate.Year;
                    res += "<tr><td><br>" + currYear + "</td></tr>";
                }

                if (IsHoliday(curDateTime) && curDateTime.DayOfWeek != DayOfWeek.Sunday &&
                    curDateTime.DayOfWeek != DayOfWeek.Saturday)
                {
                    if (hasMonth)
                    {
                        res += "</td></tr>";
                    }

                    hasMonth = false;
                    curMonth = _enCulture.DateTimeFormat.GetMonthName(currDate.Month);
                }

                if (ci.IsHoliday(currDate))
                {
                    if (!hasMonth)
                    {
                        res += "<tr><td>" + curMonth;
                        hasMonth = true;
                    }

                    res += res.EndsWith("<tr><td>" + curMonth) ? " " + HolidayStringDay(currDate) : ", " + HolidayStringDay(currDate);
                }

                currDate = currDate.AddDays(1);
            }

            var res = yearData.Where(w => w.Value <= 7)
                .Aggregate("", (current, i) => current + i.Key + ": " + i.Value + Environment.NewLine);

            File.WriteAllText(@"C:\Temp\1.html", res);

            MessageBox.Show("complete");
            //ci.IsHoliday()

            //var curDateTime = new DateTime(2016, 1, 1);
            //var endDate = curDateTime.AddYears(100).AddDays(-1);
            //var yearData = new Dictionary<int, int>();

            //while (curDateTime <= endDate)
            //{
            //    if (!yearData.ContainsKey(curDateTime.Year))
            //    {
            //        yearData.Add(curDateTime.Year, 0);
            //    }

            //    if (IsHoliday(curDateTime) && curDateTime.DayOfWeek != DayOfWeek.Sunday && curDateTime.DayOfWeek != DayOfWeek.Saturday)
            //    {
            //        yearData[curDateTime.Year] ++;
            //    }

            //    curDateTime = curDateTime.AddDays(1);
            //}

            //var res = yearData.Where(w=>w.Value<=7).Aggregate("", (current, i) => current + i.Key + ": " + i.Value + Environment.NewLine);

            //MessageBox.Show(res);
        }

        #region Service

        private bool _serviceStarted = false;
        HttpListener _httpListener;
        public string _list;
        public DateTime _lastUpDateTime = DateTime.MinValue;


        public string TTVList
        {
            get
            {
                if (_lastUpDateTime <= DateTime.Now.AddMinutes(-5))
                {
                    _lastUpDateTime = DateTime.Now;
                    using (WebClient myWebClient = new WebClient())
                    {
                        _list = myWebClient.DownloadString(
                            "http://super-pomoyka.us.to/trash/ttv-list/ttv.m3u");
                    }
                }

                return _list;
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            _httpListener = new HttpListener();
            _serviceStarted = true;
            string url = "http://*";
            string port = "1983";
            string prefix = $"{url}:{port}/";
            _httpListener.Prefixes.Add(prefix);
            _httpListener.Start();
            var previousPath = string.Empty;

            Thread httpThread = new Thread(() =>
            {
                HttpListenerContext context = null;
                Thread responseThread = null;
                while (_serviceStarted)
                {
                    try
                    {
                        context = _httpListener.GetContext();
                    }
                    catch (Exception)
                    {
                    }

                    


                    if (_serviceStarted)
                    {
                        HttpListenerRequest request = context.Request;
                        HttpListenerResponse response = context.Response;

                        Stream inputStream = request.InputStream;
                        Encoding encoding = request.ContentEncoding;
                        StreamReader reader = new StreamReader(inputStream, encoding);
                        //var requestBody = reader.ReadToEnd();
                        response.StatusCode = (int) HttpStatusCode.OK;



                        var chanellsString = TTVList;
                                

                            byte[] bytes = Encoding.Default.GetBytes(chanellsString);
                            chanellsString = Encoding.UTF8.GetString(bytes);

                            if (request.Url.AbsolutePath.ToLower() == "/tv.m3u")
                            {
                                using (StreamWriter stream = new StreamWriter(response.OutputStream))
                                {
                                    var res = "";

                                    var chanelList = GetSubStrings(chanellsString, "#EXTINF:-1,", "");
                                    res += "#EXTM3U" + Environment.NewLine;

                                    foreach (var s in chanelList)
                                    {
                                        res +=
                                            $@"#EXTINF:-1,{s}
http://{request.Url.Authority}/{ToHexString(s)}
";
                                    }

                                    stream.Write(res);

                                    stream.Flush();
                                    stream.Close();
                                }
                            }
                            else
                            {
                                var path = FromHexString(request.Url.AbsolutePath.Substring(1));

                                if (!string.IsNullOrEmpty(path))
                                {
                                    responseThread?.Abort();

                                    if (previousPath != path || Process.GetProcessesByName("ace_player").Length == 0)
                                    {
                                        previousPath = path;

                                        foreach (Process proc in Process.GetProcessesByName("ace_player"))
                                        {
                                            proc.Kill();
                                        }

                                        string regex =
                                            $"{Regex.Escape("#EXTINF:-1," + path)}\n{Regex.Escape("acestream://")}(.*?)\n";

                                        var matches = Regex.Matches(chanellsString, regex, RegexOptions.Singleline);
                                        var arguments =
                                            $" --qt-start-minimized {matches[0].Groups[1].Value} :sout=#http{{mux=ffmpeg{{mux=flv}},dst=:1999/}} :sout-keep";

                                        try
                                        {
                                            Process.Start(@"C:\Users\dron\AppData\Roaming\ACEStream\player\ace_player.exe",
                                                arguments);
                                        }
                                        catch (Exception exception)
                                        {
                                        }
                                    }

                                    
                                    

                                    

                                    responseThread = new Thread(() =>
                                    {
                                        using (HttpWebResponse vlcResponse = GetVlcRequest())
                                        {

                                            if (vlcResponse != null)
                                            {
                                                byte[] buffer = new byte[4096];
                                                using (var vlcStream = vlcResponse.GetResponseStream())
                                                {
                                                    var noError = true;

                                                    while (noError)
                                                    {
                                                        vlcStream.Flush();
                                                        int bytesRead = vlcStream.Read(buffer, 0, buffer.Length);
                                                        vlcStream.Flush();

                                                        if (bytesRead == 0)
                                                        {
                                                            //vlcResponse = GetVlcRequest();
                                                            //vlcStream = vlcResponse.GetResponseStream();
                                                            Thread.Sleep(10);
                                                        }
                                                        try
                                                        {
                                                            response.OutputStream.Flush();
                                                            response.OutputStream.Write(buffer, 0, bytesRead);
                                                            response.OutputStream.Flush();
                                                        }
                                                        catch (Exception exc1)
                                                        {
                                                            File.AppendAllText("test.txt",
                                                                DateTime.Now.ToString("o") + ": ERROR: " + exc1.Message +
                                                                Environment.NewLine + exc1.InnerException?.Message +
                                                                Environment.NewLine);

                                                            noError = false;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    });

                                    responseThread.Start();
                                }
                            }
                        }
                    }
                


            });
            
            httpThread.Start();
        }

        private HttpWebResponse GetVlcRequest()
        {
            var counter = 0;
            var vlcHasError = true;

            HttpWebRequest vlcRequest = null;

            
            HttpWebResponse vlcResponse = null;

            while (counter <= 60 && vlcHasError)
            {
                try
                {
                    vlcRequest = (HttpWebRequest)WebRequest.Create(@"http://localhost:1999");
                    vlcRequest.Timeout = 20000;
                    vlcResponse = (HttpWebResponse)vlcRequest.GetResponse();
                    vlcHasError = false;
                }
                catch (Exception)
                {
                    Thread.Sleep(1000);
                }

                counter++;
            }

            return vlcHasError ? null : vlcResponse;
        }

        public static string ToHexString(string str)
        {
            var sb = new StringBuilder();

            var bytes = Encoding.Unicode.GetBytes(str);
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString(); // returns: "48656C6C6F20776F726C64" for "Hello world"
        }

        public static string FromHexString(string hexString)
        {
            string res = null;

            try
            {
                var bytes = new byte[hexString.Length / 2];
                for (var i = 0; i < bytes.Length; i++)
                {
                    bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
                }

                res = Encoding.Unicode.GetString(bytes); // returns: "Hello world" for "48656C6C6F20776F726C64"
            }
            catch (Exception)
            {
                
            }

            return res;
        }

        public static IEnumerable<string> GetSubStrings(string text, string start, string end)
        {
            string regex = $"{Regex.Escape(start)}(.*){Regex.Escape(end)}";

            return Regex.Matches(text, regex, RegexOptions.Multiline)
                .Cast<Match>()
                .Select(match => match.Groups[1].Value);
        }

        private void Stop_OnClick(object sender, RoutedEventArgs e)
        {
            _serviceStarted = false;
            _httpListener?.Stop();
        }

        #endregion

        private string HolidayStringDay(DateTime date)
        {
            if (ci.IsHoliday(date) && date.DayOfWeek != DayOfWeek.Sunday && date.DayOfWeek != DayOfWeek.Saturday)
            {
                return "<span style='color: red'>" + date.Day + "</span>";
            }

            return date.Day.ToString();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var t1 = new TimeSpan(int.Parse(tbH1.Text), int.Parse(tbM1.Text), 0);
            var t2 = new TimeSpan(int.Parse(tbH2.Text), int.Parse(tbM2.Text), 0);

            var res = t1 - t2;

            tbRes.Text = res.Hours + ":" + res.Minutes;
        }

        private void BtnNavigateToSite_OnClick(object sender, RoutedEventArgs e)
        {
            wbSS.Navigate(tbAddress.Text);
        }

        private void BtnStartRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnStopRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
