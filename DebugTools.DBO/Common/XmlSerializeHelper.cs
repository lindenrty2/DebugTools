using System.Windows.Shapes;
using System;
using System.IO;
using System.Xml.Serialization;

namespace DebugTools.DBO
{
    public class XmlSerializeHelper
    {
        public static bool Serialize<T>(string fileName, T value)
        {
            if (File.Exists(fileName))
                File.Delete(fileName);
            if (!Directory.Exists(System.IO.Path.GetDirectoryName(fileName)))
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(fileName));
            FileStream stream = File.OpenWrite(fileName);
            XmlSerializer xml = new XmlSerializer(typeof(T));
            try
            {
                xml.Serialize(stream, value);
            }
            catch (InvalidOperationException e)
            {
            }
            stream.Close();
            stream.Dispose();
            return true;
        }

        public static T Deserialize<T>(string fileName)
        {
            FileStream stream = File.OpenRead(fileName);
            XmlSerializer xml = new XmlSerializer(typeof(T));
            T result = default(T);
            try
            {
                result = (T)xml.Deserialize(stream);
            }
            catch (Exception e)
            {
            }
            stream.Close();
            stream.Dispose();
            return result;
        }

        public static T Copy<T>(T target)
        {
            MemoryStream stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(typeof(T));
            try
            {
                xml.Serialize(stream, target);
                stream.Position = 0;
                return (T)xml.Deserialize(stream);
            }
            catch (InvalidOperationException e)
            {
            }
            finally
            {
                stream.Close();
            }

            return default(T);
        }
    }
}
