using NikosAssets.Helpers;

namespace NikosAssets.Pooling.Marker
{
    public abstract class BasePoolMarkerManagerWrapper<TPoolMarkerManager, TPoolItemMarker> : BaseSingletonMono<TPoolMarkerManager>
        where TPoolMarkerManager : BasePoolMarkerManagerWrapper<TPoolMarkerManager, TPoolItemMarker>
        where TPoolItemMarker : BasePoolItemMarker<TPoolMarkerManager, TPoolItemMarker>
    {
        public abstract void PoolMarkerEnabled(TPoolItemMarker poolItemMarker);

        public abstract void PoolMarkerDisabled(TPoolItemMarker poolItemMarker, IPoolItem poolItem);
    }
}
