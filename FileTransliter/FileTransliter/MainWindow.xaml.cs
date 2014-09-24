using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

namespace FileTransliter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<string, string> transList = new Dictionary<string, string>
        {
            {"а", "a"},
            {"б", "b"},
            {"в", "v"},
            {"г", "g"},
            {"д", "d"},
            {"е", "je"},
            {"ё", "jo"},
            {"ж", "z"},
            {"з", "z"},
            {"и", "i"},
            {"й", "j"},
            {"к", "k"},
            {"л", "l"},
            {"м", "m"},
            {"н", "n"},
            {"о", "o"},
            {"п", "p"},
            {"р", "r"},
            {"с", "c"},
            {"т", "t"},
            {"у", "u"},
            {"ф", "f"},
            {"х", "h"},
            {"ц", "c"},
            {"ч", "ch"},
            {"щ", "sch"},
            {"ъ", ""},
            {"ы", "i"},
            {"ь", "'"},
            {"э", "e"},
            {"ю", "ju"},
            {"я", "ja"}
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                tbDir.Text = dialog.SelectedPath;
            }
        }

        private void ButtonBase_OnClick2(object sender, RoutedEventArgs e)
        {
            var files = Directory.GetFiles(tbDir.Text);

            foreach (string file in files)
            {
                var fi = new FileInfo(file);

                File.Move(file, fi.DirectoryName + "\\" + ReplaceFileName(fi.Name.ToLower()));
            }
        }

        private string ReplaceFileName(string fileName)
        {
            var res = string.Empty;

            foreach (var ch in fileName)
            {
                if (transList.ContainsKey(ch.ToString()))
                {
                    res += transList[ch.ToString()];
                }
                else
                {
                    res += ch;
                }
            }

            return res;
        }
    }
}
