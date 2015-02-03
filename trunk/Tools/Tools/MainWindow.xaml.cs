using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
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
            DirectoryInfo di = new DirectoryInfo(tbPath.Text);
            th = new Thread(DeleteFolder);
            th.Start(di);
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
                double result = 0;

                var currentYear = new DateTime(int.Parse(tbYear.Text), 1, 1);
                var nextYear = currentYear.AddYears(1);
                var curMonth = currentYear.Month;
                SalaryPerMonth curSalPer = new SalaryPerMonth()
                {
                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(1) + ": "
                };

                while (currentYear < nextYear)
                {
                    if (currentYear.DayOfWeek != DayOfWeek.Sunday && currentYear.DayOfWeek != DayOfWeek.Saturday)
                    {
                        result += salar;
                        curSalPer.Salary += salar;
                    }
                    currentYear = currentYear.AddDays(1);
                    if (curMonth != currentYear.Month)
                    {
                        curMonth = currentYear.Month;
                        salPerMonths.Add(curSalPer);
                        curSalPer = new SalaryPerMonth()
                        {
                            Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(curMonth) + ": "
                        };
                    }
                }

                double sal = result/12;

                tbSalaryResult.Text = ((int)sal).ToString();
                lbMonths.ItemsSource = salPerMonths;

                tbTaxSalaryResult.Text = ((int) MinusTax(sal)).ToString();
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

        public double MinusTax(double salary)
        {
            salary = salary - 75;
            return salary - ((salary/100)*10.5) - ((salary/100)*24) + 75;
        }

        private void BtnSelDirectoryRenamer_OnClick(object sender, RoutedEventArgs e)
        {
            var diag = new System.Windows.Forms.FolderBrowserDialog();
            if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tbPathRenamer.Text = diag.SelectedPath;
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var di = new DirectoryInfo(tbPathRenamer.Text);
            var counter = 0;

            foreach (var file in di.GetFiles())
            {
                File.Move(file.FullName, file.Directory.FullName + "\\" + string.Concat(Enumerable.Repeat("0", 5 - counter.ToString().Length).ToArray()) + counter + file.Extension);
                
                counter++;
            }
        }
    }
}
