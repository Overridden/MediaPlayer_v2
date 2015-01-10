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
        private OpenFileDialog ofd;
        private string[] folder_name = new string[] { "Music", "Image", "Video" };
        private System.Windows.Controls.TreeViewItem f_library = new System.Windows.Controls.TreeViewItem() { Header = "Library" };
        private System.Windows.Controls.TreeViewItem image = new System.Windows.Controls.TreeViewItem() { Header = "Image" };
        private System.Windows.Controls.TreeViewItem video = new System.Windows.Controls.TreeViewItem() { Header = "Video" };
        private System.Windows.Controls.TreeViewItem music = new System.Windows.Controls.TreeViewItem() { Header = "Music" };

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
            Deserializer ds = new Deserializer();
            Serializer s = new Serializer();
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
                // CREATE FILE IF DOESNT EXIST
                Console.Out.WriteLine("Cannot find the file.\nCreating " + file);
                s.serialize(tmp, file);
                Console.Out.WriteLine("Success!");
                serialize_and_add(node_selected, path, file);
            }
        }

        public void init_library(System.Windows.Controls.TreeView library)
        {
            library.Items.Add(f_library);
            f_library.Items.Add(image);
            f_library.Items.Add(video);
            f_library.Items.Add(music);
        }

        public void fill_library(string file)
        {
            Item item;
            List<Item> tmp;
            Deserializer ds = new Deserializer();

            tmp = ds.deserialize(file);
            for (int i = 0; i < tmp.Count; i++)
            {
                item = tmp[i];
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
        }
    }
}
