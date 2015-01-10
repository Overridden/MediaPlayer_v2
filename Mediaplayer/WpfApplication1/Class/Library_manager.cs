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
        public void Add_Item(int node_selected)
        {
            ofd = new OpenFileDialog();
            ofd.AddExtension = true;
            ofd.DefaultExt = "*.*";
            ofd.Filter = "Media Files (*.*)|*.*";
            ofd.ShowDialog();
            Console.Out.WriteLine("File selected " + ofd.FileName);
            serialize_and_add(node_selected, ofd.FileName);
        }

        private void serialize_and_add(int node_selected, string path)
        {
            // DESERIALIZE
            string file = "../../library.xml";
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
                serialize_and_add(node_selected, path);
            }
        }
    }
}
