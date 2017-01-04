using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mocoding.EasyDocDb.Yaml
{
    public class YamlSerializer : IDocumentSerializer
    {
        public string Type => "yml";

        public string Serialize<T>(T data)
            where T : class, new()
        {
            using (var stringWriter = new StringWriter())
            {
                var serializer = new YamlDotNet.Serialization.Serializer();
                serializer.Serialize(stringWriter, data);
                return stringWriter.ToString();
            }
        }

        public T Deserialize<T>(string content)
            where T : class, new()
        {
            using (var stringReader = new StringReader(content))
            {
                var serializer = new YamlDotNet.Serialization.Deserializer();
                return serializer.Deserialize<T>(stringReader);
            }
        }
    }
}
