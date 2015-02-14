namespace gw2api.Object
{
    public interface IBundledEntity<out TKey, TValue>
    {
        TValue Object { get; set; }
        TKey Identifier { get; }
    }
}
