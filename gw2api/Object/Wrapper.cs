namespace gw2api.Object
{
    public class Wrapper<TKey, TValue>
    {
        public TValue Object { get; set; }
        public TKey Identifier { get; set; }

        public Wrapper(TKey key)
        {
            Identifier = key;
        }

        public Wrapper(TValue value)
        {
            Object = value;
        }

        public Wrapper()
        {
        }

        public static explicit operator Wrapper<TKey, TValue>(TValue obj)
        {
            return new Wrapper<TKey, TValue>()
            {
                Object = obj
            };
        }

        public static explicit operator Wrapper<TKey, TValue>(TKey key)
        {
            return new Wrapper<TKey, TValue>()
            {
                Identifier = key
            };
        }
    }
}
