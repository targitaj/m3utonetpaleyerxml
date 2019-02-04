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

namespace AceRemoteControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowModel();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UIElement_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            File.AppendAllText("keylog.txt", e.Key.ToString());
        }

        private void Control_OnPreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Information.StartVideo("0", null, ((Channel)((ListBox)sender).SelectedItems[0]).Text);
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            foreach (var lb1Item in lb1.Items)
            {
                ((Channel) lb1Item).IsSelected = false;
            }
        }

        private void ToggleButton_OnChecked1(object sender, RoutedEventArgs e)
        {
            foreach (var lb1Item in lb2.Items)
            {
                ((Channel)lb1Item).IsSelected = false;
            }
        }
    }
}
