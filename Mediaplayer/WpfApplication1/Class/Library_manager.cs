using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace WpfApplication1
{
    class Library_manager
    {
        private List<Item> tmp;
        private List<Item> empty;
        private List<Item> playlist_tmp;
        private OpenFileDialog ofd;
        private string[] folder_name = new string[] { "Music", "Image", "Video" };
        private System.Windows.Controls.TreeViewItem f_library = new System.Windows.Controls.TreeViewItem() { Header = "Library" };
        private System.Windows.Controls.TreeViewItem image = new System.Windows.Controls.TreeViewItem() { Header = "Image" };
        private System.Windows.Controls.TreeViewItem video = new System.Windows.Controls.TreeViewItem() { Header = "Video" };
        private System.Windows.Controls.TreeViewItem music = new System.Windows.Controls.TreeViewItem() { Header = "Music" };
        Deserializer ds = new Deserializer();
        Serializer s = new Serializer();

        public void Add_Item(int node_selected, string file)
        {
            ofd = new OpenFileDialog();
            ofd.AddExtension = true;
            ofd.DefaultExt = "*.*";
            ofd.Filter = "Media Files (*.*)|*.*";
            ofd.ShowDialog();
            Console.Out.WriteLine("File selected " + ofd.FileName);
            serialize_and_add(node_selected, ofd.FileName, file);
        }

        private void serialize_and_add(int node_selected, string path, string file)
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
                    path = ofd.FileName
                };
                Console.Out.WriteLine("-->" + folder_name[node_selected - 1] + "<--");
                tmp.Add(item1);
                sort_and_add(item1);
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
                serialize_and_add(node_selected, path, file);
            }
        }

        private void sort_and_add(Item item)
        {
            if (item.folder == "Video")
            {
                video.Items.Add(item.path);
            }
            else if (item.folder == "Music")
            {
                music.Items.Add(item.path);
            }
            else if (item.folder == "Image")
            {
                image.Items.Add(item.path);
            }
        }

        public void init_library(System.Windows.Controls.TreeView library)
        {
            library.Items.Add(f_library);
            f_library.Items.Add(image);
            f_library.Items.Add(video);
            f_library.Items.Add(music);
            f_library.IsExpanded = true;
            image.IsExpanded = true;
            video.IsExpanded = true;
            music.IsExpanded = true;
        }

        public void fill_library(string file)
        {
            Item item;
            List<Item> tmp;
            Deserializer ds = new Deserializer();

            Console.Out.WriteLine(file);

            tmp = ds.deserialize(file);
            for (int i = 0; i < tmp.Count; i++)
            {
                item = tmp[i];
                Console.Out.WriteLine(item.path);
                sort_and_add(item);
            }
        }


        public void remove_items(Item item)
        {
            if (item.folder == "Video")
            {
                video.Items.Remove(item.path);
            }
            else if (item.folder == "Music")
            {
                music.Items.Remove(item.path);
            }
            else if (item.folder == "Image")
            {
                image.Items.Remove(item.path);
            }
        }

        public void clean_library(string file)
        {
            Item item;
            List<Item> tmp;
            Deserializer ds = new Deserializer();

            tmp = ds.deserialize(file);
            for (int i = 0; i < tmp.Count; i++)
            {
                item = tmp[i];
                remove_items(item);
            }
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

        public void Save_CurrentPlaylist(string file)
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
                while (s.check_file(location) == false && i < 10)
                {
                    Create_file(location);
                    location = "../../Playlists/MyPlaylist-" + ++i + ".xml";
                }
                s.serialize(playlist_tmp, location);
                //Console.WriteLine("[Folder-]\t[File-----------------------------------------------------------------]");
                //foreach (Item item in playlist_tmp)
                //{
                //    Console.WriteLine("####" + item.folder + "\t####" + item.path);
                //}
                //Console.WriteLine();
            }
            else
            {
                Create_file(playlist_tmp, location);
                Save_CurrentPlaylist(location);
            }
        }

        public void refresh(System.Windows.Controls.TreeView library)
        {
            library.Items.Refresh();
            image.Items.Refresh();
        }
    }
}
