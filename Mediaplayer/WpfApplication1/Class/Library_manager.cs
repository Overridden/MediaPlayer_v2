using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace WpfApplication1
{
    class Library_manager
    {
        public bool playing { get; set; }
        public bool loaded { get; set; }
        public bool stoped { get; set; }
        private List<Item> tmp;
        private List<Item> empty;
        private List<Item> playlist_tmp;
        private OpenFileDialog ofd;
        private string[] folder_name = new string[] { "Music", "Image", "Video" };
        private System.Windows.Controls.TreeViewItem f_library = new System.Windows.Controls.TreeViewItem() { Header = "Library" };
        private System.Windows.Controls.TreeViewItem image = new System.Windows.Controls.TreeViewItem() { Header = "Image" };
        private System.Windows.Controls.TreeViewItem video = new System.Windows.Controls.TreeViewItem() { Header = "Video" };
        private System.Windows.Controls.TreeViewItem music = new System.Windows.Controls.TreeViewItem() { Header = "Music" };
        private int image_count = 0;
        private int video_count = 0;
        private int music_count = 0;
        Deserializer ds = new Deserializer();
        Serializer s = new Serializer();
        ViewModel vm;

        public void link_vm_lm(ViewModel vm)
        {
            this.vm = vm;
        }

        public bool Add_Item(int node_selected, string file, MediaElement MediaElement1, Image Image, System.Windows.Controls.Button Button1, System.Windows.Controls.Button Button2, System.Windows.Controls.Button Button4, Slider Slider1, Slider Slider2)
        {
            ofd = new OpenFileDialog();
            ofd.AddExtension = true;
            ofd.DefaultExt = "*.*";
            ofd.Filter = "Media Files (*.*)|*.*";
            ofd.ShowDialog();
            if (ofd.FileName == "")
                return false;
            Console.Out.WriteLine("File selected " + ofd.FileName);
            serialize_and_add(node_selected, ofd.FileName, file, MediaElement1, Image, Button1, Button2, Button4, Slider1, Slider2);
            return true;
        }

        private void serialize_and_add(int node_selected, string path, string file, MediaElement MediaElement1, Image Image, System.Windows.Controls.Button Button1, System.Windows.Controls.Button Button2, System.Windows.Controls.Button Button4, Slider Slider1, Slider Slider2)
        {
            // DESERIALIZE
            Console.Out.WriteLine("Checking file " + file);
            if (ds.check_file(file) == true)
            {
                Console.Out.WriteLine("Deserializing...");
                tmp = ds.deserialize(file);
                // FILL THE LIBRARY
                Console.Out.WriteLine("Adding item...");
                Item item1 = new Item
                {
                    folder = folder_name[node_selected - 1],
                    path = ofd.FileName,
                    file_name = ofd.SafeFileName
                };
                Console.Out.WriteLine("-->" + folder_name[node_selected - 1] + "<--");
                tmp.Add(item1);
                sort_and_add(item1, MediaElement1, Image, Button1, Button2, Button4, Slider1, Slider2);
                // SERIALIZE
                Console.Out.WriteLine("Serializing...");
                s.serialize(tmp, file);
                Console.WriteLine("[Folder-]\t[File-----------------------------------------------------------------]");
                foreach (Item item in tmp)
                {
                    Console.WriteLine("####" + item.folder + "\t####" + item.path);
                }
                Console.WriteLine();
            }
            else
            {
                Create_file(tmp, file);
                serialize_and_add(node_selected, path, file, MediaElement1, Image, Button1, Button2, Button4, Slider1, Slider2);
            }
        }

        public bool check_level()
        {
            if (image.IsSelected == true || music.IsSelected == true || video.IsSelected == true)
                return true;
            return false;
        }

        private void sort_and_add(Item item, MediaElement MediaElement1, Image Image, System.Windows.Controls.Button Button1, System.Windows.Controls.Button Button2, System.Windows.Controls.Button Button4, Slider Slider1, Slider Slider2)
        {
            System.Windows.Controls.TreeViewItem item_node = new System.Windows.Controls.TreeViewItem() { Header = "item" };
            if (item.folder == "Video")
            {
                item_node.Header = item.file_name;
                video.Items.Add(item_node);
                ++video_count;
            }
            else if (item.folder == "Music")
            {
                item_node.Header = item.file_name;
                music.Items.Add(item_node);
                ++music_count;
            }
            else if (item.folder == "Image")
            {
                item_node.Header = item.file_name;
                image.Items.Add(item_node);
                ++image_count;
            }
            item_node.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
            item_node.MouseDoubleClick += (sender, e) => double_click(MediaElement1, item.path, Image, Button1, Button2, Button4, Slider1, Slider2);
        }

        public void init_library(System.Windows.Controls.TreeView library, bool fill)
        {
            library.Items.Add(f_library);
            if (fill == true)
            {
                f_library.Items.Add(image);
                f_library.Items.Add(video);
                f_library.Items.Add(music);
                f_library.IsExpanded = true;
                image.IsExpanded = true;
                video.IsExpanded = true;
                music.IsExpanded = true;
                f_library.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
                image.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
                video.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
                music.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
            }
        }

        public void fill_library(string file, MediaElement MediaElement1, Image Image, System.Windows.Controls.Button Button1, System.Windows.Controls.Button Button2, System.Windows.Controls.Button Button4, Slider Slider1, Slider Slider2)
        {
            Item item;
            List<Item> tmp;
            Deserializer ds = new Deserializer();

            Console.Out.WriteLine(file);

            tmp = ds.deserialize(file);
            for (int i = 0; i < tmp.Count; i++)
            {
                item = tmp[i];
                sort_and_add(item, MediaElement1, Image, Button1, Button2, Button4, Slider1, Slider2);
            }
        }

        public void clean_library(string file, System.Windows.Controls.TreeView library)
        {
            f_library.Items.Remove(image);
            f_library.Items.Remove(video);
            f_library.Items.Remove(music);
            library.Items.Remove(f_library);
            image = new System.Windows.Controls.TreeViewItem() { Header = "Image" };
            video = new System.Windows.Controls.TreeViewItem() { Header = "Video" };
            music = new System.Windows.Controls.TreeViewItem() { Header = "Music" };
            init_library(library, true);
        }

        public void Create_file(List<Item> list, string location)
        {
            Console.Out.WriteLine("Cannot find the file.\nCreating " + location);
            s.serialize(list, location);
            Console.Out.WriteLine("Success!");
        }

        public void Create_file(string location)
        {
            Console.Out.WriteLine("Cannot find the file.\nCreating " + location);
            s.serialize(empty, location);
            Console.Out.WriteLine("Success!");
        }

        public void Save_CurrentPlaylist(string file, int choice)
        {
            int i = 1;
            string location = "../../Playlists/MyPlaylist-" + i + ".xml";
            Console.Out.WriteLine(" Myplaylist name : " + location);

            // DESERIALIZE
            Console.Out.WriteLine("Checking file " + location);
            if (ds.check_file(location) == true)
            {
                Console.Out.WriteLine("Deserializing...");
                playlist_tmp = ds.deserialize(file);
                // SERIALIZE
                Console.Out.WriteLine("Serializing...");
                ++i;
                location = "../../Playlists/MyPlaylist-" + i + ".xml";
                while (i < 10)
                {
                    if (s.check_file(location) == true)
                    {
                        Create_file(location);
                        location = "../../Playlists/MyPlaylist-" + i + ".xml";
                    }
                    ++i;
                }
                location = "../../Playlists/MyPlaylist-" + choice + ".xml";
                s.serialize(playlist_tmp, location);
                Console.Out.WriteLine("Serializing " + location);
            }
            else
            {
                Create_file(playlist_tmp, location);
                Save_CurrentPlaylist(location, choice);
            }
        }

        private void double_click(MediaElement MediaElement1, string path, Image Image, System.Windows.Controls.Button Button1, System.Windows.Controls.Button Button2, System.Windows.Controls.Button Button4, Slider Slider1, Slider Slider2)
        {
            Console.Out.WriteLine("Opening file " + path);
            try
            {
                MediaElement1.Source = new Uri(path);
                loaded = true;
                vm.loaded = true;
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

        private bool play(MediaElement MediaElement1, Image Image, System.Windows.Controls.Button Button1, System.Windows.Controls.Button Button2, System.Windows.Controls.Button Button4, Slider Slider1, Slider Slider2)
        {
            if (this.playing == false && this.loaded == true)
            {
                MediaElement1.Play();
                this.playing = true;
                vm.playing = true;
                Image.Opacity = 0;
                if (this.stoped == true && vm.stoped == true)
                    MediaElement1.Opacity = 1;
            }
            else
                return false;
            return true;
        }
    }
}
