using NikosAssets.Helpers;

namespace NikosAssets.Pooling.Marker
{
    /// <summary>
    /// A more lightweight wrapper class for the <typeparamref name="BasePoolMarkerManager"/>
    /// </summary>
    /// <typeparam name="TPoolMarkerManager"></typeparam>
    /// <typeparam name="TPoolItemMarker"></typeparam>
    public abstract class BasePoolMarkerManagerWrapper<TPoolMarkerManager, TPoolItemMarker> : BaseSingletonMono<TPoolMarkerManager>
        where TPoolMarkerManager : BasePoolMarkerManagerWrapper<TPoolMarkerManager, TPoolItemMarker>
        where TPoolItemMarker : BasePoolItemMarker<TPoolMarkerManager, TPoolItemMarker>
    {
        public abstract void PoolMarkerEnabled(TPoolItemMarker poolItemMarker);

        public abstract void PoolMarkerDisabled(TPoolItemMarker poolItemMarker, IPoolItem poolItem);
    }
}
