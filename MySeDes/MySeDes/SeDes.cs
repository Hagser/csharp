using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace MySeDes
{
    public static class SeDes
    {
        /// <summary>
        /// Serializes and object and returns xml-code.
        /// </summary>
        /// <param name="obj">Object to Serialize.</param>
        /// <returns>Xml-code</returns>
        public static string ToXml(object obj)
        {
            XmlSerializer xs = new XmlSerializer(obj.GetType());
            MemoryStream ms = new MemoryStream();
            xs.Serialize(ms, obj);
            return Encoding.Default.GetString(ms.ToArray());
        }
        /// <summary>
        /// Deserializes an object from xml-code. 
        /// </summary>
        /// <param name="xml">Xml-code to be desiralized.</param>
        /// <param name="obj">Object to be loaded.</param>
        /// <returns>Object</returns>
        public static object ToObj(string xml, object obj)
        {
            XmlSerializer xs = new XmlSerializer(obj.GetType());
            MemoryStream ms = new MemoryStream(Encoding.Default.GetBytes(xml));
            return xs.Deserialize(ms);
        }
        /// <summary>
        /// Loads an object from a deserialized xml-file.
        /// </summary>
        /// <param name="FilePath">Path to an existing xml-file.</param>
        /// <returns>Object</returns>
        public static object LoadFromXml(string FilePath, object obj)
        {
            if (File.Exists(FilePath))
            {
                string xml = File.ReadAllText(FilePath);
                obj = (ToObj(xml, obj));
            }
            return obj;
        }
        /// <summary>
        /// Serializes an object to xml.
        /// </summary>
        /// <param name="FilePath">Path to a xml-file.</param>
        /// <param name="obj">Object to be saved.</param>
        public static void SaveToXml(string FilePath,object obj)
        {
            string xmlser = ToXml(obj);
            File.WriteAllText(FilePath, xmlser);
        }
    }
}
