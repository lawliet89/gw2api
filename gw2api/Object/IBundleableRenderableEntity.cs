using GW2NET.Common;

namespace gw2api.Object
{
    public interface IBundleableRenderableEntity<out T>
        where T : IRenderable
    {
        T Renderable { get; }
        byte[] Icon { set; }
    }
}
