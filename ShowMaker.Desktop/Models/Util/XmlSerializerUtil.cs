using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ShowMaker.Desktop.Util
{
    public static class XmlSerializerUtil
    {

        public static void SaveXml(object obj, string file)
        {
            using (XmlTextWriter writer = new XmlTextWriter(file, Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 4;
                XmlSerializer ser = new XmlSerializer(obj.GetType());

                ser.Serialize(writer, obj);
            }
        }

        public static object LoadXml(Type type, string file)
        {
            if (!File.Exists(file))
                return null;
            
            using (XmlTextReader sr = new XmlTextReader(file)) // 流方式读取XML
            {
                XmlSerializer serializer = new XmlSerializer(type);
                return serializer.Deserialize(sr);
            }
        }
    }
}
