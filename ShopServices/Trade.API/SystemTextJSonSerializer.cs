using System.Text.Json;
using Confluent.Kafka;

namespace Trade.API
{
    internal class SystemTextJSonSerializer<T> : ISerializer<T>
    {
        private readonly JsonSerializerOptions _options;
        public SystemTextJSonSerializer() : this(null) { }

        public SystemTextJSonSerializer(
            JsonSerializerOptions options)
        {
            _options = options;
        }

        public byte[] Serialize(T data, SerializationContext context)
        {
            return JsonSerializer.SerializeToUtf8Bytes<T>(data, _options);
        }
    }
}
