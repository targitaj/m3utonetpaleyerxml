using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
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

                var currentRules = fs.GetAccessRules(true, false, typeof(NTAccount));
                var newRule = new FileSystemAccessRule(
                    ntAccount, FileSystemRights.FullControl,
                    AccessControlType.Allow);
                fs.AddAccessRule(newRule);
                File.SetAccessControl(filepath, fs);

                File.SetAttributes(filepath, FileAttributes.Normal);
            }
            catch { }
            finally { fs = null; ntAccount = null; }
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


                var salar = double.Parse(tbSalary.Text) * 8;
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
                
                tbSalaryResult.Text = ((int)sal).ToString();
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

        DateTimeExtensions.WorkingDays.WorkingDayCultureInfo ci = new DateTimeExtensions.WorkingDays.WorkingDayCultureInfo("lv-LV");

        public bool IsHoliday(DateTime dt)
        {
            return !ci.IsWorkingDay(dt) || ci.IsHoliday(dt);
        }

        public double MinusTax(double salary, int dependentCount)
        {
            var notTaxed = 75 + (165 * dependentCount);
            var obligate = (salary * 0.105);
            var livTax = (salary - obligate - notTaxed) * 0.23;

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
                File.Move(file.FullName, file.Directory.FullName + "\\" + string.Concat(Enumerable.Repeat("0", 5 - counter.ToString().Length).ToArray()) + counter + file.Extension);
                
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

                if (curMonth != _enCulture.DateTimeFormat.GetMonthName(currDate.Month))
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

            res += "</td></tr></table>";

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
    }
}
