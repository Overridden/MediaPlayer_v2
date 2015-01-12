using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WpfApplication1
{
    class Serializer
    {
        public bool check_file(string file)
        {
            string curFile = file;
            return File.Exists(curFile);
        }

        public void serialize(List<Item> library, string file)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<Item>));
            using (StreamWriter wr = new StreamWriter(file))
            {
                xs.Serialize(wr, library);
            }
        }
    }
}
