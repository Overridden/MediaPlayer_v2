using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
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
    class ViewModel
    {
        private bool    playing = false;
        private bool    loaded = false;
        private bool    stoped = false;
        private int     node_selected = 0;
        private enum    library_item {MUSIC=1, IMAGE, VIDEO, NONE };
        Library_manager lm = new Library_manager();

        public void me_doStop(MediaElement MediaElement1, Image Image, System.Windows.Controls.Button Button1, System.Windows.Controls.Button Button2, System.Windows.Controls.Button Button4, Slider Slider1, Slider Slider2)
        {
            MediaElement1.Stop();
            MediaElement1.Opacity = 0;
            Image.Opacity = 1;
            Button1.Content = "Play";
            this.playing = false;
            this.stoped = true;
        }

        public void me_doPlay(MediaElement MediaElement1, Image Image, System.Windows.Controls.Button Button1, System.Windows.Controls.Button Button2, System.Windows.Controls.Button Button4, Slider Slider1, Slider Slider2)
        {
            if (play(MediaElement1, Image, Button1, Button2, Button4, Slider1, Slider2) == false)
            {
                Button1.Content = "Play";
                MediaElement1.Pause();
                this.playing = false;
            }
        }

        public void me_manageVolume(MediaElement MediaElement1, Image Image, System.Windows.Controls.Button Button1, System.Windows.Controls.Button Button2, System.Windows.Controls.Button Button4, Slider Slider1, Slider Slider2)
        {
            MediaElement1.Volume = Slider2.Value;
        }

        public void me_manageSliderLevel(RoutedPropertyChangedEventArgs<double> e, MediaElement MediaElement1, Image Image, System.Windows.Controls.Button Button1, System.Windows.Controls.Button Button2, System.Windows.Controls.Button Button4, Slider Slider1, Slider Slider2)
        {
            TimeSpan ts = TimeSpan.FromSeconds(e.NewValue);
            MediaElement1.Position = ts;
        }

        public void me_doOpen(MediaElement MediaElement1, Image Image, System.Windows.Controls.Button Button1, System.Windows.Controls.Button Button2, System.Windows.Controls.Button Button4, Slider Slider1, Slider Slider2)
        {
            OpenFileDialog ofd;
            ofd = new OpenFileDialog();
            ofd.AddExtension = true;
            ofd.DefaultExt = "*.*";
            ofd.Filter = "Media Files (*.*)|*.*";
            ofd.ShowDialog();
            try
            {
                MediaElement1.Source = new Uri(ofd.FileName);
                loaded = true;
                play(MediaElement1, Image, Button1, Button2, Button4, Slider1, Slider2);
            }
            catch
            {
                new NullReferenceException("Error while opening the file.");
            }
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            //dispatcherTimer.Tick += new EventHandler(timer_Tick);
            dispatcherTimer.Tick += (sender, e) => timer_Tick(MediaElement1, Image, Button1, Button2, Button4, Slider1, Slider2);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        public void Get_Item_Name(string str)
        {
            if (str.Contains("Music"))
                node_selected = (int)library_item.MUSIC;
            else if (str.Contains("Image"))
                node_selected = (int)library_item.IMAGE;
            else if (str.Contains("Video"))
            {
                node_selected = (int)library_item.VIDEO;
                Console.Out.WriteLine("PD");
        }
            else
                node_selected = (int)library_item.NONE;
            return;
        }

        public void Add_Item(string file)
        {
            if (node_selected != (int)library_item.NONE)
                lm.Add_Item(node_selected, file);
        }

        public void Init_library(System.Windows.Controls.TreeView library, string file)
        {
            lm.init_library(library);
            lm.fill_library(file);
        }

        private void timer_Tick(MediaElement MediaElement1, Image Image, System.Windows.Controls.Button Button1, System.Windows.Controls.Button Button2, System.Windows.Controls.Button Button4, Slider Slider1, Slider Slider2)
        {
            Slider1.Value = MediaElement1.Position.TotalSeconds;
        }

        private bool play(MediaElement MediaElement1, Image Image, System.Windows.Controls.Button Button1, System.Windows.Controls.Button Button2, System.Windows.Controls.Button Button4, Slider Slider1, Slider Slider2)
        {
            if (this.playing == false && this.loaded == true)
            {
                Button1.Content = "Pause";
                MediaElement1.Play();
                this.playing = true;
                Image.Opacity = 0;
                if (this.stoped == true)
                    MediaElement1.Opacity = 1;
            }
            else
                return false;
            return true;
        }
    }
}
