using Newtonsoft.Json;

namespace Mocoding.EasyDocDb.Json
{
    public class JsonSerializer : IDocumentSerializer
    {
        private readonly JsonSerializerSettings _settings;

        public JsonSerializer()
            : this(new JsonSerializerSettings())
        {
        }

        public JsonSerializer(JsonSerializerSettings settings)
        {
            _settings = settings;
        }

        public string Type => "json";

        public string Serialize<T>(T data)
            where T : class, new()
        {
            return JsonConvert.SerializeObject(data, _settings);
        }

        public T Deserialize<T>(string content)
            where T : class, new()
        {
            var t = new T();
            JsonConvert.PopulateObject(content, t);
            return t;
        }
    }
}
