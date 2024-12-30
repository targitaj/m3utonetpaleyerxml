using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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
using AngleSharp;
using CefSharp;
using CefSharp.Wpf;
using Microsoft.Win32;
using mshtml;

namespace BreakingBet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ChromiumWebBrowser eoMain;
        private Thread _bbListener;
        private bool _started;
        private List<string> _fount = new List<string>();
        private Bet365Searcher _bet365Searcher;
        private MainWindowModel _model;

        public MainWindow()
        {
            InitializeComponent();
            _model = (MainWindowModel)DataContext;
        }

        private void BtnStart_OnClick(object sender, RoutedEventArgs e)
        {
            var cfSettings = new CefSettings();
            cfSettings.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:89.0) Gecko/20100101 Firefox/89.0";
            Cef.Initialize(cfSettings);
            eoMain = new ChromiumWebBrowser();
            gMain.Children.Add(eoMain);
            Grid.SetRow(eoMain, 1);
            eoMain.Loaded += EoMainOnLoaded;
            eoMain.Load("https://breaking-bet.com/ru/live");
        }

        private void EoMainOnLoaded(object sender, RoutedEventArgs e)
        {
            _bbListener = new Thread(BBListener);
            _started = true;
            _bbListener.Start();
        }

        private void BBListener()
        {
            while (_started)
            {
                try
                {
                    SearchData().Wait();
                }
                catch
                {
                    
                }
                
                Thread.Sleep(2000);
            }
        }

        private async Task SearchData()
        {
            var config = Configuration.Default;
            var context = BrowsingContext.New(config);
            var source = await eoMain.GetSourceAsync();
            var document = await context.OpenAsync(req => req.Content(source));

            var bookmakers = document.GetElementsByClassName("bookmaker_td");

            foreach (var bookmaker in bookmakers)
            {
                if (bookmaker.InnerHtml == "Bet365")
                {
                    var hand = bookmaker.ParentElement.GetElementsByClassName("teams").First();
                    var firstPerson = hand.Children[1].FirstElementChild.FirstElementChild.InnerHtml;
                    var secondPerson = hand.Children[1].FirstElementChild.InnerHtml
                        .Split(new string[] { "</span>" }, StringSplitOptions.RemoveEmptyEntries).Last();

                    var handData = firstPerson + " - " + secondPerson;

                    if (!_fount.Contains(handData))
                    {
                        var span = bookmaker.ParentElement.ParentElement.GetElementsByClassName("percent").First().FirstElementChild;
                        var percentage = span.InnerHtml.Replace("%", "");
                        var numPercentage = double.Parse(percentage);

                        if (numPercentage >= double.Parse(_model.PercentageOfProfitabilityFrom) &&
                            numPercentage <= double.Parse(_model.PercentageOfProfitabilityTo))
                        {
                            _fount.Add(handData);
                            _model.LogText += numPercentage + " " + handData + Environment.NewLine;

                            Dispatcher.Invoke(() =>
                            {
                                if (_bet365Searcher == null)
                                {
                                    _bet365Searcher = new Bet365Searcher();
                                    _bet365Searcher.Show();
                                }
                            });

                            while (_bet365Searcher != null && !_bet365Searcher.Fount.HasValue)
                            {
                                await _bet365Searcher.Search(handData);
                                Thread.Sleep(1000);
                            }

                            Dispatcher.Invoke(()=>
                            {
                                MessageBox.Show("fount");
                            });
                            _started = false;
                            break;
                        }
                    }
                }
            }
        }

        private void BtnStop_OnClick(object sender, RoutedEventArgs e)
        {
            _started = false;
        }
    }
}
