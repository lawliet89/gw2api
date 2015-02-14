using System.Collections.Generic;
using gw2api.Object;
using GW2NET.Common;

namespace gw2api.Request
{
    public interface IBundleableRenderable<out T>
        where T : IRenderable
    {
        IEnumerable<IBundleableRenderableEntity<T>> Renderables { get; }
    }
}
