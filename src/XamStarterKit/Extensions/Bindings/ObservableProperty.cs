namespace XamStarterKit.Extensions.Bindings
{
    public class ObservableProperty<T> : ObservableObject
    {
        private T _value;

        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                SetProperty(ref _value, value, nameof(Value));
            }
        }

        public ObservableProperty()
        {
            _value = default(T);
        }

        public ObservableProperty(T value)
        {
            _value = value;
        }

        public static implicit operator T(ObservableProperty<T> ins)
        {
            return ins.Value;
        }
    }
}