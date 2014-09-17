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
        private string mainUrl =
            "http://www.babystore.lv/?dir=cat&id=258&s=shtanishki-shorty-dzhinsy&instock=false&price_from=0.00&price_to=42.16&page=all&lang=ru";

        public MainWindow()
        {
            InitializeComponent();
            browser.LoadCompleted += browser_LoadCompleted;
            browser.Navigate(mainUrl);

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

        private string res = "";
        private List<string> seenUrls = new List<string>();
        private string lastHtml;


        private bool isInProduct = false;

        void browser_Inproduct(object sender, NavigationEventArgs e)
        {
            if (!isInProduct)
                return;

            HTMLDocument dd = (HTMLDocument)browser.Document;
            var tt = dd.body.innerHTML;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(tt);
            var fnode = doc.DocumentNode.SelectNodes("//form[@name='in_cart']").FirstOrDefault();

            if (tt.Contains("92 cm"))
            {
                File.AppendAllText("test.html", lastHtml.Replace(@"href=""", @"href=""http://www.babystore.lv/").Replace("url(", "url(http://www.babystore.lv/"));
            }

            browser.LoadCompleted -= browser_Inproduct;
            browser.LoadCompleted += browser_LoadCompleted;

            browser.Navigate(mainUrl);
        }

        void browser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            browser.LoadCompleted -= browser_LoadCompleted;
            browser.LoadCompleted += browser_Inproduct;

            HTMLDocument dd = (HTMLDocument)browser.Document;
            var tt = dd.body.innerHTML;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(tt);

            var dn = doc.DocumentNode;

            var nodes = dn.SelectNodes("//div[@class='productblock']");

            if (nodes != null)
            {
                foreach (HtmlNode htmlNode in nodes)
                {
                    var urlNode = htmlNode.SelectSingleNode(".//a");
                    var cc = htmlNode.ChildNodes;

                    var url = urlNode.GetAttributeValue("href", "");

                    if (!seenUrls.Contains(url))
                    {
                        seenUrls.Add(url);
                        isInProduct = true;
                        lastHtml = htmlNode.OuterHtml;
                        browser.Navigate("http://www.babystore.lv/" + url.Replace("amp;", ""));
                        return;
                    }
                }
            }
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
            go = true;

            HTMLDocument dd = (HTMLDocument)browser.Document;

            var tt = dd.body.innerHTML;

            //File.WriteAllText("aaa.txt", tt);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(tt);

            var dn = doc.DocumentNode;
            var nodes = dn.SelectNodes("//div[contains(@class, 'tbl')]");


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
