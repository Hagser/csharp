using System;
using System.Xml.Serialization;
using System.IO;

namespace GoodStuff
{
    public class serialize
    {

        public static void serializeObject(string filename, object obj, Type type)
        {
            XmlSerializer ser = new XmlSerializer(type);
            using (TextWriter writer = new StreamWriter(filename))
            {
                ser.Serialize(writer, obj);
                writer.Close();
            }
        }
        public static object deserializeObject(string filename, object obj, Type type)
        {
            object r_obj = new object();
            XmlSerializer ser = new XmlSerializer(type);
            if (File.Exists(filename))
            {
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    r_obj = ser.Deserialize(fs);
                    fs.Close();
                }
            }
            return r_obj;
        }

    }
}
