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
using System.Windows.Controls.Primitives;
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
        private bool userIsDraggingSlider = false;

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Left = 30;
            this.Top = 30;
            vm.Init_library(Library, file, MediaElement1, Image, Button1, Button2, Button4, Slider1, Slider2);
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

        private void Slider1_DragStarted(object sender, DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }

        private void Slider1_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            MediaElement1.Position = TimeSpan.FromSeconds(Slider1.Value);
        }

        public bool getuserIsDraggingSlider()
        {
            return userIsDraggingSlider;
        }

        private void Slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Timer.Text = TimeSpan.FromSeconds(Slider1.Value).ToString(@"hh\:mm\:ss");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            vm.me_doPlay(MediaElement1, Image, Button1, Button2, Button4, Slider1, Slider2);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (vm.me_doOpen(MediaElement1, Image, Button1, Button2, Button4, Slider1, Slider2) == false)
                Console.Out.WriteLine("Aborting the file opening");
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
            if (Library.SelectedItem != null)
            {
                if (vm.check_level() == true)
                {
                    Err_msg.Content = "";
                    vm.Get_Item_Name(Library.SelectedItem.ToString());
                    vm.Add_Item(file, Library, MediaElement1, Image, Button1, Button2, Button4, Slider1, Slider2);
                }
                else
                    Err_msg.Content = "Please select Image, Video or Music.";
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            vm.Save_CurrentPlaylist(file);
        }

        private void open_playlist_Click(object sender, RoutedEventArgs e)
        {
            vm.clean_library(file, Library);
            if (vm.load_playlist(MediaElement1, Image, Button1, Button2, Button4, Slider1, Slider2) == false)
                Console.Out.WriteLine("Aborting the file opening");
        }

        private void DeleteButton_Click_1(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.TreeViewItem lolol = new System.Windows.Controls.TreeViewItem() { Header = "lolol" };
            System.Windows.Controls.TreeViewItem caca = new System.Windows.Controls.TreeViewItem() { Header = "caca" };
            Library.Items.Add(lolol);
            lolol.Items.Add(caca);
            //Library.Items.Refresh();
            //Library.UpdateLayout();
            Console.Out.WriteLine("UPDATE MOZER FEUKEUR!");
        }
    }
}
