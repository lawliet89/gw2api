namespace gw2api.Object
{
    public interface IBundledEntity<TKey, TValue>
    {
        TValue Object { get; set; }
        TKey Identifier { get; set; }
    }
}
