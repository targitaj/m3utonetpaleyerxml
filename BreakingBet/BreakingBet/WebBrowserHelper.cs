using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using mshtml;

namespace BreakingBet
{
    public static class WebBrowserHelper
    {
        public static async Task<HTMLDocument> GetDocument(this WebBrowser webBrowser)
        {
            HTMLDocument document = null;

            while (document == null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    document = (HTMLDocument)webBrowser.Document;
                });

                Thread.Sleep(100);
            }

            return document;
        }
    }
}
