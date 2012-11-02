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
using System.IO;

namespace McLauncher2
{
    /// <summary>
    /// CustomWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class CustomWindow : Window
    {
        private bool customEnabled;
        private string path;

        public CustomWindow(string path)
        {
            InitializeComponent();
            customEnabled = Properties.Settings.Default.UseCustom;
            this.path = path;
            UpdateButton();
            ReadFile();
        }

        private void ReadFile()
        {
            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    this.TextBox_Bat.Text = reader.ReadToEnd();
                }
            }
            else
            {
                if (!Directory.GetParent(path).Exists)
                {
                    Directory.CreateDirectory(Directory.GetParent(path).FullName);
                }
                File.CreateText(path);
                this.TextBox_Bat.Text = string.Copy(RunManager.BatTemplate);
            }
        }

        private void UpdateButton()
        {
            if (customEnabled)
            {
                this.Button_UseCustom.Content = "ON";
                this.Button_UseCustom.Background = new SolidColorBrush(Color.FromRgb(0x1b, 0xa1, 0xe2));
            }
            else
            {
                this.Button_UseCustom.Content = "OFF";
                this.Button_UseCustom.Background = Brushes.Red;
            }
        }

        private void Button_Ok_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.UseCustom = customEnabled;
            Properties.Settings.Default.Save();
            var memo = this.TextBox_Bat.Text;
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.Write(memo);
                writer.Flush();
            }
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

        private void Button_UseCustom_Click(object sender, RoutedEventArgs e)
        {
            customEnabled = !customEnabled;
            UpdateButton();
        }
    }
}
