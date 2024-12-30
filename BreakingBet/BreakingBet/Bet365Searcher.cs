using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using mshtml;

namespace BreakingBet
{
    public class Bet365Searcher : BookmakersOfficeWindow
    {
        public bool? Fount { get; private set; }

        public async Task Search(string searchData)
        {
            Dispatcher.Invoke(() =>
            {
                wbMain.Navigate("https://www.bet365.com/#/AX/");
            });

            bool searchBoxFount = false;

            while (!searchBoxFount)
            {
                var doc = await wbMain.GetDocument();
                var divs = doc.getElementsByTagName("input");

                foreach (IHTMLElement div in divs)
                {
                    if (div.getAttribute("className") is string element && element.Contains("sml-SearchTextInput"))
                    {
                        IHTMLInputElement input = (IHTMLInputElement)div;
                        input.value = searchData;
                        searchBoxFount = true;
                        break;
                    }
                }

                if (!searchBoxFount)
                {
                    Thread.Sleep(1000);
                }
            }

            Fount = true;
        }
    }
}
