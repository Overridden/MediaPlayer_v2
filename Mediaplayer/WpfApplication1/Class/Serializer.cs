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
        public void serialize(List<Item> library)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<Item>));
            using (StreamWriter wr = new StreamWriter("../../library.xml"))
            {
                xs.Serialize(wr, library);
            }
        }
    }
}
