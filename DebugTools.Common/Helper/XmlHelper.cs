using System.Xml;

namespace DebugTools.Helper
{
    public class XmlHelper
    {
        public static XmlAttribute CreateAttribute(XmlDocument doc, string name, object value)
        {
            XmlAttribute attribute = doc.CreateAttribute(name);
            if (value != null)
                attribute.Value = value.ToString();
            return attribute;
        }

        public static string GetAttributeValue(XmlNode node, string name)
        {
            XmlAttribute attribute = node.Attributes[name];
            if (attribute == null)
                return string.Empty;
            return attribute.Value;
        }

        public static bool GetBooleanAttributeValue(XmlNode node, string name, bool defaultValue)
        {
            XmlAttribute attribute = node.Attributes[name];
            if (attribute == null)
                return defaultValue;
            bool result;
            if (bool.TryParse(attribute.Value, out result))
                return result;
            else
                return defaultValue;
        }
    }
}
