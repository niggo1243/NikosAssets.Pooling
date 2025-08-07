using NaughtyAttributes;
using NikosAssets.Helpers;

namespace NikosAssets.Pooling.Marker
{
    public abstract class BasePoolItemMarker<TPoolMarkerManager, TPoolItemMarker> : BaseNotesMono
        where TPoolMarkerManager : BasePoolMarkerManagerWrapper<TPoolMarkerManager, TPoolItemMarker>
        where TPoolItemMarker : BasePoolItemMarker<TPoolMarkerManager, TPoolItemMarker>
    {
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        public string poolContainerKey;

        public IPoolItem DesignatedPoolItem { get; set; }
        
        protected abstract void OnEnable();

        protected abstract void OnDisable();
    }
}
