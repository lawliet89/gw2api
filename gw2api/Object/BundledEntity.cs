namespace gw2api.Object
{
    public interface IBundledEntity
    {
        object Object { get; set; }
        object Identifier { get; }
    }

    public interface IBundledEntity<out TKey, TValue> : IBundledEntity
    {
        new TValue Object { get; set; }
        new TKey Identifier { get; }
    }
}
