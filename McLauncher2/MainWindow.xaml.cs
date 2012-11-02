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
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        public ObservableCollection<Target> Targets = new ObservableCollection<Target>();

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
                    Target target = new Target(minecraft[0], System.IO.Path.GetFileName(dir));
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
            else if(this.TargetFolderPath == "")
            {
                this.Label_TargetFolder.Content = "No Setting";
            }
        }

        private void ListBox_TargetList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Target target = this.ListBox_TargetList.SelectedItem as Target;
            this.TreeView_Target.DataContext = target;
            this.TreeView_Target.UpdateLayout();
        }


        private void TreeView_Target_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var node = this.TreeView_Target.SelectedItem as DirectoryTreeNode;
            if (node != null)
            {
                Process.Start(node.Path);
            }
        }

        private void Button_Run_Click(object sender, RoutedEventArgs e)
        {
            Target target = this.ListBox_TargetList.SelectedItem as Target;
            if (target != null)
            {
                var cd = Environment.CurrentDirectory;
                RunManager rm = new RunManager(cd + @"\emb\Minecraft.exe", target);
                rm.RunMinecraft();
            }
            else
            {
                MessageBox.Show("ターゲットを選択してください");
            }
        }

        private void Button_Open_Click(object sender, RoutedEventArgs e)
        {
            Target target = this.ListBox_TargetList.SelectedItem as Target;
            if (target != null)
            {
                Process.Start(target.Path);
            }
            else
            {
                MessageBox.Show("ターゲットを選択してください");
            }
        }

        private void Button_ConfigEditor_Click(object sender, RoutedEventArgs e)
        {
            Target target = this.ListBox_TargetList.SelectedItem as Target;
            if (target != null)
            {
                ConfigEditor.MainWindow window = new ConfigEditor.MainWindow(target.Path);
                window.Show();
            }
            else
            {
                MessageBox.Show("ターゲットを選択してください");
            }
        }

        private void Button_Setting_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow window = new SettingWindow();
            window.ShowDialog();
        }

        private void Button_EditBat_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Memo_Click(object sender, RoutedEventArgs e)
        {
            Target target = this.ListBox_TargetList.SelectedItem as Target;
            var cd = Environment.CurrentDirectory;
            if (target != null)
            {
                MemoWindow window = new MemoWindow(cd + @"\emb\" + target.Name + @".memo", target);
                window.Show();
            }
            else
            {
                MessageBox.Show("ターゲットを選択してください");
            }
        }

        private void Button_Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void Button_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.MouseLeftButtonDown += delegate { DragMove(); };
        }

        private void Button_Help_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
