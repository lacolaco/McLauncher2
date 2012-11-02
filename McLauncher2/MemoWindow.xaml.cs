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
    /// MemoWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MemoWindow : Window
    {
        private Target target;
        private string path;

        public MemoWindow(string path, Target target)
        {
            InitializeComponent();
            this.path = path;
            this.target = target;
            GetTextFromFile();
        }

        public void GetTextFromFile()
        {
            if(File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    this.TextBox_Memo.Text = reader.ReadToEnd();
                }                
            }
            else
            {
                if(!Directory.GetParent(path).Exists)
                {
                    Directory.CreateDirectory(Directory.GetParent(path).FullName);
                }
                File.CreateText(path);
            }
        }

        private void Button_Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void Button_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.MouseLeftButtonDown += delegate { try { DragMove(); } catch { } };
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var memo = this.TextBox_Memo.Text;
            using(StreamWriter writer = new StreamWriter(path, false))
            {
                writer.Write(memo);
                writer.Flush();
            }
        }
    }
}
