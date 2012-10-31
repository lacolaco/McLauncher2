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

namespace ConfigEditor
{
    /// <summary>
    /// EditWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class EditWindow : Window
    {
        private Item item;

        public EditWindow(Item item)
        {
            InitializeComponent();
            this.item = item;
            InitLabels();
        }

        private void InitLabels()
        {
            label_name.Content = item.Name;
            label_type.Content = item.Type;
            label_defalut.Content = item.DefaultValue;
            label_min.Content = item.MinValue;
            label_max.Content = item.MaxValue;
            textBlock_info.Text = item.Info;
            textBox_edit.Text = item.Value;
        }

        private void button_ok_Click(object sender, RoutedEventArgs e)
        {
            var value = textBox_edit.Text.Trim();
            if(TypeCheck(value))
            {
                item.Value = value;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("不正な値です");
            }
        }

        private bool TypeCheck(string value)
        {
            if(item.Type == "string")
            {
                return true;
            }
            if(item.Type == "boolean")
            {
                bool b;
                return bool.TryParse(value, out b);
            }
            else
            {
                float f;
                return float.TryParse(value, out f);
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
            this.MouseLeftButtonDown += delegate { DragMove(); };
        }
    }
}
