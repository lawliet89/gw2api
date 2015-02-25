namespace gw2api.Object
{
    public interface IBundledEntity<out TKey, TValue> : IHasIdentifier<TKey>
    {
        TValue Object { get; set; }
    }
}
