namespace gw2api.Object
{
    public interface IHasIdentifier<out T>
    {
        T Identifier { get; }
    }
}
