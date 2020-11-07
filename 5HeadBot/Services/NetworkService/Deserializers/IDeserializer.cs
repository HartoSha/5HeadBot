namespace _5HeadBot.Services.NetworkService.Deserializers
{
    public interface IDeserializer
    {
        public T DeserializeObject<T>(string value);

        public T DeserializeAnonymousType<T>(string value, T anonymousTypeObject);
    }
}
