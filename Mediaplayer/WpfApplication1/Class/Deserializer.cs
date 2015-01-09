using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WpfApplication1
{
    class Deserializer
    {
        public bool check_file(string file)
        {
            string curFile = file;
            return File.Exists(curFile);
        }

        public List<Item> deserialize(string file)
        {
            List<Item> library_v2;

            XmlSerializer dxs = new XmlSerializer(typeof(List<Item>));
            using (StreamReader rd = new StreamReader("../../library.xml"))
            {
                library_v2 = dxs.Deserialize(rd) as List<Item>;
            }
            return library_v2;
        }
    }
}
