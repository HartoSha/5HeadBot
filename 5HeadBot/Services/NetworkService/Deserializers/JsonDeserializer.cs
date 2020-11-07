using Newtonsoft.Json;

namespace _5HeadBot.Services.NetworkService.Deserializers
{
    public class JsonDeserializer : IDeserializer
    {
        public T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
        public T DeserializeAnonymousType<T>(string value, T anonymousTypeObject)
        {
            return JsonConvert.DeserializeAnonymousType(value, anonymousTypeObject);
        }
    }
}
