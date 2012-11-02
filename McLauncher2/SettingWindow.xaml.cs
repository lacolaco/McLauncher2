using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace McLauncher2
{
    /// <summary>
    /// EditWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class SettingWindow : Window
    {
        private bool logEnabled;
        private bool noUpdate;

        public SettingWindow()
        {
            InitializeComponent();
            logEnabled = Properties.Settings.Default.LogEnabled;
            noUpdate = Properties.Settings.Default.NoUpdate;
            UpdateLogButton();
            UpdateNoUpdateButton();
        }
        private void UpdateLogButton()
        {
            if(logEnabled)
            {
                this.Button_Log.Content = "ON";
                this.Button_Log.Background = new SolidColorBrush(Color.FromRgb(0x1b,0xa1,0xe2));
            }
            else
            {
                this.Button_Log.Content = "OFF";
                this.Button_Log.Background = Brushes.Red;
            }
        }

        private void UpdateNoUpdateButton()
        {
            if (noUpdate)
            {
                this.Button_NoUpdate.Content = "ON";
                this.Button_NoUpdate.Background = new SolidColorBrush(Color.FromRgb(0x1b, 0xa1, 0xe2));
            }
            else
            {
                this.Button_NoUpdate.Content = "OFF";
                this.Button_NoUpdate.Background = Brushes.Red;
            }
        }

        private void Button_Ok_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.LogEnabled = logEnabled;
            Properties.Settings.Default.NoUpdate = noUpdate;
            Properties.Settings.Default.Save();
            DialogResult = true;
        }

        private void Button_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.MouseLeftButtonDown += delegate { DragMove(); };
        }

        private void Button_Log_Click(object sender, RoutedEventArgs e)
        {
            logEnabled = !logEnabled;
            UpdateLogButton();
        }

        private void Button_NoUpdate_Click(object sender, RoutedEventArgs e)
        {
            noUpdate = !noUpdate;
            UpdateNoUpdateButton();
        }
    }
}
