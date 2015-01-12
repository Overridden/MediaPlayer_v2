using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Logique d'interaction pour save_box.xaml
    /// </summary>
    public partial class save_box : Window
    {
        public int choice { set; get; }
        public save_box()
        {
            InitializeComponent();
            choice = 1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            choice = 1;
            Application.Current.Shutdown();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            choice = 2;
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            choice = 3;
            this.Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            choice = 4;
            this.Close();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            choice = 5;
            this.Close();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            choice = 6;
            this.Close();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            choice = 7;
            this.Close();
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            choice = 8;
            this.Close();
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            choice = 9;
            this.Close();
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            Console.Out.WriteLine("YOOO");
            choice = 10;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //e.Cancel = true;
        }
    }
}
