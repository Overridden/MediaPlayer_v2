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
using System.Windows.Threading;
using System.Windows.Controls.Primitives;

namespace WpfApplication1
{
    public class ViewModel
    {
        public bool    playing { set; get; }
        public bool    loaded { set; get; }
        public bool    stoped { set; get; }
        private int     node_selected = 0;
        private enum    library_item {MUSIC=1, IMAGE, VIDEO, NONE };
        Library_manager lm = new Library_manager();

        public ViewModel()
        {
            playing = false;
            loaded = false;
            stoped = false;
            lm.playing = false;
            lm.loaded = false;
            lm.stoped = false;
            lm.link_vm_lm(this);
        }

        public void me_doStop(MediaElement MediaElement1, Image Image, System.Windows.Controls.Button Button1, System.Windows.Controls.Button Button2, System.Windows.Controls.Button Button4, Slider Slider1, Slider Slider2)
        {
            MediaElement1.Stop();
            MediaElement1.Opacity = 0;
            Image.Opacity = 1;
            this.playing = false;
            lm.playing = false;
            this.stoped = true;
            lm.stoped = true;
        }

        public void me_doPlay(MediaElement MediaElement1, Image Image, System.Windows.Controls.Button Button1, System.Windows.Controls.Button Button2, System.Windows.Controls.Button Button4, Slider Slider1, Slider Slider2)
        {
            if (play(MediaElement1, Image, Button1, Button2, Button4, Slider1, Slider2) == false)
            {
                MediaElement1.Pause();
                this.playing = false;
                lm.playing = false;
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

        public bool me_doOpen(MediaElement MediaElement1, Image Image, System.Windows.Controls.Button Button1, System.Windows.Controls.Button Button2, System.Windows.Controls.Button Button4, Slider Slider1, Slider Slider2)
        {
            OpenFileDialog ofd;
            ofd = new OpenFileDialog();
            ofd.AddExtension = true;
            ofd.DefaultExt = "*.*";
            ofd.Filter = "Media Files (*.*)|*.*";
            ofd.ShowDialog();
            if (ofd.FileName == "")
                return false;
            try
            {
                MediaElement1.Source = new Uri(ofd.FileName);
                loaded = true;
                lm.loaded = true;
                play(MediaElement1, Image, Button1, Button2, Button4, Slider1, Slider2);
            }
            catch
            {
                new NullReferenceException("Error while opening the file.");
            }
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (sender, e) => timer_Tick(MediaElement1, Slider1);
            timer.Start();
            return true;
        }

        private void timer_Tick(MediaElement MediaElement1, Slider Slider1)
        {
            if ((MediaElement1.Source != null) && (MediaElement1.NaturalDuration.HasTimeSpan))
            {
                Slider1.Minimum = 0;
                Slider1.Maximum = MediaElement1.NaturalDuration.TimeSpan.TotalSeconds;
                Slider1.Value = MediaElement1.Position.TotalSeconds;
            }
        }

        public bool check_level()
        {
            return lm.check_level();
        }

        public void Get_Item_Name(string str)
        {
            Console.Out.WriteLine(str);
            if (str.Contains("Music"))
                node_selected = (int)library_item.MUSIC;
            else if (str.Contains("Image"))
                node_selected = (int)library_item.IMAGE;
            else if (str.Contains("Video"))
                node_selected = (int)library_item.VIDEO;
            else
                node_selected = (int)library_item.NONE;
            return;
        }

        public bool Add_Item(string file, System.Windows.Controls.TreeView library, MediaElement MediaElement1, Image Image, System.Windows.Controls.Button Button1, System.Windows.Controls.Button Button2, System.Windows.Controls.Button Button4, Slider Slider1, Slider Slider2)
        {
            if (node_selected != (int)library_item.NONE)
                if (lm.Add_Item(node_selected, file, MediaElement1, Image, Button1, Button2, Button4, Slider1, Slider2) == false)
                    return false;
            return true;
        }

        public void Init_library(System.Windows.Controls.TreeView library, string file, MediaElement MediaElement1, Image Image, System.Windows.Controls.Button Button1, System.Windows.Controls.Button Button2, System.Windows.Controls.Button Button4, Slider Slider1, Slider Slider2)
        {
            lm.init_library(library, true);
            lm.fill_library(file, MediaElement1, Image, Button1, Button2, Button4, Slider1, Slider2);
        }

        public void clean_library(string file, System.Windows.Controls.TreeView library)
        {
            lm.clean_library(file, library);
        }

        public bool load_playlist(MediaElement MediaElement1, Image Image, System.Windows.Controls.Button Button1, System.Windows.Controls.Button Button2, System.Windows.Controls.Button Button4, Slider Slider1, Slider Slider2)
        {
            string new_playlist;
            OpenFileDialog ofd;
            ofd = new OpenFileDialog();
            ofd.AddExtension = true;
            ofd.DefaultExt = "*.*";
            ofd.Filter = "XML Files (*.xml)|*.xml";
            ofd.ShowDialog();
            if (ofd.FileName == "")
                return false;
            new_playlist = ofd.FileName;
            Console.WriteLine(new_playlist);
            lm.fill_library(new_playlist, MediaElement1, Image, Button1, Button2, Button4, Slider1, Slider2);
            return true;
        }

        public void Save_CurrentPlaylist(string file, int choice)
        {
            lm.Save_CurrentPlaylist(file, choice);
        }

        private bool play(MediaElement MediaElement1, Image Image, System.Windows.Controls.Button Button1, System.Windows.Controls.Button Button2, System.Windows.Controls.Button Button4, Slider Slider1, Slider Slider2)
        {
            if (this.playing == false && this.loaded == true)
            {
                MediaElement1.Play();
                this.playing = true;
                lm.playing = true;
                Image.Opacity = 0;
                if (this.stoped == true && lm.stoped == true)
                    MediaElement1.Opacity = 1;
            }
            else
                return false;
            return true;
        }
    }
}
