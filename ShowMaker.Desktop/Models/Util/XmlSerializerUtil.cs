using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ShowMaker.Desktop.Util
{
    public static class XmlSerializerUtil
    {

        /// <summary>
        /// Object->XML文件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="file"></param>
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

        /// <summary>
        /// Object->XML字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="file"></param>
        public static string SaveXmlToString(object obj)
        {
            XmlSerializer xs = new XmlSerializer(obj.GetType());
            MemoryStream stream = new MemoryStream();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UTF8Encoding(false);
            settings.Indent = true;
            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                xs.Serialize(writer, obj);
            }
            return Encoding.UTF8.GetString(stream.ToArray());
        }

        /// <summary>
        /// XML文件->Object
        /// </summary>
        /// <param name="type"></param>
        /// <param name="file"></param>
        /// <returns></returns>
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
