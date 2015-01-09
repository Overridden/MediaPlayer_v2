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
        private string[] folder_name = new string[] { "Music", "Image", "Video" };
        public void Add_Item(int node_selected)
        {
            OpenFileDialog ofd;
            ofd = new OpenFileDialog();
            ofd.AddExtension = true;
            ofd.DefaultExt = "*.*";
            ofd.Filter = "Media Files (*.*)|*.*";
            ofd.ShowDialog();
            Console.Out.WriteLine("Opening file name " + ofd.FileName);

            // DESERIALIZE
            Deserializer ds = new Deserializer();
            Console.Out.WriteLine("Checking file ../../library.xml");
            if (ds.check_file("../../library.xml") == true)
            {
                Console.Out.WriteLine("Deserializing...");
                List<Item> tmp;
                tmp = ds.deserialize("../../library.xml");

                // FILL THE LIBRARY

                Console.Out.WriteLine("Adding item...");
                Item item1 = new Item
                {
                    folder = folder_name[node_selected],
                    path = ofd.FileName
                };
                tmp.Add(item1);

                // SERIALIZE

                Console.Out.WriteLine("Serializing...");
                Serializer s = new Serializer();
                s.serialize(tmp);

                Console.WriteLine();
                foreach (Item item in tmp)
                {
                    Console.WriteLine("####" + item.folder + "####" + item.path);
                }
            }
        }
    }
}
