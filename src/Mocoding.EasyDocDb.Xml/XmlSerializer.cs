using System.IO;

namespace Mocoding.EasyDocDb.Xml
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class XmlSerializer : IDocumentSerializer
    {
        public string Type => "xml";

        public string Serialize<T>(T data)
            where T : class, new()
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, data);
                return writer.ToString();
            }
        }

        public T Deserialize<T>(string content)
            where T : class, new()
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (var reader = new StringReader(content))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}
