using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public partial class Form1 : Form
    {
        

        public Form1()
        {
            InitializeComponent();
        }
        private static Regex r = new Regex(":");
        private string mainText = "";

        private void Process()
        {
            
            if (!cbxOffset1.Checked)
            {
                mainText = "Копирование файлов из первой дирректории";
                CopyFiles(lblDir1.Text, lblDestDir.Text);
            }
            else
            {
                mainText = "Копирование файлов из первой дирректории и добавление сдвига";
                CopyWithOffsetFiles(lblDir1.Text, lblDestDir.Text, (int)nudHours1.Value, (int)nudMinutes1.Value,
                                   (int)nudYaer1.Value, (int)nudMonth1.Value, (int)nudDay1.Value);
            }

            if (!cbxOffset2.Checked)
            {
                mainText = "Копирование файлов из второй дирректории";
                CopyFiles(lblDir2.Text, lblDestDir.Text);
            }
            else
            {
                mainText = "Копирование файлов из второй дирректории и добавление сдвига";
                CopyWithOffsetFiles(lblDir2.Text, lblDestDir.Text, (int) nudHours2.Value, (int) nudMinutes2.Value,
                                    (int) nudYaer2.Value, (int) nudMonth2.Value, (int) nudDay2.Value);
            }

            string dir = lblDestDir.Text;
            string to = lblDestDir.Text + "\\";
            string[] files = Directory.GetFiles(dir);
            List<FileInfo> filesFI = files.Select(s => new FileInfo(s)).ToList();
            Dictionary<DateTime, FileInfo> res = new Dictionary<DateTime, FileInfo>();


            foreach (var f in filesFI)
            {
                Bitmap image = null;
                try
                {
                    image = new Bitmap(f.FullName);
                    PropertyItem propItem = image.GetPropertyItem(36867);

                    string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                    DateTime dt = DateTime.Parse(dateTaken);

                    while (res.ContainsKey(dt))
                        dt = dt.AddSeconds(1);

                    res.Add(dt, f);

                    image.Dispose();
                }
                catch
                {
                    Invoke(new MethodInvoker(delegate
                    {
                        lblError.Text = "Произошла ошибка при сортировки, файлы с ошибками помечены как 0700_PR.jpg";
                    }));

                    string filePath = to + "_ERROR" + Guid.NewGuid().ToString();

                    if (image != null)
                        image.Dispose();
                    f.MoveTo(filePath);

                    DateTime lastDate = res.Last().Key.AddSeconds(1);
                    

                    res.Add(lastDate, new FileInfo(filePath));
                }

                Invoke(new MethodInvoker(delegate
                {
                    lblProgress.Text = "Сортировка файлов " + (filesFI.IndexOf(f) + 1) + "/" + filesFI.Count;
                    progressBar1.Value = (int)((Convert.ToDouble((filesFI.IndexOf(f) + 1)) / Convert.ToDouble(filesFI.Count)) * 1000);
                }));
            }

            int counter = 1;

            filesFI = res.OrderBy(k => k.Key).Select(k => k.Value).ToList();

            foreach (var f in filesFI)
            {
                string saveTO = to + (counter < 10 ? "000" + counter : (counter < 100 ? "00" + counter : (counter < 1000 ? "0" + counter : counter.ToString()))) + (f.Name.Contains("_ERROR") ? "_PR" : "") + ".jpg";
                f.MoveTo(saveTO);
                counter++;

                Invoke(new MethodInvoker(delegate
                {
                    lblProgress.Text = "Переименование файлов " + (filesFI.IndexOf(f) + 1) + "/" + filesFI.Count;
                    progressBar1.Value = (int)((Convert.ToDouble((filesFI.IndexOf(f) + 1)) / Convert.ToDouble(filesFI.Count)) * 1000);
                }));
            }

            Invoke(new MethodInvoker(delegate
            {
                btnSplit.Text = "Соединить и отсортировать";
                lblProgress.Text = "Конец";
                progressBar1.Value = 0;
            }));
            
        }

        private void CopyFiles(string from, string to)
        {
            string[] files = Directory.GetFiles(from);
            List<FileInfo> filesFI = files.Select(s => new FileInfo(s)).ToList();

            foreach (var file in filesFI)
            {
                file.CopyTo(to + "\\" + Guid.NewGuid().ToString());

                Invoke(new MethodInvoker(delegate
                    {
                        lblProgress.Text = mainText + " " + (filesFI.IndexOf(file) + 1) + "/" + filesFI.Count;
                        progressBar1.Value = (int) ((Convert.ToDouble((filesFI.IndexOf(file) + 1)) / Convert.ToDouble(filesFI.Count)) * 1000);
                    }));
            }
        }

        private void CopyWithOffsetFiles(string from, string to, int hours, int minutes, int years, int months, int days)
        {
            string[] files = Directory.GetFiles(from);
            List<FileInfo> filesFI = files.Select(s => new FileInfo(s)).ToList();

            foreach (var file in filesFI)
            {
                string saveTO = to + "\\" + Guid.NewGuid().ToString();

                try
                {
                    Bitmap image = new Bitmap(file.FullName);
                    PropertyItem propItem = image.GetPropertyItem(36867);

                    string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                    DateTime dt = DateTime.Parse(dateTaken);

                    //dt = dt.AddHours(11).AddMinutes(23).AddYears(7).AddMonths(5).AddDays(-6);
                    dt = dt.AddHours(hours).AddMinutes(minutes).AddYears(years).AddMonths(months).AddDays(days);

                    byte[] aa = Encoding.UTF8.GetBytes(dt.ToString("yyyy:MM:dd HH:mm:ss") + "\0");
                    propItem.Value = aa;

                    image.SetPropertyItem(propItem);

                    image.Save(saveTO);
                    image.Dispose();
                    //f.CopyTo(to + (counter < 10 ? "0" + counter : counter.ToString()) + ".jpg");
                }
                catch
                {
                    Invoke(new MethodInvoker(delegate
                    {
                        lblError.Text = "Произашла ошибка при добавленни сдвига во времени, файлы с ошибками помечены как ERROR_0700.jpg";
                    }));
                    file.CopyTo(saveTO + "_ERROR");
                }

                Invoke(new MethodInvoker(delegate
                {
                    lblProgress.Text = mainText + " " + (filesFI.IndexOf(file) + 1) + "/" + filesFI.Count;
                    progressBar1.Value = (int)((Convert.ToDouble((filesFI.IndexOf(file) + 1)) / Convert.ToDouble(filesFI.Count)) * 1000);
                }));
            }
        }

        private Thread thr = null;

        private void button1_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            if (thr == null || !thr.IsAlive)
            {
                thr = new Thread(new ThreadStart(Process));
                thr.Start();
                btnSplit.Text = "Стоп";
            }
            else
            {
                thr.Abort();
                btnSplit.Text = "Соединить и отсортировать";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            dialog.ShowDialog(this);

            if (!string.IsNullOrWhiteSpace(dialog.SelectedPath))
            {
                lblDir1.Text = dialog.SelectedPath;
            }

            EnableSplitButton();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            dialog.ShowDialog(this);

            if (!string.IsNullOrWhiteSpace(dialog.SelectedPath))
            {
                lblDir2.Text = dialog.SelectedPath;
            }

            EnableSplitButton();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            dialog.ShowDialog(this);

            if (!string.IsNullOrWhiteSpace(dialog.SelectedPath))
            {
                lblDestDir.Text = dialog.SelectedPath;
            }

            EnableSplitButton();
        }

        private void cbxOffset1_CheckedChanged(object sender, EventArgs e)
        {
            nudYaer1.Enabled =
                nudMonth1.Enabled = nudDay1.Enabled = nudHours1.Enabled = nudMinutes1.Enabled = cbxOffset1.Checked;
        }

        private void cbxOffset2_CheckedChanged(object sender, EventArgs e)
        {
            nudYaer2.Enabled =
                nudMonth2.Enabled = nudDay2.Enabled = nudHours2.Enabled = nudMinutes2.Enabled = cbxOffset2.Checked;
        }

        private void EnableSplitButton()
        {
            btnSplit.Enabled = !string.IsNullOrWhiteSpace(lblDir1.Text) && 
                               !string.IsNullOrWhiteSpace(lblDir2.Text) &&
                               !string.IsNullOrWhiteSpace(lblDestDir.Text);
        }
    }
}
