using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace SubNet
{
    public static class SeDes
    {
        public static string ToXml(object obj)
        {
            XmlSerializer xs = new XmlSerializer(obj.GetType());
            MemoryStream ms = new MemoryStream();
            xs.Serialize(ms, obj);
            return Encoding.Default.GetString(ms.ToArray());
        }
        public static object ToObj(string xml, object obj)
        {
            XmlSerializer xs = new XmlSerializer(obj.GetType());
            MemoryStream ms = new MemoryStream(Encoding.Default.GetBytes(xml));
            return xs.Deserialize(ms);
            
        }
    }
}
