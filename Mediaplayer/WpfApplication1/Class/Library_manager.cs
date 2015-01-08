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
            Item item = new Item
            {
                folder = folder_name[node_selected],
                path = ofd.FileName
            };
            XmlSerializer xs = new XmlSerializer(typeof(Item));
            using (StreamWriter wr = new StreamWriter("../../library.xml"))
            {
                xs.Serialize(wr, item);
            }
        }
    }
}
