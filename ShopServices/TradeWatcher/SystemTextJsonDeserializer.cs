using System.Text.Json;
using Confluent.Kafka;

namespace TradeWatcher
{
    internal class SystemTextJsonDeserializer<T> : IDeserializer<T>
    {
        private readonly JsonSerializerOptions _options;

        public SystemTextJsonDeserializer() : this(null) { }

        public SystemTextJsonDeserializer(
            JsonSerializerOptions options)
        {
            _options = options;
        }

        public T Deserialize(
            ReadOnlySpan<byte> data,
            bool isNull,
            SerializationContext context)
        {
            if (isNull)
                return default!;

            return JsonSerializer.Deserialize<T>(data, _options)!;
        }
    }
}
