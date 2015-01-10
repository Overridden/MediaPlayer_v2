using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace WpfApplication1
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    /// 
    ///  Ctrl K + Ctrl C / Ctrl K + Ctrl U  to comment/uncomment

    public partial class MainWindow : Window
    {
        private string file = "../../library.xml";
        ViewModel vm = new ViewModel();
        TreeView_manager tvm = new TreeView_manager();

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Left = 30;
            this.Top = 30;
            vm.Fill_library(file, Library);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            vm.me_doStop(MediaElement1, Image, Button1, Button2, Button4, Slider1, Slider2);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MediaElement1.Pause();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
        //    MediaElement1.Stop();
        //    Button1.Content = "Play";
        //    this.playing = false;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            vm.me_manageVolume(MediaElement1, Image, Button1, Button2, Button4, Slider1, Slider2);
        }

        private void Slider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            vm.me_manageSliderLevel(e, MediaElement1, Image, Button1, Button2, Button4, Slider1, Slider2);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            vm.me_doPlay(MediaElement1, Image, Button1, Button2, Button4, Slider1, Slider2);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            vm.me_doOpen(MediaElement1, Image, Button1, Button2, Button4, Slider1, Slider2);
        }

        private void MediaElement1_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (MediaElement1.NaturalDuration.HasTimeSpan)
            {
                TimeSpan ts = TimeSpan.FromMilliseconds(MediaElement1.NaturalDuration.TimeSpan.TotalMilliseconds);
                Slider1.Maximum = ts.TotalSeconds;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            vm.Get_Item_Name(Library.SelectedItem.ToString());
            vm.Add_Item(file);
        }

        private void create_playlist_Click(object sender, RoutedEventArgs e)
        {

        }

        private void add_to_playlist_Click(object sender, RoutedEventArgs e)
        {

        }

        private void delete_from_playlist_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
