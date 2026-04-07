using Newtonsoft.Json;

namespace Assets._Project.Develop.Runtime.Utilities.DataManagment.Serializers
{
    public class JsonSerializer : IDataSerializer
    {
        // Runtime
        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Auto
        };

        public JsonSerializer()
        { }

        public JsonSerializer(JsonSerializerSettings settings)
            => _serializerSettings = settings;

        public string Serialize<TData>(TData data)
            => JsonConvert.SerializeObject(data, _serializerSettings);

        public TData Deserialize<TData>(string serializedData)
            => JsonConvert.DeserializeObject<TData>(serializedData, _serializerSettings);
    }
}
