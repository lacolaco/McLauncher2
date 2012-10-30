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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace McLauncher2
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public string TargetFolderPath;
        public List<Target> Targets = new List<Target>();

        public MainWindow()
        {
            InitializeComponent();
            this.TargetFolderPath = Properties.Settings.Default.TargetFolderPath;
            if (!string.IsNullOrEmpty(this.TargetFolderPath))
            {
                this.Label_TargetFolder.Content = this.TargetFolderPath;
                InitTargetList();
            }
            this.ListBox_TargetList.DataContext = Targets;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.TargetFolderPath = this.TargetFolderPath;
            Properties.Settings.Default.Save();
        }

        private void InitTargetList()
        {
            this.Targets.Clear();
            var dirs = Directory.GetDirectories(this.TargetFolderPath);
            foreach (var dir in dirs)
            {
                var minecraft = Directory.GetDirectories(dir, ".minecraft");
                if (minecraft.Length > 0)
                {
                    Target target = new Target(dir, System.IO.Path.GetFileName(dir));
                    this.Targets.Add(target);
                }
            }
        }

        private void Button_TargetFolder_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            dialog.AllowNonFileSystemItems = false;
            dialog.DefaultDirectory = @"C:\";
            dialog.Title = "ターゲットフォルダを選択してください";
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                this.TargetFolderPath = dialog.FileName;
                this.Label_TargetFolder.Content = this.TargetFolderPath;
                InitTargetList();
            }
        }

        private void ListBox_TargetList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ListBox_Target_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_Run_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_ConfigEditor_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Setting_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_EditBat_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Memo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.MouseLeftButtonDown += delegate { DragMove(); };
        }
    }
}
