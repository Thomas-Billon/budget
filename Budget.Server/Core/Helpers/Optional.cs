using Newtonsoft.Json;

namespace Budget.Server.Core.Helpers
{
    public class Optional<T>
    {
        private T? _value;

        public T? Value
        {
            get => _value;
            set { _value = value; IsSet = true; }
        }

        public bool IsSet { get; private set; }

        public static implicit operator Optional<T>(T? value) => new Optional<T> { Value = value };
    }

    public class OptionalJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsGenericType &&
                   objectType.GetGenericTypeDefinition() == typeof(Optional<>);
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            var objectType = value != null ? value.GetType() : typeof(object);
            var innerValue = objectType.GetProperty(nameof(Optional<object>.Value))?.GetValue(value);

            serializer.Serialize(writer, innerValue);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? _, JsonSerializer serializer)
        {
            var innerType = objectType.GenericTypeArguments[0];
            var innerValue = serializer.Deserialize(reader, innerType);
            var optionalWrapper = Activator.CreateInstance(objectType);

            objectType.GetProperty(nameof(Optional<object>.Value))?.SetValue(optionalWrapper, innerValue);

            return optionalWrapper;
        }
    }
}
