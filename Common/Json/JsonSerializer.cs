using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Common.Json
{
    public class JsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerSettings _settings;

        public JsonSerializer(JsonSerializerSettings settings = null)
        {
            _settings = settings ?? new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include, ContractResolver = new CamelCasePropertyNamesContractResolver()};
        }

        public Task<string> SerializeAsync<T>(T obj)
        {
            return Task.Run(() => Serialize(obj));
        }

        public Task<T> DeserializeAsync<T>(string raw)
        {
            if (string.IsNullOrEmpty(raw) || raw == "null")
            {
                return Task.FromResult(default(T));
            }
            return Task.Run(() => Deserialize<T>(raw));
        }

        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, _settings);
        }

        public T Deserialize<T>(string raw)
        {
            return JsonConvert.DeserializeObject<T>(raw, _settings);
        }
    }
}
