using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace XML_WS_AgencyApp.Helpers
{
    public class XMLHelper
    {
        public string SerializeToXml<T>(T myObject)
        {
            Stream s = new MemoryStream();
            XmlWriter writer = new XmlTextWriter(s, Encoding.UTF8);
            XmlSerializer serializer = GetXmlSerializer<T>(myObject);
            writer.WriteStartElement("root");
            serializer.Serialize(writer, myObject);
            writer.WriteEndElement();

            s.Position = 0;
            StreamReader sr = new StreamReader(s);
            string data = sr.ReadToEnd();

            return data;
        }

        public XmlSerializer GetXmlSerializer<T>(T myObject)
        {
            XmlTypeMapping myTypeMapping = new SoapReflectionImporter().ImportTypeMapping(myObject.GetType());
            return new XmlSerializer(myTypeMapping);
        }
    }
}