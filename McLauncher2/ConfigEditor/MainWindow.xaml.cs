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
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace ConfigEditor
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        //データテーブル
        private DataTable dataTable;
        public Dictionary<string, Config> LoadedConfig = new Dictionary<string,Config>();
        private Config selectedConfig;
        private bool editing = false;
        private string configDir;
                
        public MainWindow(string targetDir)
        {
            InitializeComponent();
            InitTable();
            button_overwrite.IsEnabled = false;
            this.configDir = targetDir + @"\config";
        }

        //データグリッド初期化
        private void InitTable()
        {
            dataGrid_edit.AutoGenerateColumns = false;
            dataGrid_edit.CanUserSortColumns = false;
            //スクロールバー設定
            dataGrid_edit.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            dataGrid_edit.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            //列追加
            dataGrid_edit.Columns.Clear();
            dataGrid_edit.Columns.Add(GenerateColumn("GROUP", "data_category"));
            dataGrid_edit.Columns.Add(GenerateColumn("NAME", "data_name"));
            dataGrid_edit.Columns.Add(GenerateColumn("TYPE", "data_type"));
            dataGrid_edit.Columns.Add(GenerateColumn("VALUE", "data_value"));
            //データテーブル初期化
            dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("data_category", typeof(string)));
            dataTable.Columns.Add(new DataColumn("data_name", typeof(string)));
            dataTable.Columns.Add(new DataColumn("data_type", typeof(string)));
            dataTable.Columns.Add(new DataColumn("data_value", typeof(string)));
            //バインド
            dataGrid_edit.DataContext = dataTable;
        }

        private DataGridTextColumn GenerateColumn(string header, string key)
        {
            return new DataGridTextColumn()
            {
                Header = header,
                IsReadOnly = true,
                Binding = new Binding(key)
            };
        }

        new public void Show()
        {
            if (!Directory.Exists(this.configDir))
            {
                MessageBox.Show("configフォルダがありません。コンフィグエディターを終了します");
            }
            else
            {
                LoadConfigs();
                base.Show();
            }
        }

        public void LoadConfigs()
        {
            var list = new List<Config>();
            var flag = Directory.Exists(configDir);

            foreach (var file in Directory.GetFiles(configDir))
            {
                var name = System.IO.Path.GetFileName(file);
                if (Regex.IsMatch(name, @"^(mod_).*(\.cfg)$"))
                {
                    list.Add(new MLPropConfig(file, name));
                }
            }
            listBox_configs.DataContext = list;
            LoadedConfig.Clear();
            foreach (Config config in list)
            {
                LoadedConfig.Add(config.Name, config);
            }
        }

        private void TableUpdate(Config config)
        {
            InitTable();
            foreach(var item in config.Items.Values)
            {
                DataRow newRowItem;
                newRowItem = dataTable.NewRow();
                newRowItem["data_category"] = item.Category;
                newRowItem["data_name"] = item.Name;
                newRowItem["data_type"] = item.Type;
                newRowItem["data_value"] = item.Value;
                dataTable.Rows.Add(newRowItem);
            }
        }

        private void Reload_Click(object sender, RoutedEventArgs e)
        {
            if(editing)
            {
                if(MessageBox.Show("編集中の内容を破棄しますがよろしいですか？","警告",MessageBoxButton.YesNo) 
                    == MessageBoxResult.No)
                {
                    return;
                }
            }
            LoadConfigs();
            editing = false;
            button_overwrite.IsEnabled = false;
        }

        private void Overwrite_Click(object sender, RoutedEventArgs e)
        {
            editing = false;
            button_overwrite.IsEnabled = false;
            selectedConfig.WriteFile();
        }

        private void Configs_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            var config = LoadedConfig[listBox_configs.SelectedItem.ToString()];
            if(!config.Equals(selectedConfig) && editing)
            {
                return;
            }
            else
            {
                selectedConfig = config;
                editing = false;
                button_overwrite.IsEnabled = false;
                config.ReadFile();
                TableUpdate(config);
            }
            
        }

        private void Edit_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            var row = dataGrid_edit.Items.IndexOf(dataGrid_edit.CurrentItem);
            if(row > dataTable.Rows.Count)
            {
                return;
            }
            Item item = selectedConfig.Items[dataTable.Rows[row]["data_name"].ToString()];
            var oldValue = item.Value;
            Window edit = new EditWindow(item);
            edit.ShowDialog();
            var newValue = item.Value;
            if (oldValue.Trim() != newValue.Trim())
            {
                editing = true;
                button_overwrite.IsEnabled = true;
                TableUpdate(selectedConfig);
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
    }
}
