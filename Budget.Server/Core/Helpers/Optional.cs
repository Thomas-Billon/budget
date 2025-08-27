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
}
