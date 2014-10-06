using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using HtmlAgilityPack;
using mshtml;

namespace BSerach
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            browser.Navigate("http://www.boomtime.lv");
            browser.LoadCompleted += browser_FirstLoadCompleted;
            SuppressScriptErrors(browser, true);
        }



        private void SuppressScriptErrors(System.Windows.Controls.WebBrowser wb, bool Hide)
        {

            FieldInfo fi = typeof (WebBrowser).GetField("_axIWebBrowser2",
                BindingFlags.Instance | BindingFlags.NonPublic);
            if (fi != null)
            {
                object browser = fi.GetValue(wb);
                if (browser != null)
                {
                    browser.GetType()
                        .InvokeMember("Silent", BindingFlags.SetProperty, null, browser, new object[] {Hide});
                }
            }
        }

        private bool go = false;

        private void browser_FirstLoadCompleted(object sender, NavigationEventArgs e)
        {
            browser.LoadCompleted -= browser_FirstLoadCompleted;

            HTMLDocument dom = (HTMLDocument)browser.Document;
            object ie = dom.getElementById("prf_login");
            if (ie != null)
            {
                if (ie is IHTMLInputElement)
                {
                    ((IHTMLInputElement)ie).value = "targitaj";
                }
            }

            ie = dom.getElementById("prf_pass");
            if (ie != null)
            {
                if (ie is IHTMLInputElement)
                {
                    ((IHTMLInputElement)ie).value = "privetboomtime";
                }
            }


            dom.all.item("auth_button").click();

            ThreadPool.QueueUserWorkItem((delegate
            {
                Thread.Sleep(1000);
                Dispatcher.BeginInvoke(new Action(delegate()
                {
                    //browser.LoadCompleted += browser_LoadCompleted;
                    browser.Navigate("http://friends.boomtime.lv/dating.html");
                }));
            }));

            
        }

        private string res = "";

        void browser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            HTMLDocument dd = (HTMLDocument)browser.Document;
            var tt = dd.body.innerHTML;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(tt);

            var dn = doc.DocumentNode;

            var nodes = dn.SelectNodes("//td[@class='tbl']");
;
            if (nodes != null)
            {
                foreach (HtmlNode htmlNode in nodes)
                {
                    if (IsOnline(htmlNode.ChildNodes))
                    {
                        if (!File.Exists("html.html"))
                        {
                            File.AppendAllText("html.html",
                                @"<meta http-equiv=""content-type"" content=""text/html; charset=utf-8"">");
                        }
                        if (!htmlNode.InnerHtml.Contains("http://friends.boomtime.lv"))
                        {
                            File.AppendAllText("html.html",
                                htmlNode.InnerHtml.Replace(@"<a href=""", @"<a href=""http://friends.boomtime.lv"));
                        }

                        File.AppendAllText("html.html", htmlNode.InnerHtml);

                        browser.InvokeScript("write_fast", GetId(htmlNode), "sid");
                        Thread.Sleep(500);

                        var msg = dd.getElementById("msg_editor_w");
                        msg.innerText = "Привет :)";

                        var links = dd.getElementsByTagName("table");
                        this.InvalidateVisual();
                        Thread.Sleep(1000);

                        foreach (var link in links)
                        {
                            var table = (IHTMLElement)link;

                            if (table.className == "button size1")
                            {
                                table.click();
                            }
                        }
                        this.InvalidateVisual();
                        Thread.Sleep(5000);
                    }
                }

                counter++;

                if (nodes.Count > 0)
                {
                    browser.Navigate(string.Format(tbUri.Text, counter));
                }

                txtNumber.Text = counter.ToString();
            }
        }

        

        private string GetId(HtmlNode node)
        {
            var nd = node.SelectNodes(".//a").First();
            var val = nd.GetAttributeValue("href", "");

            return val.Substring(val.LastIndexOf('/') + 1, val.Length - val.LastIndexOf('/') - 6);
        }

        bool IsOnline(HtmlNodeCollection nodeCollection)
        {
            var res = false;

            foreach (HtmlNode childNode in nodeCollection)
            {
                string def = "";

                if (childNode.GetAttributeValue("src", def) != "")
                {
                    if (childNode.GetAttributeValue("src", def) == "/skin/all/on-line.gif")
                    {
                        return true;
                    }
                }
                else
                {
                    if (childNode.HasChildNodes)
                    {
                        res = IsOnline(childNode.ChildNodes);
                    }

                    if (res)
                    {
                        break;
                    }
                }
            }

            return res;
        }

        public void HideScriptErrors(WebBrowser wb, bool Hide)
        {
            FieldInfo fiComWebBrowser = typeof(WebBrowser)
                .GetField("_axIWebBrowser2",
                          BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;
            object objComWebBrowser = fiComWebBrowser.GetValue(wb);
            if (objComWebBrowser == null) return;
            objComWebBrowser.GetType().InvokeMember(
                "Silent", BindingFlags.SetProperty, null, objComWebBrowser,
                new object[] { Hide });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            browser.InvokeScript("write_fast", "clcxaaupfbn", "sid");
        }

        private bool started = true;

        private List<string> GetAllPersons(string html)
        {
            int idx = 0;
            List<string> res = new List<string>();

            Regex regex = new Regex(@"<td class=tbl[^>]*>((?:(?:(?!<div[^>]*>|</div>).)+|<div[^>]*>([\s\S]*?)</div>)*)</div>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            //Regex r = new Regex(@"[<td class=tbl></td>]+?(?=<|$)");

            var zzz = regex.Match(html);

            

            return res;
        }

        private string GetPerson(string html, ref int index)
        {
            int start = html.IndexOf(@"<td class=tbl", index);
            string res = null;


            if (start >= 0)
            {
                var end = GetLastIndexOfPerson(html, start);
                res = html.Substring(start, end - start);
            }

            index = start+1;

            return res;
        }


        private int GetLastIndexOfPerson(string html, int index)
        {
            int cnt = 1;

            do
            {
                var startr = html.IndexOf(@"<td ", index+1);
                var end = html.IndexOf(@"</td>", index+1);

                if (startr < end)
                {
                    index = startr;
                    cnt++;
                }
                else
                {
                    index = end;
                    cnt--;
                }

            } while (cnt > 0);

            return index;
        }

        void Go()
        {
            while (started)
            {
                HTMLDocument dd = (HTMLDocument)browser.Document;
            
                var tt = dd.body.innerHTML;

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(tt);

                var nodes = doc.DocumentNode.SelectNodes("//td[@class=tbl]");

                Thread.Sleep(1000);

                return;
            }
        }

        private int counter = 0;

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            counter = int.Parse(txtNumber.Text);

            browser.LoadCompleted += browser_LoadCompleted;

            browser.Navigate(string.Format(tbUri.Text, counter));
        }
    }
}
